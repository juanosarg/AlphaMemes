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

        }

        public static void PatchVFEPirate(Harmony harmony)
        {
            //Occupant patch
            var postfix = typeof(Building_WarcasketFoundry_OccupantAliveAndPresent_Patch).GetMethod("Postfix");
            if (postfix != null)
            {
                harmony.Patch(AccessTools.PropertyGetter("VFEPirates.Building_WarcasketFoundry:OccupantAliveAndPresent"), postfix: new HarmonyMethod(postfix));
            }
            //Draw Patch          
            postfix = typeof(Building_WarcasketFoundry_Draw_Patch).GetMethod("Postfix");
            if (postfix != null)
            {
                harmony.Patch(AccessTools.Method("VFEPirates.Building_WarcasketFoundry:Draw"), postfix: new HarmonyMethod(postfix));
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
