using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(LordJob_Ritual))]
    [HarmonyPatch("Notify_AddedToLord")]

    //this is to remove the corpse stage positions from the dictionary as they aren't needed, and with them there it causes a null key in the dictionary during save.
    public static class FuneralFramework_LordJobRitual_NotifyAddedToLord_Patch
    {
        [HarmonyPostfix]        
        public static void Postfix(LordJob_Ritual __instance, List<RitualStagePositions> ___ritualStagePositions)
        {
            if (__instance.Ritual == null)
            {
                return;
            }
            Precept_Ritual ritual = __instance.Ritual;

            if (ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                Pawn corpse = __instance.assignments.Participants.FirstOrDefault(x => x.Dead);
                if(corpse != null)
                {
                    foreach (RitualStagePositions positions in ___ritualStagePositions)
                    {
                        if (positions.positions.ContainsKey(corpse))
                        {
                            positions.positions.Remove(corpse);
                        }
                    }
                }



            }      
            

        }

  
    }

}
