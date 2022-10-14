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
    //Patch to make skyburial buildable
    [HarmonyPatch(typeof(Ideo))]
    [HarmonyPatch("MembersCanBuild")]
    public static class AlphaMemes_Ideo_MembersCanBuild
    {
        [HarmonyPostfix]
          
        public static void Postfix(Ideo __instance, ref bool __result, Thing thing)
        {
            
            if (Find.IdeoManager.classicMode) { return; }            
            if (__result) { return; }
            foreach (var ritual in __instance.GetAllPreceptsOfType<Precept_Ritual>())
            {
                if (ritual.def.HasModExtension<FuneralPreceptExtension>())
                {
                    var extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
                    if (extension.addDesignators.Count > 0)
                    {
                        BuildableDef buildableDef = thing.def.entityDefToBuild ?? thing.def;
                        foreach (BuildableDef def in extension.addDesignators)
                        {
                            
                            if (buildableDef == def)
                            {
                                __result = true;
                                break;
                            }
                                
                        }
                    }
                }

            }

        }      

        
    }

}


