using HarmonyLib;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;

namespace AlphaMemes
{
    /*[HarmonyPatch(typeof(IdeoUtility))]
    [HarmonyPatch("IsMemeAllowedForInitialFluidIdeo")]*/
    public static class AlphaMemes_IdeoUtility_IsMemeAllowedForInitialFluidIdeo_Patch
    {

        /*[HarmonyPostfix]*/
        public static void AllowAllMemes(ref bool __result)
        {
            if (AlphaMemes_Settings.allowHighImpactMemesForFluidIdeos)
            {
                __result = true;
            }
        }
    }
}