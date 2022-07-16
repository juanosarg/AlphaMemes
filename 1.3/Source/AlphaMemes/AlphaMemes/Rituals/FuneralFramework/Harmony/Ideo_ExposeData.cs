using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(Ideo))]
    [HarmonyPatch("ExposeData")]

    //Fixing a vanilla issue on expose data that adds precepts in expose data without checking if they can be added to that ideo first.
    public static class FuneralFramework_Ideo_ExposeData_Patch 
    {

        static MethodInfo AddPrecept = AccessTools.Method(typeof(Ideo), "AddPrecept");
        static MethodInfo DebugLog = AccessTools.Method(typeof(Debug), "LogWarning", new Type[] { typeof(object)});
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            bool found = false;
            int debugCall = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (!found && codes[i].Calls(AddPrecept)) //Expose data only calls Add Precept in one case
                {                    
                    found = true;
                    
                    codes[i].opcode = OpCodes.Nop;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FuneralFramework_Ideo_ExposeData_Patch), nameof(CheckIfCanAdd)));
                    //Noping the next set to remove debug log as long its where I expect
                    if (codes[i + 7].Calls(DebugLog))
                    {
                        debugCall = i + 7;
                        //codes[i + 7].opcode = OpCodes.Nop; I dont trust you magical box//I feel like this shouldnt work because im only noping the call but it does, and I can't figure out why. Where do you go magical box on the stack
                    }
                }
                if(found && i <= debugCall)
                {
                    codes[i].opcode = OpCodes.Nop;
                }
            }
            if (!found)
            {
                Log.Message("AlphaMemes Transpiler on Ideo:ExposeData could not find hook");
            }
        }
        public static void CheckIfCanAdd(Ideo ideo, Precept precept, bool init = false, FactionDef generatingFor = null, RitualPatternDef ritualPatternBase = null)
        {
            if (ideo.foundation.CanAdd(precept.def))//Can Add filters things that shouldnt be added for memes and such
            {
                if (precept.def.canGenerateAsSpecialPrecept)
                {
                    ideo.AddPrecept(precept, true, null, ritualPatternBase);
                    Debug.LogWarning("A hidden ritual precept was missing, adding: " + precept.def.LabelCap);
                }

            }

        }
        //*Warning rambly wall of text below of me working through the issues*
        //I cant find any good way to determine whether hidden ritual should be added or not using just the def besides if there's a direct conflict
        //Example is the Dreadnought Crypto Funeral is getting added to everyone during expose when it shouldn't. 
        //I cant stop it, without also doing something that would break the ability for other modders myself included to have their rituals automatically added when someone adds mod
        //Eg Persona Bond Ritual; Where I thought it wouldnt work without a new save or at least reform. I now know it worked because of this.
        //So it's a dilema because having AlsoAdd Rituals that aren't visible opens up a tonne of possilibites, but I have to resolve this because it causes nasty annoying bugs otherwise.
        //Much digging later, the PreceptDef field canGenerateAsSpecialPrecept is the safest bet. It's default to true, and is how all the autoinclude rituals are setup.
        //Specifically canGenerateAsSpecialPrecept == true && countsTowardsPreceptLimit == False
        //This is also why it would add all of my funerals every time. I copied base funeral, which is actually always added because of the above.
        //canGenerateAsSpecialPrecept = false is what base nocorpse does to avoid the autoadd in random generation.
        //In this expose data it avoids autoadd with the takeNameFrom. Which is not useable for my purposes.
        //So that's what I'm adding as the extra criteria. It shouldnt hurt any modders as if they wanted it to be autoadd to ideo. Then it has to be true for it to work at all for them. (Barring some silliniess with harmony but hopefully no one is doing that)


    }

}
