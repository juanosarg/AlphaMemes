using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace AlphaMemes
{
    public class MapComponent_IsMapWatery : MapComponent
    {

      

        public MapComponent_IsMapWatery(Map map) : base(map)
        {

        }


        public override void FinalizeInit()
        {
            base.FinalizeInit();

            bool IsMapWatery = map.terrainGrid.topGrid.Where(x => x.IsWater)?.Count() > 100;

            StaticCollections.SetMapWateriness(map, IsMapWatery);
        }






    }


}



