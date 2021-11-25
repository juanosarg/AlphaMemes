using HarmonyLib;
using RimWorld;
using RimWorld.SketchGen;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(SketchGenUtility))]
    [HarmonyPatch("PlayerCanBuildNow")]
    public static class AlphaMemes_SketchGenUtility_PlayerCanBuildNow_Patch
    {
        [HarmonyPostfix]
        static void DisableIdeoFloors(ref bool __result, BuildableDef buildable)
        {

            if (StaticCollectionsClass.designatorsToBeRemoved.Contains(buildable.designatorDropdown))
            {


                __result = false;


            }





        }
    }








}
