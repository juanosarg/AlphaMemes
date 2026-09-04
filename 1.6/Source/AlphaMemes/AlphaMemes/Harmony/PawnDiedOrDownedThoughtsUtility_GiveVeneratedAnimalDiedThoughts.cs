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
        public static void HorseDied(Pawn victim, Map map)
        {

            if (victim.Faction == Faction.OfPlayerSilentFail && StaticCollections.horseAnimals.Contains(victim.kindDef))
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_ColonyHorseDied);
                Find.HistoryEventsManager.RecordEvent(historyEvent);


            }





        }
    }








}
