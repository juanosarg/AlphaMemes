using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;



namespace AlphaMemes
{
    //Setting the Harmony instance
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

        }
     

     }

}
