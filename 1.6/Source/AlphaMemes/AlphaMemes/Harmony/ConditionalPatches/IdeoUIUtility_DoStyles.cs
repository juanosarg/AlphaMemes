using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;

namespace AlphaMemes
{
   /* [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("DoStyles")]*/
    public static class AlphaMemes_IdeoUIUtility_DoStyles_Patch
    {
        public static IEnumerable<CodeInstruction> TranspileStyles(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                if (i > 0 && codes[i - 1].opcode == OpCodes.Ldloc_1 && codes[i].opcode == OpCodes.Ldc_I4_3)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(AlphaMemes_IdeoUIUtility_DoStyles_Patch), nameof(ChangeMaxStyles)));
                }
                else
                {
                    yield return code;
                }
            }
        }
        public static int ChangeMaxStyles()
        {
            return (int)AlphaMemes_Settings.stylesAmount;
        }
    }
}