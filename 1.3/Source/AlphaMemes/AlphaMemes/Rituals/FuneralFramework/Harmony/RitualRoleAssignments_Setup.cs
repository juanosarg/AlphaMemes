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
    [HarmonyPatch("Setup")]
    //This patch is to get the corpses in the list of selectable pawns
    public static class FuneralFramework_RitualRoleAssignments_Setup_Patch
    {
        [HarmonyPrefix]
        public static void SelectViableCorpse(RitualRoleAssignments __instance, ref List<Pawn> allPawns, ref Dictionary<string, Pawn> forcedRoles)
        {
            
            if (__instance.Ritual == null)
            {
                return; //Not all rituals are rituals neat.
            }
            Precept_Ritual ritual = __instance.Ritual;
            
            if (ritual.def.HasModExtension<FuneralPreceptExtension>()&& ritual.def != PreceptDefOf.Funeral)
            {
                //Better idea then to recheck all criteria
                foreach (RitualObligation obligation in ritual.activeObligations)
                {
                    Corpse corpse = (Corpse)obligation.targetA.Thing;
                    if (corpse != null && !corpse.Destroyed)
                    {
                        if (!allPawns.Contains(corpse.InnerPawn) && corpse.ParentHolder as Building_Casket == null)
                        {
                            allPawns.Add(corpse.InnerPawn);
                        }                            
                    }
                }


            }

        }

    }

}
