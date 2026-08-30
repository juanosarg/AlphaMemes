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
    [HarmonyPatch("MemeImpactLabel")]
    public static class AlphaMemes_IdeoImpactUtility_MemeImpactLabel_Patch
    {
      

        [HarmonyPrefix]
        public static bool DrawImpactFourLabel(int impact, ref string __result)
        {
            if (impact == 4)
            {
                __result = "AM_IdeoMemeImpactLabel_4".Translate();
                return false;
            }
            return true;
        }
    }
}
