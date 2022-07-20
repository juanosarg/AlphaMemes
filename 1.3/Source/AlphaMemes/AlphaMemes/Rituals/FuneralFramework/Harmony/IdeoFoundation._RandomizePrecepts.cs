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

    [HarmonyPatch(typeof(IdeoFoundation))]
    [HarmonyPatch("RandomizePrecepts")]
    //A patch to randomize which funeral is used based on weighting
    public static class FuneralFramework_IdeoFoundation_RandomizePrecepts
    {

        [HarmonyPostfix]        
        public static void Postfix(IdeoFoundation __instance, IdeoGenerationParms parms)
        {
            if (Find.IdeoManager.classicMode) { return; }
            if (__instance.ideo.PreceptsListForReading.Any(x => x.def.HasModExtension<FuneralPreceptExtension>()))
            {
                
                List<Precept>rituals = __instance.ideo.PreceptsListForReading.Where(x => x.def.HasModExtension<FuneralPreceptExtension>()).ToList();//There can never be more then one based on other harmony so this should be fine
                foreach(Precept ritual in rituals)//Now removing all funerals because change to CanAdd to allow multiple now means it adds all of them on random
                {
                    if (!ritual.def.GetModExtension<FuneralPreceptExtension>().replaceConflictRituals) //Still leave replaceConflict
                    {
                        __instance.ideo.RemovePrecept(ritual);                    
                    }

                }
                List<PreceptDef> validDefs = (from x in FuneralFrameWork_StaticStartup.funeralDefs
                                              where x.GetModExtension<FuneralPreceptExtension>()._weighting > 0 &&
                                              x.GetModExtension<FuneralPreceptExtension>().CanAddPrecept(__instance.ideo, x, parms.forFaction)
                                              select x).ToList();
                PreceptDef def = validDefs.RandomElementByWeight(x => x.GetModExtension<FuneralPreceptExtension>().Weighting(__instance.ideo, x));
                if (def != null)
                {
                    __instance.ideo.AddPrecept(PreceptMaker.MakePrecept(def), true, null, def.ritualPatternBase);
                }

            }


        }

    }
}


