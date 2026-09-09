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
            PatchVFEPirate(AlphaMemes_Mod.harmony);
        }

        public static void PatchVFEPirate(Harmony harmony)
        {
            //Occupant patch
            var postfix = typeof(Building_WarcasketFoundry_OccupantAliveAndPresent_Patch).GetMethod("Postfix");

            var type = AccessTools.TypeByName("VFEPirates.Building_WarcasketFoundry");
            var method = AccessTools.PropertyGetter(type, "OccupantAliveAndPresent");
            if (postfix != null && method != null)
            {
                harmony.Patch(method, postfix: new HarmonyMethod(postfix));
            }
            //Draw Patch          
            postfix = typeof(Building_WarcasketFoundry_Draw_Patch).GetMethod("Postfix");
            if (postfix != null)
            {
                harmony.Patch(AccessTools.Method("VFEPirates.Building_WarcasketFoundry:DrawAt"), postfix: new HarmonyMethod(postfix));
            }
            //Hediff patch
            postfix = typeof(RecipeWorker_WarcasketRemoval_AvailableOnNow_Patch).GetMethod("Postfix");
            if (postfix != null)
            {
                harmony.Patch(AccessTools.Method("VFEPirates.RecipeWorker_WarcasketRemoval:AvailableOnNow"), postfix: new HarmonyMethod(postfix));
            }
            //OskarPotocki.VFECore
            //Patch to let resurecttion penalties affect sarco casket
            postfix = typeof(PawnCapacityUtility_CalculateCapacityLevel_Patch).GetMethod("Postfix");
            if (postfix != null)
            {
                var harmonyPostfix = new HarmonyMethod(postfix)
                {
                    after = new[] { "OskarPotocki.VFECore" }
                };

                harmony.Patch(AccessTools.Method("Verse.PawnCapacityUtility:CalculateCapacityLevel"), postfix: harmonyPostfix);
            }
        }
    }

}
