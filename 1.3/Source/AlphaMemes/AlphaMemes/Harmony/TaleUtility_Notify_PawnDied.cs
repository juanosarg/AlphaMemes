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


    [HarmonyPatch(typeof(TaleUtility))]
    [HarmonyPatch("Notify_PawnDied")]
    public static class AlphaMemes_TaleUtility_Notify_PawnDied_Patch
    {
        [HarmonyPostfix]
        static void NotifyEnemyDied(Pawn victim, DamageInfo? dinfo)
        {


           

            if (victim?.RaceProps?.Humanlike == true)
            {
              
               Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_SomeoneDied, new SignalArgs(victim.Named(HistoryEventArgsNames.Victim))), true);
                

            }

            




        }






    }
}









