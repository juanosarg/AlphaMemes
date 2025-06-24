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


    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("TrySpawnStump")]
    public static class AlphaMemes_Plant_TrySpawnStump_Patch
    {
        [HarmonyPostfix]
        static void NotifyGauranlenTreeDead(Plant __instance)
        {

            if (__instance.def == ThingDefOf.Plant_TreeGauranlen)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_CutGauranlenTree);
                Find.HistoryEventsManager.RecordEvent(historyEvent);
            }
            if (__instance.def == ThingDefOf.Plant_TreeHarbinger)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_CutHarbingerTree);
                Find.HistoryEventsManager.RecordEvent(historyEvent);
            }




        }
    }








}
