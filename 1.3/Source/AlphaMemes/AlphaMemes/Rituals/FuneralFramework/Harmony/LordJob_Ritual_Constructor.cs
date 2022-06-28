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
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(TargetInfo), typeof(Precept_Ritual),typeof(RitualObligation),typeof(List<RitualStage>),typeof(RitualRoleAssignments),typeof(Pawn)})]
    //This nonesense is because createlordjob decides center always for the spot with no concern about whether one can reach center of the thing which if they cant breaks everything \o/
    //specifically they dont get added to the pawns participating list because to be participating the Lord Ritual start spot has to be reachable. Which doesnt error just tells you no pawns particpated.
    public static class FuneralFramework_LordJobRitual_Constructor_Patch
    {
        [HarmonyPostfix]        
        public static void Postfix(LordJob_Ritual __instance, TargetInfo selectedTarget)
        {

            if (selectedTarget.HasThing) 
            {
                Thing thing = selectedTarget.Thing;
                IntVec3 cell = thing.OccupiedRect().CenterCell;
                
                if (thing.def.passability != Traversability.Standable)
                {
                    if (!thing.def.hasInteractionCell)
                    {                        
                        CellFinder.TryFindRandomReachableCellNear(thing.Position, thing.Map, 1, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), null, null, out cell);//Random spot can cause weird positioning of the progress bar
                    }
                    else
                    {
                        cell = thing.InteractionCell;
                    }                   

                    AccessTools.Field(typeof(LordJob_Ritual), "spot").SetValue(__instance, cell);
                }
            }

        }

  
    }

}
