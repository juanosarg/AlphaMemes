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

    [HarmonyPatch(typeof(Precept_Ritual))]
    [HarmonyPatch("AddObligation")]
    //patch to try and create a master obligation for all potentional individual obligations
    public static class Precept_Ritual_AddObligation
    {
        [HarmonyPrefix]
        public static void Prefix(Precept_Ritual __instance, RitualObligation obligation)
        {
            if (Find.IdeoManager.classicMode) { return; }
            if (obligation.targetA.HasThing)
            {
                Corpse corpse = obligation.targetA.Thing as Corpse;
                if(corpse == null) { return; }
                if (obligation.sendLetter) //If its not sending a letter we dont need it
                {
                    FuneralFramework_ObligationUtility.obligations.Add(obligation);
                    FuneralFramework_ObligationUtility.rituals.Add(__instance);
                    obligation.sendLetter = false; //Making it not send a letter as we will handle that after.
                    //Copied from vanilla AddOlbigation send letter section
                    TargetInfo firstValidTarget = obligation.FirstValidTarget;
                    Map map = Find.AnyPlayerHomeMap;
                    if (firstValidTarget.IsValid)
                    {
                        map = firstValidTarget.Map;
                    }
                    IEnumerable<TargetInfo> targets = __instance.obligationTargetFilter.GetTargets(obligation, map);
                    if (targets.Any<TargetInfo>())
                    {
                        FuneralFramework_ObligationUtility.targets.Add(targets.First());
                    }
                    else
                    {
                        FuneralFramework_ObligationUtility.targets.Add(firstValidTarget);
                    }                    
                }
            }
        }

    }


}
