using System.Text;

namespace BetterRimworlds;

internal static class ArabicShaper
{
    private static readonly Dictionary<char, char[]> Map = new()
    {
        // format: isolated, final, initial, medial

        ['ا'] = new[] { '\uFE8D', '\uFE8E', '\0', '\0' },
        ['ب'] = new[] { '\uFE8F', '\uFE90', '\uFE91', '\uFE92' },
        ['ت'] = new[] { '\uFE95', '\uFE96', '\uFE97', '\uFE98' },
        ['ث'] = new[] { '\uFE99', '\uFE9A', '\uFE9B', '\uFE9C' },
        ['ج'] = new[] { '\uFE9D', '\uFE9E', '\uFE9F', '\uFEA0' },
        ['ح'] = new[] { '\uFEA1', '\uFEA2', '\uFEA3', '\uFEA4' },
        ['خ'] = new[] { '\uFEA5', '\uFEA6', '\uFEA7', '\uFEA8' },
        ['د'] = new[] { '\uFEA9', '\uFEAA', '\0', '\0' },
        ['ذ'] = new[] { '\uFEAB', '\uFEAC', '\0', '\0' },
        ['ر'] = new[] { '\uFEAD', '\uFEAE', '\0', '\0' },
        ['ز'] = new[] { '\uFEAF', '\uFEB0', '\0', '\0' },
        ['س'] = new[] { '\uFEB1', '\uFEB2', '\uFEB3', '\uFEB4' },
        ['ش'] = new[] { '\uFEB5', '\uFEB6', '\uFEB7', '\uFEB8' },
        ['ص'] = new[] { '\uFEB9', '\uFEBA', '\uFEBB', '\uFEBC' },
        ['ض'] = new[] { '\uFEBD', '\uFEBE', '\uFEBF', '\uFEC0' },
        ['ط'] = new[] { '\uFEC1', '\uFEC2', '\uFEC3', '\uFEC4' },
        ['ظ'] = new[] { '\uFEC5', '\uFEC6', '\uFEC7', '\uFEC8' },
        ['ع'] = new[] { '\uFEC9', '\uFECA', '\uFECB', '\uFECC' },
        ['غ'] = new[] { '\uFECD', '\uFECE', '\uFECF', '\uFED0' },
        ['ف'] = new[] { '\uFED1', '\uFED2', '\uFED3', '\uFED4' },
        ['ق'] = new[] { '\uFED5', '\uFED6', '\uFED7', '\uFED8' },
        ['ك'] = new[] { '\uFED9', '\uFEDA', '\uFEDB', '\uFEDC' },
        ['ل'] = new[] { '\uFEDD', '\uFEDE', '\uFEDF', '\uFEE0' },
        ['م'] = new[] { '\uFEE1', '\uFEE2', '\uFEE3', '\uFEE4' },
        ['ن'] = new[] { '\uFEE5', '\uFEE6', '\uFEE7', '\uFEE8' },
        ['ه'] = new[] { '\uFEE9', '\uFEEA', '\uFEEB', '\uFEEC' },
        ['و'] = new[] { '\uFEED', '\uFEEE', '\0', '\0' },
        ['ي'] = new[] { '\uFEF1', '\uFEF2', '\uFEF3', '\uFEF4' },
        ['ى'] = new[] { '\uFEEF', '\uFEF0', '\0', '\0' },
    };

    public static string Shape(string input)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];

            if (!Map.ContainsKey(current))
            {
                sb.Append(current);
                continue;
            }

            bool connectsBefore = i > 0 && Map.ContainsKey(input[i - 1]) && Map[input[i - 1]][2] != '\0';
            bool connectsAfter = i < input.Length - 1 && Map.ContainsKey(input[i + 1]) && Map[current][2] != '\0';

            var forms = Map[current];

            if (connectsBefore && connectsAfter && forms[3] != '\0')
                sb.Append(forms[3]); // medial
            else if (connectsBefore && forms[1] != '\0')
                sb.Append(forms[1]); // final
            else if (connectsAfter && forms[2] != '\0')
                sb.Append(forms[2]); // initial
            else
                sb.Append(forms[0]); // isolated
        }

        return sb.ToString();
    }
}
