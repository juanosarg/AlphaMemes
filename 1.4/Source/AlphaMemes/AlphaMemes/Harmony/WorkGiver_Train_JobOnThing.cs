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


    [HarmonyPatch(typeof(WorkGiver_Train))]
    [HarmonyPatch("JobOnThing")]
    public static class AlphaMemes_WorkGiver_Train_JobOnThing_Patch
    {
        [HarmonyPostfix]
        static void DisallowTamingEvent(Pawn pawn, Thing t, ref Job __result)
        {

            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Bonding_Abhorrent) == true)
            {
                new HistoryEvent(InternalDefOf.AM_TrainAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
                __result = null;
            }





        }
    }








}
