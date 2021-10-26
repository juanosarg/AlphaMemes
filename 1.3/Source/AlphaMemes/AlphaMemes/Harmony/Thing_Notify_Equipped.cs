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


    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("Notify_Equipped")]
    public static class AlphaMemes_Thing_Notify_Equipped_Patch
    {
        [HarmonyPostfix]
        static void IncreaseRange(Thing __instance, Pawn pawn)
        {

            if (__instance.def.IsRangedWeapon)
            {
                if (pawn.Ideo?.GetPrecept(InternalDefOf.AM_CombatProwess_Increased) != null)
                {
                    GameComponent_WeaponRanges comp = Current.Game.GetComponent<GameComponent_WeaponRanges>();

                    if (!comp.weapon_ranges.ContainsKey(__instance))
                    {
                        comp.AddWeaponAndRange(__instance, __instance.def.Verbs.FirstOrDefault().range);
                    }

                    __instance.def.Verbs.FirstOrDefault().range = comp.weapon_ranges[__instance] * 1.2f;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("Notify_Unequipped")]
    public static class AlphaMemes_Thing_Notify_Unequipped_Patch
    {
        [HarmonyPostfix]
        static void DecreaseRange(Thing __instance, Pawn pawn)
        {

            if (__instance.def.IsRangedWeapon)
            {
                if (pawn.Ideo?.GetPrecept(InternalDefOf.AM_CombatProwess_Increased) != null)
                {
                    GameComponent_WeaponRanges comp = Current.Game.GetComponent<GameComponent_WeaponRanges>();

                    if (comp.weapon_ranges.ContainsKey(__instance))
                    {
                        __instance.def.Verbs.FirstOrDefault().range = comp.weapon_ranges[__instance];
                        comp.RemoveWeaponAndRange(__instance);
                    }

                    
                }
            }
        }
    }






}
