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


    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("GiveVeneratedAnimalDiedThoughts")]
    public static class AlphaMemes_PawnDiedOrDownedThoughtsUtility_GiveVeneratedAnimalDiedThoughts_Patch
    {
        [HarmonyPostfix]
        static void HorseDied(Pawn victim, Map map)
        {

            if (victim.Faction == Faction.OfPlayerSilentFail && victim.kindDef== InternalDefOf.Horse)
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_ColonyHorseDied);
                Find.HistoryEventsManager.RecordEvent(historyEvent);


            }





        }
    }








}
