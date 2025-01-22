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
    [HarmonyPatch("SpawnSetup")]
    public static class AlphaMemes_Plant_SpawnSetup_Patch
    {
        [HarmonyPostfix]
        static void SetPlantFertility(Map map, Plant __instance)
        {
            if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Hydroagriculture_Revered) != null && __instance.IsCrop)
            {
                WorldComponent_PlantFertility.Instance.CheckIfPlantExtraFertile(__instance,map);
            }






        }
    }








}
