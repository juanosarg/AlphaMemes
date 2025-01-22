using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("GrowthRate", MethodType.Getter)]
    public static class AlphaMemes_Plant_GrowthRate_Patch
    {
        [HarmonyPostfix]
        static void GetPlantFertility(ref float __result, Plant __instance)
        {
            if (WorldComponent_PlantFertility.Instance.plants_and_fertility.ContainsKey(__instance) && WorldComponent_PlantFertility.Instance.plants_and_fertility[__instance])
            {
                __result *= 1.2f;
            }






        }
    }








}
