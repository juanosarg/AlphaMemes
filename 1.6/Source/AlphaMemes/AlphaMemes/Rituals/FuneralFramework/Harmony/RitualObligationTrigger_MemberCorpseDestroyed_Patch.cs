using HarmonyLib;
using RimWorld;

namespace AlphaMemes
{
    public static class RitualObligationTrigger_MemberCorpseDestroyed_Patch
    {
        // Prevent the vanilla no-corpse funeral, since AM adds its own one.
        // In previous versions this was not needed as a different patch completely
        // prevented it from running, but that patch was very destructive and removed.
        [HarmonyPatch(typeof(RitualObligationTrigger_MemberCorpseDestroyed))]
        [HarmonyPatch(nameof(RitualObligationTrigger_MemberCorpseDestroyed.Notify_MemberCorpseDestroyed))]
        public static class FuneralFramework_RitualObligationTrigger_MemberCorpseDestroyed
        {
            static bool Prefix(RitualObligationTrigger_MemberCorpseDestroyed __instance)
            {
                return __instance.ritual?.def != PreceptDefOf.Funeral;
            }
        }
    }
}