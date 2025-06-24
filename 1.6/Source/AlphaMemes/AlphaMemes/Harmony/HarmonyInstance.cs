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
            var harmony = new Harmony("com.alphamemes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            if (FuneralFrameWork_StaticStartup.VFEPLoaded)
            {
                PatchVFEPirate(harmony);
            }
            if (!ModLister.HasActiveModWithName("Vanilla Ideology Expanded - Memes and Structures"))
            {
                PatchVEMemesOptions(harmony);
            }

        }

        public static void PatchVFEPirate(Harmony harmony)
        {
            //Occupant patch
            var postfix = typeof(Building_WarcasketFoundry_OccupantAliveAndPresent_Patch).GetMethod("Postfix");
            
            var type = AccessTools.TypeByName("VFEPirates.Building_WarcasketFoundry");
            var method = AccessTools.PropertyGetter(type,"OccupantAliveAndPresent");
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

        public static void PatchVEMemesOptions(Harmony harmony)
        {
            var method = AccessTools.PropertyGetter(typeof(Dialog_ChooseMemes), "MemeCountRangeAbsolute");
            var postfix = typeof(AlphaMemes_Dialog_ChooseMemes_MemeCountRangeAbsolute_Patch).GetMethod("SetMaxToOptions");
            harmony.Patch(method, postfix: postfix);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoMemes");
            var prefix = typeof(AlphaMemes_IdeoUIUtility_DoMemes_Patch).GetMethod("MakeBoxSmaller");
            postfix = typeof(AlphaMemes_IdeoUIUtility_DoMemes_Patch).GetMethod("MakeBoxBigger");
            harmony.Patch(method, prefix: prefix);
            harmony.Patch(method, postfix: postfix);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoPrecepts");         
            postfix = typeof(AlphaMemes_IdeoUIUtility_DoPrecepts_Amount_Patch).GetMethod("EnableMorePrecepts");       
            harmony.Patch(method, postfix: postfix);
 
            method = AccessTools.Method(typeof(IdeoUIUtility), "AddPrecept");
            var transpiler = typeof(AlphaMemes_IdeoUIUtility_AddPrecept_Patch).GetMethod("TranspileAddPrecept");
            harmony.Patch(method, transpiler: transpiler);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoStyles");
            transpiler = typeof(AlphaMemes_IdeoUIUtility_DoStyles_Patch).GetMethod("TranspileStyles");
            harmony.Patch(method, transpiler: transpiler);

            method = AccessTools.Method(typeof(IdeoUtility), "IsMemeAllowedForInitialFluidIdeo");
            postfix = typeof(AlphaMemes_IdeoUtility_IsMemeAllowedForInitialFluidIdeo_Patch).GetMethod("AllowAllMemes");
            harmony.Patch(method, postfix: postfix);

        }

     }

}
