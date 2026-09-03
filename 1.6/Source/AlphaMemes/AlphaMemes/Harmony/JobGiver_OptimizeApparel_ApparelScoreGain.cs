using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace AlphaMemes
{
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "ApparelScoreGain")]
    internal class AlphaMemes_JobGiver_OptimizeApparel_ApparelScoreGain_Postfix
    {

        [HarmonyPostfix]
        public static void PostFix(ref float __result, Pawn pawn, Apparel ap)
        {
            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Forbidden) == true)
            {
                __result = -10000;

            }
        }
    }
}