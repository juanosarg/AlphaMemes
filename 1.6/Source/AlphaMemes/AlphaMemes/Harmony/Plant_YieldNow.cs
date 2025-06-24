using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;

namespace AlphaMemes
{
    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("YieldNow")]
    public static class AlphaMemes_Plant_YieldNow_Patch
    {
        [HarmonyPostfix]
        static void IncreaseTeaYield(Plant __instance, ref int __result)
        {
            if (Find.IdeoManager.classicMode) return;

            if ((__instance?.def == InternalDefOf.VBE_Plant_Tea && Current.Game.World.factionManager.OfPlayer.ideos?.GetPrecept(InternalDefOf.AM_TeaYield_Increased) != null)
               )
            {
                __result = GenMath.RoundRandom(__result * 1.1f);
            }

        }
    }
}
