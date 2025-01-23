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


    [HarmonyPatch(typeof(WorkGiver_PruneGauranlenTree))]
    [HarmonyPatch("HasJobOnThing")]
    public static class AlphaMemes_WorkGiver_PruneGauranlenTree_HasJobOnThing_Patch
    {
        [HarmonyPostfix]
        static void DisallowPruningEvent(Pawn pawn, Thing t, ref bool __result)
        {

            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_GauranlenConnection_Forbidden) == true)
            {
                new HistoryEvent(InternalDefOf.AM_PruneGauranlenTree, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
                __result = false;
            }





        }
    }








}
