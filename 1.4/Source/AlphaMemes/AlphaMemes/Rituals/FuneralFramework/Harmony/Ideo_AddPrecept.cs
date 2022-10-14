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
    //This patch is to handle special conflicts

    public static class FuneralFramework_Ideo_AddPrecept
    {

        [HarmonyPrefix]
        public static bool Prefix(Ideo __instance, Precept precept, ref RitualPatternDef fillWith, ref bool init)
        {

            if (precept.def.issue.defName != "Ritual")
            {
                //check if the precept being added creates a conflict if so, remove the ritual ideally id warn person first but a bit difficult to do so
                foreach (Precept p in __instance.PreceptsListForReading.Where(x => x.def.HasModExtension<FuneralPreceptExtension>()))
                {
                    if (p.def.GetModExtension<FuneralPreceptExtension>().specialConflicts?.AddingConflict(__instance, precept) ?? false)
                    {
                        __instance.RemovePrecept(p);
                    }
                }
                return true;
            }
            //I think this is a fix for an obscure bug that I've only seen twice, and never when Im trying to get it. Happens in world gen, did over 50 worldgen iterations without being able to trigger it when i was in a position to trace it
            //But looking at the log I saved from it, I'm 99% sure whats happening is something is adding a no corpse but for whatever reason that no corpse failed to pass validation here.
            //I can't really see how with how randomise patch is setup but this cant hurt as Precept:GenerateNewName is First() on below criteria so it will error if a precept like below is added
            if(precept.def.takeNameFrom != null)
            {
                if(!__instance.PreceptsListForReading.Any(x=> x.def == precept.def.takeNameFrom))
                {
                    return false;
                }
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

                AcceptanceReport report = extension.specialConflicts?.PreceptConflicts(__instance, out ritualConflict, extension) ?? true;
                if (report.Accepted)//Checks precepts first then research to not clog up small ritual UI by concatting
                {
                    report = extension.specialConflicts?.ResearchConflicts(__instance) ?? true;
                }
                if (ritualConflict.Count > 0 && report.Accepted) //Remove the conflicts then go ahead
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


