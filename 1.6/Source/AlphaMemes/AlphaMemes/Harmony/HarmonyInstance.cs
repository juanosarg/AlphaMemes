using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;



namespace AlphaMemes
{
    //Manual patches that for one reason or another need to be called after Mod
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var method = AccessTools.Method(typeof(PregnancyUtility), "ChanceMomDiesDuringBirth");
            var postfix = typeof(AlphaMemes_PregnancyUtility_ChanceMomDiesDuringBirth_Patch).GetMethod("RemoveMaternalMortality");
            AlphaMemes_Mod.harmony.Patch(method, postfix: postfix);

            var method2 = AccessTools.Method(typeof(PregnancyUtility), "ApplyBirthOutcome");
            var postfix2 = typeof(AlphaMemes_PregnancyUtility_ApplyBirthOutcome_Patch).GetMethod("DevelopmentPointsForChildbirth");
            AlphaMemes_Mod.harmony.Patch(method2, postfix: postfix2);

            var method3 = AccessTools.Method(typeof(PawnApparelGenerator), "GenerateStartingApparelFor");
            var postfix3 = typeof(AlphaMemes_PawnApparelGenerator_GenerateStartingApparelFor_Patch).GetMethod("RemoveApparel");
            AlphaMemes_Mod.harmony.Patch(method3, postfix: postfix3);

        }
     

     }

}
