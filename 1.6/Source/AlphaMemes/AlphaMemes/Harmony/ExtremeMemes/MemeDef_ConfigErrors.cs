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
    public static class AlphaMemes_MemeDef_ConfigErrors_Patch
    {
        public static MethodInfo moveNext;

        public static MethodBase TargetMethod()
        {
            moveNext = AccessTools.EnumeratorMoveNext(AccessTools.Method(typeof(MemeDef), "ConfigErrors"));
            return moveNext;
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AdjustConfigErrorsForMemeImpact(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var impactField = AccessTools.Field(typeof(MemeDef), "impact");
            var defNameField = AccessTools.Field(typeof(Def), "defName");


            for (var i = 0; i < codes.Count; i++)
            {

                if (i > 0 && codes[i-1].opcode == OpCodes.Ldfld && codes[i-1].OperandIs(impactField) && codes[i].opcode == OpCodes.Ldc_I4_3)
                {
                    
                    yield return new CodeInstruction(OpCodes.Ldc_I4_4);
                   
                }else 
                if (i > 0 && codes[i - 1].opcode == OpCodes.Ldfld && codes[i - 1].OperandIs(defNameField) && codes[i].opcode == OpCodes.Ldc_I4_3)
                {

                    yield return new CodeInstruction(OpCodes.Ldc_I4_4);

                }

                else yield return codes[i];
            }
        }

       



    }
}