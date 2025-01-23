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

            if (ingester.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_RoughLiving_Disliked) == true)
            {
                IntVec3 positionTable = ingester.Position + ingester.Rotation.FacingCell;

                bool continueChecking = true;
                List<Thing> thingList = positionTable.GetThingList(ingester.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i].def.surfaceType == SurfaceType.Eat)
                    {
                        if (thingList[i].Stuff == ThingDefOf.WoodLog)
                        {
                            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteInWoodenTable, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);
                            continueChecking = false;
                            break;
                        }
                    }
                }
                if (continueChecking)
                {
                    Pawn actor = ingester;
                    Job curJob = actor.jobs.curJob;
                   
                    if (ingester.needs.mood != null && __instance.def.IsNutritionGivingIngestible && __instance.def.ingestible.chairSearchRadius > 10f)
                    {
                        if (!(ingester.Position + ingester.Rotation.FacingCell).HasEatSurface(actor.Map) && ingester.GetPosture() == PawnPosture.Standing && !ingester.IsWildMan() && __instance.def.ingestible.tableDesired)
                        {
                            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AteWithoutATable, new SignalArgs(ingester.Named(HistoryEventArgsNames.Doer))), true);

                        }
                    }
                }


            }



        }
    }
}
