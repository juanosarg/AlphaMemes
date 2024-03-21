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


    [HarmonyPatch(typeof(ReleaseAnimalToWildUtility))]
    [HarmonyPatch("DoReleaseAnimal")]
    public static class AlphaMemes_ReleaseAnimalToWildUtility_DoReleaseAnimal_Patch
    {
        [HarmonyPostfix]
        static void AnnounceAnimalReleased(Pawn animal)
        {

            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AnimalReleased, new SignalArgs(animal.Named(HistoryEventArgsNames.Subject))), true);






        }
    }








}
