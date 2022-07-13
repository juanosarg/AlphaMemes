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

    [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("CanAddPrecept")]
    //Patch to prevent user from adding 2 funerals
    public static class FuneralFramework_IdeoUIUtility_CanAddPrecept_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref AcceptanceReport __result, PreceptDef def, RitualPatternDef pat, Ideo ideo)
        {
            if(__result.Accepted == false)
            {
                return;
            }
    
            if (def.HasModExtension<FuneralPreceptExtension>())
            {
                FuneralPreceptExtension extension = def.GetModExtension<FuneralPreceptExtension>();
                List<PreceptDef> ritualconflict = new List<PreceptDef>();
                AcceptanceReport report = extension.specialConflicts?.PreceptConflicts(ideo, out ritualconflict, extension)?? true;
                if(report.Accepted && ritualconflict.Count > 0)
                {
                    __result = "Funeral_WillReplace".Translate(string.Join(", ", ritualconflict.Select(x => x.LabelCap)).Named("REPLACING"));
                    return;
                }
                if (report.Accepted)//Checks precepts first then research to not clog up small ritual UI by concatting
                {
                    report = extension.specialConflicts?.ResearchConflicts(ideo) ?? true; ;
                }
                if (!report.Accepted)
                {
                    __result = report.Reason;
                }
                else
                {
                    __result = true;
                }
                
            }
        }

    }
    //The fact the rituals window is not sorted is annoys me so Im fixing it.
    //DoPreceptsInt would be the better method but it's a shitshow that'd need a complicated transpiler 
    //This is worse cause I keep sorting over and over, but this only runs in the UI so whatever
    [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("AddPreceptOption")]
    public static class FuneralFramework_IdeoUIUtility_AddPreceptOption_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(List<FloatMenuOption> options,PreceptDef def)
        {
            if(def.issue.defName == "Ritual")
            {
                options.SortBy(x => x.Disabled);
            }
            

        }
    }



}
