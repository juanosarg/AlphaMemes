using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(CompGiveThoughtToAllMapPawnsOnDestroy))]
    [HarmonyPatch("PostDestroy")]
    public static class AlphaMemes_CompGiveThoughtToAllMapPawnsOnDestroy_PostDestroy_Patch
    {


        [HarmonyPostfix]
        static void DetectAnimaScream(CompGiveThoughtToAllMapPawnsOnDestroy __instance)
        {
            if (__instance.parent.def == ThingDefOf.Plant_TreeAnima)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_AnimaScream);
                Find.HistoryEventsManager.RecordEvent(historyEvent);

            }





        }
    }








}
