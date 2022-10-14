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

            if (victim?.RaceProps?.Animal == true && Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_AnimalDespised)!=null)
            {

                bool isDespised = false;
                foreach (Ideo ideo in Current.Game.World.factionManager.OfPlayer.ideos.AllIdeos)
                {
                    foreach (Precept precept in ideo.PreceptsListForReading)
                    {
                        Precept_DespisedAnimal precept_DespisedAnimal;
                        if ((precept_DespisedAnimal = precept as Precept_DespisedAnimal) != null && victim.def.defName == precept_DespisedAnimal.ThingDef.defName)
                        {
                            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_DespisedAnimalDied, new SignalArgs(victim.Named(HistoryEventArgsNames.Victim))), true);
                            isDespised = true;
                        }
                    }

                }

                if (!isDespised)
                {
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_AnimalDied, new SignalArgs(victim.Named(HistoryEventArgsNames.Victim))), true);
                }
                
                
                
                


            }






        }






    }
}









