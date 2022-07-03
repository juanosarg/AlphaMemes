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
            if (__instance.ideo.PreceptsListForReading.Any(x => x.def.HasModExtension<FuneralPreceptExtension>()))
            {
                
                Precept ritual = __instance.ideo.PreceptsListForReading.First(x => x.def.HasModExtension<FuneralPreceptExtension>());//There can never be more then one based on other harmony so this should be fine
                if (!ritual.def.GetModExtension<FuneralPreceptExtension>().replaceConflictRituals) //If the ritual is one that replaces we leave it
                {
                    __instance.ideo.RemovePrecept(ritual);//remove this one so it doesn't cause ritual conflict flags
                    List<PreceptDef> validDefs = (from x in FuneralFrameWork_StaticStartup.funeralDefs
                                                  where x.GetModExtension<FuneralPreceptExtension>()._weighting > 0 &&
                                                  x.GetModExtension<FuneralPreceptExtension>().CanAddPrecept(__instance.ideo, x, parms.forFaction)
                                                  select x).ToList();
                    PreceptDef def = validDefs.RandomElementByWeight(x => x.GetModExtension<FuneralPreceptExtension>().Weighting(__instance.ideo, x));
                    if(def != null)
                    {                        
                        __instance.ideo.AddPrecept(PreceptMaker.MakePrecept(def),true,null,def.ritualPatternBase);
                    }
                    else //I dont think this can ever get hit, but just in case it fails readd the one we removed
                    {
                        __instance.ideo.AddPrecept(ritual, true, null, def.ritualPatternBase);
                    }
                }
            }


        }

    }
}


