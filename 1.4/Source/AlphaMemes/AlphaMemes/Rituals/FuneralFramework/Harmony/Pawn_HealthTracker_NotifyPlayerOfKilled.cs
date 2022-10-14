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
            foreach(Ideo ideo in Faction.OfPlayer.ideos.AllIdeos) //Doing all ideos so it still triggers for minor ideos.
            {       
                bool createdObligation = false;
                bool sendLetter = Faction.OfPlayer.ideos.IsPrimary(ideo);//Only send letter for primary ideo so we dont spam letters if multipe ideos all have a claim to the animal's funeral
                foreach (Precept_Ritual ritual in ideo.GetAllPreceptsOfType<Precept_Ritual>().Where(x => x.def.HasModExtension<FuneralPreceptExtension>() ? x.def.GetModExtension<FuneralPreceptExtension>().allowAnimals : false))
                {   
                    RitualObiligationTrigger_Animals trigger = ritual.def.GetModExtension<FuneralPreceptExtension>().animalObligationTrigger;
                    Pawn target2 = null;
                    bool canAdd = trigger.CanAddPawn(pawn, ideo, ritual, out target2);
                    if (canAdd && target2 != null)
                    {
                        ritual.AddObligation(new RitualObligation(ritual, pawn, target2, sendLetter));
                        createdObligation = true; //Target 2 is not used at all currently, but added in case I wanted to make something where target 2 replaces the moralist role for quality eg animal's master does the service
                    }                  
                    else if (canAdd)
                    {
                        ritual.AddObligation(new RitualObligation(ritual, pawn, sendLetter));//Obligation Utility will get set when we send this
                        createdObligation = true;
                    }

                }
                if (createdObligation && sendLetter)
                {
                    FuneralFramework_ObligationUtility.SendLetter(ideo, ___pawn);                  
                }
                FuneralFramework_ObligationUtility.Cleanup(); //Just in case
            }

        }

    }


}
