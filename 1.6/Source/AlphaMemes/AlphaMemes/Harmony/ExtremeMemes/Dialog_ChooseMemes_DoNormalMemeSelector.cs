using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using System;
using System.Reflection;
using VEF.AnimalGenes;

namespace AlphaMemes
{
    [HarmonyPatch]
    public static class AlphaMemes_Dialog_ChooseMemes_DoNormalMemeSelector_Patch
    {

        [HarmonyPatch(typeof(Dialog_ChooseMemes))]
        [HarmonyPatch("DoNormalMemeSelector")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AdjustImpactOnDialog(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
          

            for (var i = 0; i < codes.Count; i++)
            {

                if (codes[i].opcode == OpCodes.Ldc_I4_3)
                {

                    yield return new CodeInstruction(OpCodes.Ldc_I4_4);

                }
               

                else yield return codes[i];
            }
        }





    }
}