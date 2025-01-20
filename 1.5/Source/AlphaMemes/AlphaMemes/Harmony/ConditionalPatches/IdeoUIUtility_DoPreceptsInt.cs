using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;

namespace AlphaMemes
{
    /*[HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("DoPreceptsInt")]*/
    public static class AlphaMemes_IdeoUIUtility_DoPreceptsInt_Patch
    {
        public static IEnumerable<CodeInstruction> TranspilePreceptInt(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                if (i > 0 && codes[i - 1].opcode == OpCodes.Ldloc_S && codes[i - 1].operand is LocalBuilder lb && lb.LocalIndex == 17)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)AlphaMemes_Settings.ritualsAmount);
                }
                else if (i > 0 && codes[i - 1].opcode == OpCodes.Ldstr && (string)codes[i - 1].operand == "MaxRitualCount")
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)AlphaMemes_Settings.ritualsAmount);
                }
                else
                {
                    yield return code;
                }
            }
        }
    }
}