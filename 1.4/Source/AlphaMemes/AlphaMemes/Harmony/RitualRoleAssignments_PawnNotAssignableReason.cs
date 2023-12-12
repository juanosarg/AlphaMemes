using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using VFECore;

namespace AlphaMemes
{




    [HarmonyPatch(typeof(RitualRoleAssignments))]
    [HarmonyPatch("PawnNotAssignableReason")]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(RitualRole), typeof(Precept_Ritual),
        typeof(RitualRoleAssignments), typeof(TargetInfo), typeof(bool) }, new ArgumentType[]
        {ArgumentType.Normal,ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal,ArgumentType.Ref})]
    public static class AlphaMemes_RitualRoleAssignments_PawnNotAssignableReason_Patch
    {

        public static void Postfix(Precept_Ritual ritual, Pawn p, ref string __result)
        {
            if (ritual?.def == InternalDefOf.AM_SkyBurial)
            {

                if (__result == "MessageRitualWontAttendExtremeTemperature".Translate(p))
                {
                    __result = null;
                }


            }


        }

    }





}
