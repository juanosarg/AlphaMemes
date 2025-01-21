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


    [HarmonyPatch(typeof(PregnancyUtility))]
    [HarmonyPatch("ChanceMomDiesDuringBirth")]
    public static class AlphaMemes_PregnancyUtility_ChanceMomDiesDuringBirth_Patch
    {
        [HarmonyPostfix]
        static void RemoveMaternalMortality(ref float __result)
        {
            if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_ArtProductionSpeed_Increased) != null)
            {
                __result= 0f;
            }
           





        }
    }








}
