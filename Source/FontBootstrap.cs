// ==== ./Source/FontBootstrap.cs ====
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BetterRimworlds
{
    public static class FontBootstrap
    {
        public static Font LoadedFont;
        private static FontLanguageConfig _activeConfig;

        private static readonly Dictionary<string, FontLanguageConfig> LanguageFonts =
            new Dictionary<string, FontLanguageConfig>
            {
                ["Hindi"] = new FontLanguageConfig(
                    language: "Hindi",
                    fontSearchNames: new[]
                    {
                        "Noto Sans Devanagari",
                        "Noto Sans Devanagari Regular",
                        "NotoSansDevanagari-Regular",
                    },
                    packageName: "noto-fonts-extra",
                    testChars: "अआइईउऊकखगघचछजझटठडढणतथदधनपफबभमयरलवशषसहािीुूृेैोौंःँ्",
                    installInstructions:
                        "  Arch Linux:  sudo pacman -S noto-fonts-extra\n" +
                        "  Ubuntu/Deb:  sudo apt install fonts-noto-core\n" +
                        "  Fedora:      sudo dnf install google-noto-sans-devanagari-fonts\n" +
                        "  Windows:     Install 'Noto Sans Devanagari' from fonts.google.com\n" +
                        "  macOS:       brew install --cask font-noto-sans-devanagari"
                ),
                ["Bengali"] = new FontLanguageConfig(
                    language: "Bengali",
                    fontSearchNames: new[]
                    {
                        "Noto Sans Bengali",
                        "Noto Sans Bengali Regular",
                        "NotoSansBengali-Regular",
                    },
                    packageName: "noto-fonts-extra",
                    testChars: "অআইঈউঊএঐওঔকখগঘঙচছজঝঞটঠডঢণতথদধনপফবভমযরলশষসহািীুূৃেৈোৌংঃঁ্",
                    installInstructions:
                        "  Arch Linux:  sudo pacman -S noto-fonts-extra\n" +
                        "  Ubuntu/Deb:  sudo apt install fonts-noto-core\n" +
                        "  Fedora:      sudo dnf install google-noto-sans-bengali-fonts\n" +
                        "  Windows:     Install 'Noto Sans Bengali' from fonts.google.com\n" +
                        "  macOS:       brew install --cask font-noto-sans-bengali"
                ),
                ["Tamil"] = new FontLanguageConfig(
                    language: "Tamil",
                    fontSearchNames: new[]
                    {
                        "Noto Sans Tamil",
                        "Noto Sans Tamil Regular",
                        "NotoSansTamil-Regular",
                    },
                    packageName: "noto-fonts-extra",
                    testChars: "அஆஇஈஉஊஎஏஐஒஓஔகஙசஜஞடணதநனபமயரறலளழவஷஸஹாிீுூெேைொோௌ்",
                    installInstructions:
                        "  Arch Linux:  sudo pacman -S noto-fonts-extra\n" +
                        "  Ubuntu/Deb:  sudo apt install fonts-noto-core\n" +
                        "  Fedora:      sudo dnf install google-noto-sans-tamil-fonts\n" +
                        "  Windows:     Install 'Noto Sans Tamil' from fonts.google.com\n" +
                        "  macOS:       brew install --cask font-noto-sans-tamil"
                ),
                ["Arabic"] = new FontLanguageConfig(
                    language: "Arabic",
                    fontSearchNames: new[]
                    {
                        "Noto Sans Arabic",
                        "Noto Sans Arabic Regular",
                        "NotoSansArabic-Regular",
                    },
                    packageName: "noto-fonts-extra",
                    testChars: "ابتثجحخدذرزسشصضطظعغفقكلمنهويءآأإؤئ",
                    installInstructions:
                        "  Arch Linux:  sudo pacman -S noto-fonts-extra\n" +
                        "  Ubuntu/Deb:  sudo apt install fonts-noto-core\n" +
                        "  Fedora:      sudo dnf install google-noto-sans-arabic-fonts\n" +
                        "  Windows:     Install 'Noto Sans Arabic' from fonts.google.com\n" +
                        "  macOS:       brew install --cask font-noto-sans-arabic"
                ),
            };

        public static void Init(string language)
        {
            if (!LanguageFonts.TryGetValue(language, out _activeConfig))
            {
                Log.Error(
                    $"[BetterRimworlds] No font config for language '{language}'. " +
                    $"Supported: {string.Join(", ", LanguageFonts.Keys)}"
                );
                return;
            }

            try
            {
                string match = FindSystemFont(_activeConfig);

                if (match == null)
                {
                    Log.Error(
                        $"[BetterRimworlds] ERROR: No suitable font found " +
                        $"for {_activeConfig.Language}.\n\n" +
                        $"Please install a {_activeConfig.Language} font:\n" +
                        $"{_activeConfig.InstallInstructions}\n\n" +
                        "Then restart RimWorld."
                    );
                    return;
                }

                Log.Message(
                    $"[BetterRimworlds:{_activeConfig.Language}] " +
                    $"Found system font: '{match}'"
                );

                LoadedFont = Font.CreateDynamicFontFromOSFont(match, 16);

                if (LoadedFont == null || !LoadedFont.dynamic)
                {
                    Log.Error(
                        $"[BetterRimworlds:{_activeConfig.Language}] " +
                        $"Failed to create dynamic font. " +
                        $"name={LoadedFont?.name}, dynamic={LoadedFont?.dynamic}"
                    );
                    LoadedFont = null;
                    return;
                }

                Log.Message(
                    $"[BetterRimworlds:{_activeConfig.Language}] Loaded: " +
                    $"name={LoadedFont.name}, " +
                    $"dynamic={LoadedFont.dynamic}, " +
                    $"lineHeight={LoadedFont.lineHeight}"
                );

                WarmUpFont(LoadedFont, _activeConfig);
                DumpFontInfo(LoadedFont, _activeConfig);
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"[BetterRimworlds:{_activeConfig.Language}] " +
                    $"Font init failed: {ex}"
                );
            }
        }

        public static bool ShouldUseCustomFont()
        {
            return _activeConfig != null
                && LanguageDatabase.activeLanguage?.folderName == _activeConfig.Language
                && LoadedFont != null
                && LoadedFont.dynamic;
        }

        private static string FindSystemFont(FontLanguageConfig config)
        {
            string[] osFonts = Font.GetOSInstalledFontNames();

            // Exact match first
            foreach (var candidate in config.FontSearchNames)
            {
                if (osFonts.Contains(candidate))
                    return candidate;
            }

            // Substring match fallback
            string primary = config.FontSearchNames[0];
            foreach (var osFont in osFonts)
            {
                if (osFont.IndexOf(primary, StringComparison.OrdinalIgnoreCase) >= 0)
                    return osFont;
            }

            return null;
        }

        private static void WarmUpFont(Font font, FontLanguageConfig config)
        {
            try
            {
                font.RequestCharactersInTexture(
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
                    "0123456789 !?.,:;()-_+/\\[]{}<>=*\"'|" +
                    config.TestChars,
                    18,
                    FontStyle.Normal
                );

                Log.Message(
                    $"[BetterRimworlds:{config.Language}] Warmed up font texture."
                );
            }
            catch (Exception ex)
            {
                Log.Warning(
                    $"[BetterRimworlds:{config.Language}] WarmUpFont failed: {ex}"
                );
            }
        }

        private static void DumpFontInfo(Font font, FontLanguageConfig config)
        {
            try
            {
                Log.Message(
                    $"[BetterRimworlds:{config.Language}] Font info: " +
                    $"name={font.name}, " +
                    $"lineHeight={font.lineHeight}, " +
                    $"dynamic={font.dynamic}"
                );

                // Test first 8 script-specific chars
                var testChars = config.TestChars;
                var results = new List<string>();
                for (int i = 0; i < Math.Min(8, testChars.Length); i++)
                {
                    char c = testChars[i];
                    results.Add($"U+{(int)c:X4}={font.HasCharacter(c)}");
                }

                Log.Message(
                    $"[BetterRimworlds:{config.Language}] Glyph test: " +
                    $"A={font.HasCharacter('A')}, " +
                    $"1={font.HasCharacter('1')}, " +
                    string.Join(", ", results)
                );
            }
            catch (Exception ex)
            {
                Log.Warning(
                    $"[BetterRimworlds:{config.Language}] DumpFontInfo failed: {ex}"
                );
            }
        }
    }

    public class FontLanguageConfig
    {
        public string Language { get; }
        public string[] FontSearchNames { get; }
        public string PackageName { get; }
        public string TestChars { get; }
        public string InstallInstructions { get; }

        public FontLanguageConfig(
            string language,
            string[] fontSearchNames,
            string packageName,
            string testChars,
            string installInstructions)
        {
            Language = language;
            FontSearchNames = fontSearchNames;
            PackageName = packageName;
            TestChars = testChars;
            InstallInstructions = installInstructions;
        }
    }
}
