using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(DesignationCategoryDef))]
    [HarmonyPatch("AllIdeoDesignators")]
    [HarmonyPatch(MethodType.Getter)]

    //Patch to allow me to add designators with precepts instead of just memes or style defs like vanilla
    public static class FuneralFramework_DesignationCategoryDef_AllIdeoDesignators_Postfix
    {
        [HarmonyPostfix]
        public static IEnumerable<Designator> Postfix(IEnumerable<Designator> designators, DesignationCategoryDef __instance,
            Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator> ___ideoBuildingDesignatorsCached)
        {
            //Just a few for loops. I really hope this isnt called a lot because this might be bad
            foreach(var designator in designators)
            {
                yield return designator;
            }
            foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
            {
                foreach (Precept precept in ideo.PreceptsListForReading)
                {
                    if (precept.def.HasModExtension<FuneralPreceptExtension>())
                    {
                        var extension = precept.def.GetModExtension<FuneralPreceptExtension>();
                        if (extension.addDesignators.Count > 0)
                        {
                            foreach(BuildableDef def in extension.addDesignators)
                            {
                                if (__instance == def.designationCategory)
                                {
                                    DesignationCategoryDef.BuildablePreceptBuilding key = new DesignationCategoryDef.BuildablePreceptBuilding(def, null);
                                    Designator cachedDesignator;
                                    if (!___ideoBuildingDesignatorsCached.TryGetValue(key, out cachedDesignator))
                                    {
                                        Designator_Build designatorBuild = new Designator_Build(def);
                                        cachedDesignator = designatorBuild;

                                        ___ideoBuildingDesignatorsCached[key] = cachedDesignator;
                                    }
                                    yield return cachedDesignator;
                                }
                            }
                        }
                    }
                }
            }
        }





    }

}
