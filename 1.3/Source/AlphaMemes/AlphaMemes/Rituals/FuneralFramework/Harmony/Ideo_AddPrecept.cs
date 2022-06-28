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

    [HarmonyPatch(typeof(Ideo))]
    [HarmonyPatch("AddPrecept")]
    //This patch is to prevent the game from adding 2 funerals
    //Im sure there's a better spot/way for this but i cant find it T_T since IdeoFoundation Canadd doesnt feel like being used for rituals
    //Does some other stuff now too handling replacing and removing ritual when adding conflcit
    public static class FuneralFramework_Ideo_AddPrecept
    {

        [HarmonyPrefix]        
        public static bool Prefix(Ideo __instance, Precept precept, ref RitualPatternDef fillWith, ref bool init)
        {

            if (precept.def.issue.defName != "Ritual")
            {
                //check if the precept being added creates a conflict if so, remove the ritual ideally id warn person first but a bit difficult to do so
                foreach(Precept p in __instance.PreceptsListForReading.Where(x=>x.def.HasModExtension<FuneralPreceptExtension>()))
                {
                    if (p.def.GetModExtension<FuneralPreceptExtension>().specialConflicts?.AddingConflict(__instance, precept) ?? false)
                    {
                        __instance.RemovePrecept(p);
                    }
                }
                return true;
            }
            
            //Fix for vanilla bug, adding arguments for rituals that dont get passed during reform when they are need
            if (fillWith == null)
            {
                Precept_Ritual ritual;
                if ((ritual = (precept as Precept_Ritual)) != null)
                {
                    init = true;
                    fillWith = ritual.def.ritualPatternBase;
                }
            }
            //Determination Logic for conflicts
            if (precept.def.HasModExtension<FuneralPreceptExtension>())
            {
                FuneralPreceptExtension extension = precept.def.GetModExtension<FuneralPreceptExtension>();
                List<PreceptDef> ritualConflict = new List<PreceptDef>();
                if (precept.def == InternalDefOf.AM_FuneralNoCorpse)//little hack to make the no corpse name == main ritual name without creating a unique def for each one
                {
                    extension.SetNoCorpseFuneralDefName(__instance, precept.def);
                }
                AcceptanceReport report = extension.specialConflicts?.PreceptConflicts(__instance, out ritualConflict, extension) ?? true;
                if (report.Accepted)//Checks precepts first then research to not clog up small ritual UI by concatting
                {
                    report = extension.specialConflicts?.ResearchConflicts(__instance)?? true;
                }
                if(ritualConflict.Count > 0 && report.Accepted) //Remove the conflicts then go ahead
                {
                    foreach (PreceptDef p in ritualConflict)
                    {
                        __instance.RemovePrecept(__instance.GetPrecept(p));//Remove Conflicts
                    }
                    return true;
                }
                
                if (!report.Accepted)
                {
                    return false; //Dont add it
                }

            }
            return true;

        }

    }
}


