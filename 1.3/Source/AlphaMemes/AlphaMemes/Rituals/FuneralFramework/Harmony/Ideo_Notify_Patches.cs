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
                    if (FuneralFramework_ObligationUtility.obligations.Any(x => x.targetA.Thing == member.Corpse))
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

                LordJob_Ritual_FuneralFramework lordJob = null;
                Map map = Find.CurrentMap;
                if(map == null) { return true; }//Turns out this can get called when map doesnt exist yet. At least character editor did it while loading a saved pawn...
                var lord = map.lordManager.lords.FirstOrDefault(x => x.LordJob is LordJob_Ritual_FuneralFramework);
                if(lord != null)
                {
                    lordJob = lord.LordJob as LordJob_Ritual_FuneralFramework;
                    if(lordJob.corpse == member)
                    {
                        return false;
                    }
                }
                List<Precept_Ritual> rituals = __instance.GetAllPreceptsOfType<Precept_Ritual>().ToList();
                if (rituals.Where(x => x.def.HasModExtension<FuneralPreceptExtension>()).Any(x => x.def.GetModExtension<FuneralPreceptExtension>().addNoCorpseFuneral))
                {
                    var noCorpse = rituals.FirstOrDefault(x => x.def.defName == "AM_FuneralNoCorpse");
                    if (noCorpse != null)
                    {
                        noCorpse.obligationTriggers.First().Notify_MemberCorpseDestroyed(member);
                        return false;
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
                    if (FuneralFramework_ObligationUtility.obligations.Any(x => x.targetA.Thing == member.Corpse))
                    {
                        FuneralFramework_ObligationUtility.SendLetter(__instance, member);
                    }
                    FuneralFramework_ObligationUtility.Cleanup();
                }
            }
        }
    }

}


