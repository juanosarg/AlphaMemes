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
    public static class AlphaMemesIdeo_Notify_Patches
    {
        [HarmonyPatch(typeof(Ideo))]
        [HarmonyPatch("Notify_MemberDied")]
        public static class FuneralFramework_Ideo_Notify_MemberDied
        {
            [HarmonyPostfix]
            //Fires our letter
            public static void Postfix(Ideo __instance, Pawn member)
            {
                if (Find.IdeoManager.classicMode) { return; }
                if (!FuneralFramework_ObligationUtility.obligations.NullOrEmpty())
                {
                    if (FuneralFramework_ObligationUtility.obligations.Any(x => x.targetA.Thing == member))
                    {
                        FuneralFramework_ObligationUtility.SendLetter(__instance, member);
                    }
                    FuneralFramework_ObligationUtility.Cleanup();
                }
            }

        }
        [HarmonyPatch(typeof(Ideo))]
        [HarmonyPatch("Notify_MemberCorpseDestroyed")]
        public static class FuneralFramework_Ideo_MemberCorpseDestroyed
        {
            [HarmonyPrefix]
            //This is to prevent no corpse funeral being created by our funerals that destory corpes.
            //Ideo Notify_memberlost does nothing but trigger obligation triggers by itself So should be safe
            public static bool Prefix(Ideo __instance, Pawn member)
            {
                if (Find.IdeoManager.classicMode) { return true;}

                if (!Find.Maps.NullOrEmpty())
                {
                    foreach (var map in Find.Maps)
                    {
                        var lordJob = map.lordManager.lords.Select(x => x.LordJob).OfType<LordJob_Ritual_FuneralFramework>().FirstOrDefault();
                        if(lordJob?.corpse == member)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            [HarmonyPostfix]
            //Fires our letter
            public static void Postfix(Ideo __instance, Pawn member)
            {
                if (Find.IdeoManager.classicMode) { return; }
                if (!FuneralFramework_ObligationUtility.obligations.NullOrEmpty())
                {
                    if (FuneralFramework_ObligationUtility.obligations.Any(x => x.targetA.Thing == member))
                    {
                        FuneralFramework_ObligationUtility.SendLetter(__instance, member);
                    }
                    FuneralFramework_ObligationUtility.Cleanup();
                }
            }
        }
    }

}


