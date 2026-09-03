using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(Apparel))]
    [HarmonyPatch("PawnCanWear")]
    public static class AlphaMemes_Apparel_PawnCanWear_Patch
    {
        [HarmonyPostfix]
        public static void CantWear(ref bool __result, Pawn pawn)
        {
            if (pawn?.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Forbidden) == true)
            {
                __result = false;
            }


        }
    }




}
