using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("AddRequiredScars")]
    public static class AlphaMemes_PawnGenerator_AddRequiredScars_Patch
    {
        [HarmonyPrefix]
        public static bool RemoveScarMaking(Pawn pawn)
        {
            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Scarification_Insane) == true)
            {
          
                return false;
            }
            return true;
        }
    }
}
