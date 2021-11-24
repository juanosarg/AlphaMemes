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


    [HarmonyPatch(typeof(Verb))]
    [HarmonyPatch("EffectiveRange", MethodType.Getter)]
    public static class AlphaMemes_Verb_EffectiveRange_Patch
    {
        [HarmonyPostfix]
        static void IncreaseRange(Verb __instance, ref float __result)
        {

                if (__instance.CasterPawn?.Ideo?.GetPrecept(InternalDefOf.AM_CombatProwess_Increased) != null)
                {

                    __result = __instance.verbProps.range * 1.2f;
                }
            
        }
    }

    






}
