using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using RimWorld.Planet;
using System.Linq;
using System;
using RimWorld.BaseGen;


namespace AlphaMemes
{




    /*This Harmony Postfix tries to remove some of the Alpha Memes floors from map ruins generation
   */
    [HarmonyPatch(typeof(BaseGenUtility))]
    [HarmonyPatch("TryRandomInexpensiveFloor")]
    public static class AlphaMemes_BaseGenUtility_TryRandomInexpensiveFloor_Patch
    {
        [HarmonyPostfix]
        public static void RemoveAlphaMemesFloorsFromRuins(ref bool __result, ref TerrainDef floor)

        {

            if (floor != null && floor.designatorDropdown == InternalDefOf.AM_Floor_NeolithicTiles)
            {
                List<TerrainDef> vanillaTiles = new List<TerrainDef>();
                vanillaTiles.Add(TerrainDef.Named("TileSandstone"));
                vanillaTiles.Add(TerrainDef.Named("TileGranite"));
                vanillaTiles.Add(TerrainDef.Named("TileLimestone"));
                vanillaTiles.Add(TerrainDef.Named("TileSlate"));
                vanillaTiles.Add(TerrainDef.Named("TileMarble"));
                floor = vanillaTiles.RandomElement();
            }


        }

    }



}
