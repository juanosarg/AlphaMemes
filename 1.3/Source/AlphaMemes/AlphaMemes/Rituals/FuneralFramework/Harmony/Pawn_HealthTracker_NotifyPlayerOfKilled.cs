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

    [HarmonyPatch(typeof(Pawn_HealthTracker))]
    [HarmonyPatch("NotifyPlayerOfKilled")]
    //Patch to create essentially a new obligation trigger for animals
    //This is because animals dont get the ideo meaning base obligation trigger cant be used
    //Best spot I could find to hook into
    //Handles filtering out of Hunted, sluaghtered, and killed as part of ritual(sacrifice)
    //As well as none faction and a few other relevant ones
    public static class FuneralFramework_Pawn_HealthTracker_NotifyPlayerOfKilled_AnimalObligationTrigger
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn_HealthTracker __instance, Pawn ___pawn)
        {
            Pawn pawn = ___pawn;
            if (pawn.IsColonist && !pawn.RaceProps.Animal) //Should filter out the rest quickly, no robot funerals
            {
                return;
            }
            //Have to do some work to get what I need as animals dont have the ideo
            foreach(Ideo ideo in Faction.OfPlayer.ideos.AllIdeos) //Doing all ideos so it still triggers for minor ideos. : Fairly certain first ideo is always primary ideo based on how the enumerable is built
            {   
                bool createdObligation = false;
                foreach (Precept_Ritual ritual in ideo.GetAllPreceptsOfType<Precept_Ritual>().Where(x => x.def.HasModExtension<FuneralPreceptExtension>() ? x.def.GetModExtension<FuneralPreceptExtension>().allowAnimals : false))
                {   //Doing for each for as I might allow multiple animal funerals but still only 1 per type eg 1 bonded 1 venerated though probably not because it will clutter up rituals and max 6
                    if(ritual.activeObligations != null)
                    {
                        if (ritual.activeObligations.Any(x => x.targetA.Thing is Corpse ? (Corpse)x.targetA.Thing == pawn.Corpse : false))
                        {//just in case this somehow gets called twice for same pawn
                            createdObligation = true;
                            break;
                        }
                    }                      
                    
                    
                    RitualObiligationTrigger_Animals trigger = ritual.def.GetModExtension<FuneralPreceptExtension>().animalObligationTrigger;
                    Pawn target2 = null;
                    bool canAdd = trigger.CanAddPawn(pawn, ideo, ritual, out target2);
                    if (canAdd && target2 != null)
                    {
                        ritual.AddObligation(new RitualObligation(ritual, pawn.Corpse, target2, true));
                        createdObligation = true; //Target 2 is not used at all currently, but added in case I wanted to make something where target 2 replaces the moralist role for quality eg animal's master does the service
                                                  //But it's tricky because Outcome Comps is set by defs and I dont want to duplicate every ritual into animal due to list clutter and limit 6 rituals
                                                  //Probably just need to make my own role comp
                    }                  
                    else if (canAdd)
                    {
                        ritual.AddObligation(new RitualObligation(ritual, pawn.Corpse,true));
                        createdObligation = true;
                    }

                }
                if (createdObligation)//Once an obligation is made stop searching so we never create 2 obligations
                {
                    break;
                }
            }

        }

    }


}
