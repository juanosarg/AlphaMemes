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


    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnOrganHarvested")]
    public static class AlphaMemes_ThoughtUtility_GiveThoughtsForPawnOrganHarvested_Patch
    {
        [HarmonyPostfix]
        static void NotifySurgeryToNonColonist(Pawn victim, Pawn billDoer)
        {

            if (billDoer.Faction== Faction.OfPlayer && victim.Faction !=Faction.OfPlayer)
            {

                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_HarvestedNonColonistOrgan, billDoer.Named(HistoryEventArgsNames.Doer)), true);

            }





        }
    }








}
