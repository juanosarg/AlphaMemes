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


    [HarmonyPatch(typeof(QuestManager))]
    [HarmonyPatch("Notify_PlantHarvested")]
    public static class AlphaMemes_QuestManager_Notify_PlantHarvested_Patch
    {
        [HarmonyPostfix]
        static void NotifyGauranlenTreeDead(Thing harvested)
        {

            if (harvested.def == ThingDefOf.Plant_TreeGauranlen)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_CutGauranlenTree);
                Find.HistoryEventsManager.RecordEvent(historyEvent);
            }





        }
    }








}
