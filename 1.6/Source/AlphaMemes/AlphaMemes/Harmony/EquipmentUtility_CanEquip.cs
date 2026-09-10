
using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;

namespace AlphaMemes
{
    [HarmonyPatch(typeof(EquipmentUtility), "CanEquip", new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[]
        {0,0,ArgumentType.Out,0})]
    internal class AlphaMemes_EquipmentUtility_CanEquip_Postfix
    {

        [HarmonyPostfix]
        public static void PostFix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Forbidden) == true && thing.def.IsApparel)
            {
                __result = false;
                cantReason = "AM_NakedTruthCantWearArmor".Translate();
            }


        }
    }
}