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

        public static Thing GetTable(IntVec3 tablePosition, Map map)
        {
            List<Thing> thingList = tablePosition.GetThingList(map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (thingList[i].def.surfaceType == SurfaceType.Eat)
                {
                    return thingList[i];
                }
            }
            return null;
        }


        [HarmonyPostfix]
        static void IngestedNutrientPaste(Thing __instance, Pawn ingester)
        {
            if (nutrientMeals.Contains(__instance.def) && ingester.Ideo?.HasPrecept(InternalDefOf.AM_NutrientPasteEating_Forbidden) == true)
            {
                ingester.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);

            }

            if (__instance.def?.ingestible?.preferability == FoodPreferability.MealSimple && InternalDefOf.AM_AteSimpleMeal != null)
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteSimpleMeal, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);

            }
            if (__instance.def?.ingestible?.preferability == FoodPreferability.RawBad && InternalDefOf.AM_AteRawFood != null)
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteRawFood, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);

            }
            if (__instance.def?.ingestible?.preferability == FoodPreferability.MealLavish && InternalDefOf.AM_AteLavishMeal != null)
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteLavishMeal, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);

            }

            if (ingester.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_RoughLiving_Disliked) == true)
            {
                IntVec3 positionTable = ingester.Position + ingester.Rotation.FacingCell;
                Job curJob = ingester.jobs.curJob;
                Thing table = GetTable(positionTable, ingester.Map);

                if (table?.Stuff == ThingDefOf.WoodLog)
                {
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteInWoodenTable, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);
                }


                if (table is null && ingester.needs.mood != null && __instance.def.IsNutritionGivingIngestible && __instance.def.ingestible.chairSearchRadius > 10f)
                {
                    if (ingester.GetPosture() == PawnPosture.Standing && !ingester.IsWildMan() && __instance.def.ingestible.tableDesired)
                    {
                        Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteWithoutATable, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);
                    }
                }
            }

            if (ingester.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_TableQuality_Desired) == true)
            {
                IntVec3 positionTable = ingester.Position + ingester.Rotation.FacingCell;
                Thing table = GetTable(positionTable, ingester.Map);
                CompQuality compQuality = table.TryGetComp<CompQuality>();
                if (compQuality != null)
                {
                    if (compQuality.Quality < QualityCategory.Normal)
                    {
                        Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteInLowQualityTable, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);

                    }
                }

            }


                



        }
    }
}
