using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using AlphaPrefabs;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("Ingested")]
    public static class AlphaMemes_Thing_Ingested_Patch
    {
        public static HashSet<ThingDef> nutrientMeals = new HashSet<ThingDef>() { ThingDefOf.MealNutrientPaste, InternalDefOf.AA_DisgustingMealNutrientPaste,
        DefDatabase<ThingDef>.GetNamedSilentFail("AM_DisgustingMealNutrientPaste")};

        [HarmonyPostfix]
        static void IngestedNutrientPaste(Thing __instance, Pawn ingester)
        {
            if(nutrientMeals.Contains(__instance.def) && ingester.Ideo?.HasPrecept(InternalDefOf.AM_NutrientPasteEating_Forbidden)==true)
            {
                ingester.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);

            }



        }
    }
}
