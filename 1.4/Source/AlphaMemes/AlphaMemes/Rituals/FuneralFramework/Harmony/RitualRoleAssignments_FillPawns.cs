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

    [HarmonyPatch(typeof(RitualRoleAssignments))]
    [HarmonyPatch("FillPawns")]
    //Patch to allow me to change assigments based on my comps because game doesnt
    public static class FuneralFramework_RitualRoleAssignments_FillPawns_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(RitualRoleAssignments __instance, TargetInfo ritualTarget)
        {

            if(__instance.Ritual == null)
            {
                return; //Not all rituals are rituals neat.
            }
            Precept_Ritual ritual = __instance.Ritual;
            
            if (ritual.def.HasModExtension<FuneralPreceptExtension>()&& ritual.def != PreceptDefOf.Funeral)
            {
                //Custom assignment filter logic, vanilla code only checks a specific comp, so I gotta do my own

                foreach(RitualOutcomeComp comp in ritual.outcomeEffect.def.comps)
                {
                    if(comp is RitualOutcomeComp_FuneralFramework)
                    {
                        RitualOutcomeComp_FuneralFramework comp2 = comp as RitualOutcomeComp_FuneralFramework;
                        string roleid;
                        RitualRoleAssignments.FailReason failReason;
                        Pawn pawn = comp2.BestPawnForRole(__instance.AllPawns, __instance, out roleid);
                        if(pawn != null)
                        {
                            if (__instance.AnyPawnAssigned(roleid))
                            {
                                __instance.TryUnassignAnyRole(__instance.AssignedPawns(roleid).First());
                            }
                            if (!__instance.TryAssign(pawn, __instance.GetRole(roleid), ritualTarget, out failReason))
                            {

                            }

                        }
                       
                    }
                }


            }

        }

    }

}
