using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(PregnancyUtility))]
    [HarmonyPatch("ApplyBirthOutcome")]
    public static class AlphaMemes_PregnancyUtility_ApplyBirthOutcome_Patch
    {
        [HarmonyPostfix]
        static void DevelopmentPointsForChildbirth(RitualOutcomePossibility outcome)
        {
            if (outcome.positivityIndex>0)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_ChildBorn);
                Find.HistoryEventsManager.RecordEvent(historyEvent);
            }






        }
    }








}
