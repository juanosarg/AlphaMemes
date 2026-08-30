using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(IdeoImpactUtility))]
    [HarmonyPatch("OverallImpactLabel")]
    public static class AlphaMemes_IdeoImpactUtility_OverallImpactLabel_Patch
    {

        [HarmonyPrefix]
        public static bool DrawInsaneImpactLabel(int impact, ref string __result)
        {
            if (impact > 20)
            {
                __result = "AM_IdeoImpactLabel_11".Translate();
                return false;
            }
            if (impact > 9)
            {
                __result = "AM_IdeoImpactLabel_10".Translate();
                return false;
            }
            return true;
        }
    }
}
