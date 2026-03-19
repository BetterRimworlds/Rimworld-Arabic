// ==== ./Source/DebugPatches.cs ====
using HarmonyLib;
using RimWorld;
using Verse;

namespace BetterRimworlds
{
    [HarmonyPatch(typeof(MainMenuDrawer), "DoMainMenuControls")]
    public static class Patch_MainMenuDrawer_Debug
    {
        private static bool _logged;

        public static void Prefix()
        {
            if (_logged)
                return;
            _logged = true;

            if (FontBootstrap.LoadedFont == null)
            {
                Log.Warning(
                    $"[BetterRimworlds:{RimworldArabicMod.Language}] " +
                    "DebugPatch: LoadedFont is null."
                );
                return;
            }

            Log.Message(
                $"[BetterRimworlds:{RimworldArabicMod.Language}] DebugPatch: " +
                $"ActiveLanguage={LanguageDatabase.activeLanguage?.folderName}, " +
                $"Font={FontBootstrap.LoadedFont.name}, " +
                $"dynamic={FontBootstrap.LoadedFont.dynamic}"
            );
        }
    }
}
