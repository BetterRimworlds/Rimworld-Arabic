// ==== ./Source/Main.cs ====
using HarmonyLib;
using Verse;

namespace BetterRimworlds
{
    public class RimworldArabicMod : Mod
    {
        public const string Language = "Arabic";

        public RimworldArabicMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony(
                $"HopeSeekr.BetterRimworlds.Rimworld{Language}"
            );
            harmony.PatchAll();

            Log.Message($"[BetterRimworlds:{Language}] Harmony patches applied.");
        }
    }
}
