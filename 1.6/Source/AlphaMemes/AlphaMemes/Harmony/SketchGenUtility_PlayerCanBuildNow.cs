using HarmonyLib;
using RimWorld;
using RimWorld.SketchGen;
using Verse;
using VEF.Memes;
using System.Linq;


namespace AlphaMemes
{


    [HarmonyPatch(typeof(SketchGenUtility))]
    [HarmonyPatch("PlayerCanBuildNow")]
    public static class AlphaMemes_SketchGenUtility_PlayerCanBuildNow_Patch
    {
        [HarmonyPostfix]
        static void DisableIdeoFloors(ref bool __result, BuildableDef buildable)
        {

            if (StaticCollections.designatorsToBeRemoved.Contains(buildable.designatorDropdown))
            {
                __result = false;
            }
            //Adding to prevent monuments from adding things that players can't build
            foreach (var ideo in Faction.OfPlayer.ideos.AllIdeos)
            {
                foreach (var meme in ideo.memes)
                {
                    var ext = meme.GetModExtension<ExtendedMemeProperties>();
                    if (ext!=null && (ext.removedDesignators?.Any(x=>x == (buildable as ThingDef) ) ?? false))
                    {
                        __result = false;
                    }
                }
            }




        }
    }








}
