using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;
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
            if (!__instance.verbProps.IsMeleeAttack) {
                if (__instance.CasterPawn?.Ideo?.GetPrecept(InternalDefOf.AM_CombatProwess_Increased) != null)
                {

                    __result = __instance.verbProps.range * 1.2f;
                }
            }
                
            
        }
    }

    [HarmonyPatch(typeof(VerbProperties))]
    [HarmonyPatch("DrawRadiusRing")]
    public static class AlphaMemes_VerbProperties_DrawRadiusRing_Patch
    {
        [HarmonyPostfix]
        static void ShowBiggerRadius(IntVec3 center, VerbProperties __instance)
        {
            if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_CombatProwess_Increased) != null) {

                if (Find.CurrentMap == null || __instance.IsMeleeAttack || !__instance.targetable)
                {
                    return;
                }
                float num = __instance.EffectiveMinRange(allowAdjacentShot: true) * 1.2f;
                if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
                {
                    GenDraw.DrawRadiusRing(center, num);
                }
                if (__instance.range * 1.2 < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && __instance.range * 1.2 < GenRadial.MaxRadialPatternRadius)
                {
                    Func<IntVec3, bool> predicate = null;
                    if (__instance.drawHighlightWithLineOfSight)
                    {
                        predicate = ((IntVec3 c) => GenSight.LineOfSight(center, c, Find.CurrentMap));
                    }
                    GenDraw.DrawRadiusRing(center, __instance.range * 1.2f, Color.blue, predicate);
                }

            }
            


        }
    }








}
