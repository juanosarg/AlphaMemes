using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;

using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(GatheringsUtility))]
    [HarmonyPatch("InGatheringArea")]
    //Patch to solve my spot issues once and for all.
    //There's a chance this causes issues somehow, but tbh idk there's so many checks that'd prevent a ritual from working that as far as i can tell this is completely redundant to begin with.
    //So even if this somehow makes something count The effectt is basically nothing
    public static class GatheringsUtility_InGatheringArea_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, IntVec3 cell, IntVec3 partySpot, Map map)
        {
            if(__result == true) { return; }
            Building building = partySpot.GetEdifice(map);
            if(building == null) { return; }
            LordJob_Ritual ritual = Find.IdeoManager.GetActiveRitualOn(building);
            if (ritual != null)
            {
                if(map.reachability.CanReach(cell,building.InteractionCell,PathEndMode.ClosestTouch,TraverseParms.For(TraverseMode.NoPassClosedDoors))) 
                {
                    __result = true;
                }
            }
        }

    }


}
