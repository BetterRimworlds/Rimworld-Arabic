using System.Text.RegularExpressions;
using Verse;

namespace BetterRimworlds
{
    public class LanguageWorker_Arabic : LanguageWorker
    {
        // RimWorld calls these for grammar templates.
        // Arabic:
        // - Indefinite article: none (indefiniteness is absence of "ال" / tanwin, but game text usually omits diacritics)
        // - Definite article: "ال"
        public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
        {
            if (str.NullOrEmpty())
                return "";

            // If it's a name, leave it alone. (Same intent as base)
            if (name)
                return str;

            // Arabic has no indefinite article; just return the noun phrase as-is.
            return str;
        }

        public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
        {
            if (str.NullOrEmpty())
                return "";

            if (name)
                return str;

            // Avoid double-definiteness if translation already starts with "ال"
            // and avoid attaching to placeholders/tokens.
            if (StartsWithArabicDefiniteArticle(str) || LooksLikeToken(str))
                return str;

            return "ال" + str;
        }

        public override string PostProcessed(string str)
        {
            return PostProcessedInt(base.PostProcessed(str));
        }
        
        private static string FixArabicForRimWorld(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var tokens = new Dictionary<string, string>();
            int tokenIndex = 0;

            // 1. Protect tokens
            string protectedText = Regex.Replace(input, @"(\{.*?\}|\[.*?\]|<.*?>)", match =>
            {
                string key = $"@{tokenIndex++}@"; // Shorter key is safer
                tokens[key] = match.Value;
                return key;
            });

            // 2. Shape the characters (connect the letters)
            string shaped = ArabicShaper.Shape(protectedText);

            // 3. REVERSE the string (Crucial for RimWorld's LTR engine)
            char[] charArray = shaped.ToCharArray();
            Array.Reverse(charArray);
            string reversed = new string(charArray);

            // 4. Restore tokens and FIX their internal order
            foreach (var kvp in tokens)
            {
                // Since the whole string was reversed, our token key "@0@" 
                // is now "@0@". We need to find that and replace it with the 
                // ORIGINAL (un-reversed) token so {0} doesn't become }0{
                string reversedKey = new string(kvp.Key.ToCharArray().Reverse().ToArray());
                reversed = reversed.Replace(reversedKey, kvp.Value);
            }

            return reversed;
        }
        
        private string PostProcessedInt(string str)
        {
            if (str.NullOrEmpty())
                return str;

            // Normalize spacing around Arabic definite article and common preposition+article joins.
            // Note: we keep this conservative to avoid breaking legitimate text.
            str = str
                // Collapse weird splits like "ال رجل" -> "الرجل"
                .Replace("ال ", "ال")
                // Join "ب ال" -> "بال" and "ل ال" -> "لل"
                .Replace("ب ال", "بال")
                .Replace("ل ال", "لل")
                // Frequently appears from templates: "في ال..." etc.
                .Replace("في ال", "في الـ")
                .Replace("من ال", "من الـ")
                .Replace("على ال", "على الـ")
                .Replace("إلى ال", "إلى الـ");

            // return FixArabicForRimWorld(str);
            return str;
        }

        // ---------- Arabic number cases ----------
        //
        // RimWorld default supports 3 forms: one / several / many.
        // Arabic needs more. We can still accept variable arg lengths and choose intelligently.
        //
        // Recommended args count for Arabic (6 forms):
        // 0: zero   (0)
        // 1: one    (1)
        // 2: two    (2)
        // 3: few    (3-10)
        // 4: many   (11-99)
        // 5: other  (100+ / fallback)
        //
        // If only 3 args are provided, we degrade gracefully.
        public override string ResolveNumCase(float number, List<string> args)
        {
            if (args == null || args.Count == 0)
                return null;

            // Unquote forms like the base method does
            List<string> forms = new List<string>(args.Count);
            for (int i = 0; i < args.Count; i++)
                forms.Add(args[i]?.Trim('\'') ?? "");

            // If fractional, behave like base: number + "several-like" form.
            // Arabic fractional quantities usually take singular (or "many"/generic), but game text tends to treat as "several".
            if (number - (float)Math.Floor(number) > float.Epsilon)
            {
                string fracForm = PickFractionForm(forms);
                return $"{number} {fracForm}";
            }

            int n = (int)number;
            string form = GetArabicFormForNumber(n, forms);
            return $"{n} {form}";
        }

        protected override string GetFormForNumber(int num, string formOne, string formSeveral, string formMany)
        {
            // This method is used by the base ResolveNumCase pipeline.
            // If any older content relies on 3 forms, keep a reasonable Arabic mapping:
            // 1 -> formOne
            // 2-10 -> formSeveral
            // else -> formMany
            if (num == 1)
                return formOne;

            int mod100 = num % 100;
            if (mod100 >= 2 && mod100 <= 10)
                return formSeveral;

            return formMany;
        }

        private static string GetArabicFormForNumber(int n, List<string> forms)
        {
            // If the caller only provided 1-3 forms, degrade gracefully.
            // 1 form: always use it
            // 2 forms: 1 vs other
            // 3 forms: use the 3-form mapping via Arabic-ish rules
            if (forms.Count == 1)
                return forms[0];

            if (forms.Count == 2)
                return (n == 1) ? forms[0] : forms[1];

            if (forms.Count == 3)
            {
                // forms[0]=one, forms[1]=several, forms[2]=many
                if (n == 1) return forms[0];
                int mod100 = n % 100;
                if (mod100 >= 2 && mod100 <= 10) return forms[1];
                return forms[2];
            }

            // 4+ forms: try to use Arabic plural categories.
            // If 6 provided, follow the 6-form scheme; if 4-5, best-effort.
            int mod100_ = n % 100;

            // zero
            if (n == 0)
                return forms[0];

            // one
            if (n == 1)
                return forms.Count > 1 ? forms[1] : forms[0];

            // two
            if (n == 2)
                return forms.Count > 2 ? forms[2] : forms[Math.Min(1, forms.Count - 1)];

            // few: 3-10 (and also 103-110 etc based on mod100)
            if (mod100_ >= 3 && mod100_ <= 10)
                return forms.Count > 3 ? forms[3] : forms[Math.Min(2, forms.Count - 1)];

            // many: 11-99
            if (mod100_ >= 11 && mod100_ <= 99)
                return forms.Count > 4 ? forms[4] : forms[Math.Min(3, forms.Count - 1)];

            // other: 100, 101, 102, 200, ...
            return forms.Count > 5 ? forms[5] : forms[forms.Count - 1];
        }

        private static string PickFractionForm(List<string> forms)
        {
            // Fractional quantities in Arabic usually prefer singular or a generic plural.
            // Here: prefer "many"/generic if available, otherwise last.
            if (forms.Count >= 5 && !forms[4].NullOrEmpty())
                return forms[4];
            return forms[forms.Count - 1];
        }

        // ---------- Helpers ----------
        private static bool StartsWithArabicDefiniteArticle(string s)
        {
            if (s.NullOrEmpty() || s.Length < 2)
                return false;

            // "ال" (U+0627 U+0644)
            return s[0] == 'ا' && s[1] == 'ل';
        }

        private static bool LooksLikeToken(string s)
        {
            // Avoid article attachment to things like [PAWN_nameDef], {0}, <tag>, etc.
            if (s.NullOrEmpty())
                return false;

            char c = s[0];
            return c == '[' || c == '{' || c == '<' || c == '(' || c == '"' || c == '\'';
        }

        public override string OrdinalNumber(int number, Gender gender = Gender.None)
        {
            // Safe, readable ordinals for UI. You can expand later.
            // NOTE: Arabic ordinals are gendered; these are the most common UI forms.
            if (gender == Gender.Female)
            {
                return number switch
                {
                    1 => "الأولى",
                    2 => "الثانية",
                    3 => "الثالثة",
                    4 => "الرابعة",
                    5 => "الخامسة",
                    6 => "السادسة",
                    7 => "السابعة",
                    8 => "الثامنة",
                    9 => "التاسعة",
                    10 => "العاشرة",
                    _ => "الـ" + number + "ة"
                };
            }

            return number switch
            {
                1 => "الأول",
                2 => "الثاني",
                3 => "الثالث",
                4 => "الرابع",
                5 => "الخامس",
                6 => "السادس",
                7 => "السابع",
                8 => "الثامن",
                9 => "التاسع",
                10 => "العاشر",
                _ => "الـ" + number
            };
        }
    }
}

