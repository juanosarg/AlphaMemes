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
    [HarmonyPatch("ApplyOutcome")]

    //have to do this in harmony instead of my lordjob because it needs to be done on funerals not just specific to mine
    public static class FuneralFramework_LordJobRitual_ApplyOutcome_Patch
    {
        [HarmonyPostfix]        
        public static void Postfix(LordJob_Ritual __instance, List<RitualStagePositions> ___ritualStagePositions)
        {
            if (__instance.Ritual == null)
            {
                return;
            }
            if (__instance.obligation?.targetA.HasThing ?? false) //All funeral obligations target A is corpse
            {
                Corpse corpse = (__instance.obligation.targetA.Thing as Pawn)?.Corpse;
                if (corpse == null) { return; }
                if(__instance.Ritual.activeObligations.Any(x=>x == __instance.obligation)) { return; }//Only do it if the obligation is removed from this ritual. Otherwise it means it failed in some way
                foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos) //Cleanup all ideos not just the ritual on incase there was multiple ideos with this obligation
                {
                    foreach (Precept_Ritual precept in ideo.GetAllPreceptsOfType<Precept_Ritual>())
                    {
                        if (!precept.activeObligations.NullOrEmpty() && 
                            (precept.def.alsoAdds != __instance.Ritual.def))//To not remove alsoadds obligations for things like cryptosleep
                        {
                            foreach (RitualObligation obligation in precept.activeObligations.ToList())
                            {
                                if (obligation.targetA.Thing == corpse)
                                {
                                    precept.activeObligations.Remove(obligation);
                                }
                            }
                        }
                    }
                }
            }



        }      

  
    }

}
