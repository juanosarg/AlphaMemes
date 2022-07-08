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

    [HarmonyPatch(typeof(RitualPatternDef))]
    [HarmonyPatch("Fill")]
    //Patch to handle the inits of my extensions
    public static class RitualPatternDef_Fill_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Precept_Ritual ritual)
        {
            if (!ritual.def.HasModExtension<FuneralPreceptExtension>())//Still have to worry about vanilla funeral
            {
                return;
            }
            //Filter init
            if(ritual.obligationTargetFilter is RitualObligationTargetWorker_FuneralThingExtended)
            {
                RitualObligationTargetWorker_FuneralThingExtended filter = ritual.obligationTargetFilter as RitualObligationTargetWorker_FuneralThingExtended;
                filter.initFilters(ritual);
            }

        }

    }


}
