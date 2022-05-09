using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(Building_NutrientPasteDispenser))]
    [HarmonyPatch("TryDispenseFood")]
    public static class AlphaMemes_Building_NutrientPasteDispenser_TryDispenseFood_Patch
    {
        [HarmonyPostfix]
        static void ApplyNutrientPasteStyle(Thing __result, Building_NutrientPasteDispenser __instance)
        {
           
            if (__instance.def == InternalDefOf.AM_GrogDispenser)
            {
                __result.StyleDef = InternalDefOf.AM_MealNutrientPaste;

            }







        }
    }








}
