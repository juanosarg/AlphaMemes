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
    [HarmonyPatch("ShouldBeCalledOff")]

    //stops the ritual from failing because corpses isnt part of lord.ownedpawns
    public static class FuneralFramework_LordJobRitual_ShouldBeCalledOff_Patch
    {
        [HarmonyPostfix]        
        public static void Postfix(LordJob_Ritual __instance, ref bool __result)
        {
            if (__instance.Ritual == null)
            {
                return;
            }
            Precept_Ritual ritual = __instance.Ritual;

            if (ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                if (__result == true)
                {
                    __result = false;
                    if (__instance.lord.ownedPawns.Count == 0)
                    {
                        __result = true;
                        
                    }
                    if (__instance.selectedTarget.ThingDestroyed)
                    {
                        __result = true;                        
                    }


                    foreach (Pawn participant in __instance.assignments.Participants)
                    {
                        if (participant.Map != null)
                        {
                            RitualRole ritualRole = __instance.assignments.RoleForPawn(participant, true);
                            if (((ritualRole != null && ritualRole.required) || __instance.assignments.Required(participant) || __instance.assignments.ExtraRequiredPawnsForReading.Contains(participant)) && SelfDefenseUtility.ShouldStartFleeing(participant))
                            {
                                __result = true;
                                break;
                            }
                        }
                    }
                    
                }
            }      
            

        }

  
    }

}
