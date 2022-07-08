using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(Dialog_BeginRitual))]
    [HarmonyPatch("CalculatePawnPortraitIcons")]

    //This patch removes the sleeping icon for dead pawns
    public static class FuneralFramework_Dialog_BeginRitual_CalculatePawnPortraitIcons_Patch
    {
      
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo IsAwake = AccessTools.Method(typeof(RestUtility), nameof(RestUtility.Awake));
            List<CodeInstruction> codes = instructions.ToList();
            bool found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (!found && codes[i].opcode == OpCodes.Call && codes[i].operand == (object)IsAwake)
                {
                    
                    found = true;
                    codes[i].opcode = OpCodes.Nop;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FuneralFramework_Dialog_BeginRitual_CalculatePawnPortraitIcons_Patch), nameof(IsAwakeNotDead)));

                }
            }
        }


        public static bool IsAwakeNotDead(Pawn pawn)
        {
            if(pawn.Dead == true)
            {
                return true;
            }
            return pawn.Awake();
        }
    }

}
