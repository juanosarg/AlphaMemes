using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using AlphaPrefabs;
using UnityEngine.UIElements;
using Verse.Noise;
using System.Linq;
using RimWorld.Planet;
using System.Collections;

namespace AlphaMemes
{
    public class WorldComponent_PlantFertility : WorldComponent
    {

        public Dictionary<Plant, bool> plants_and_fertility = new Dictionary<Plant, bool>();
        List<Plant> list1;
        List<bool> list2;

        public static WorldComponent_PlantFertility Instance;

        public WorldComponent_PlantFertility(World world) : base(world) => Instance = this;



        public override void ExposeData()
        {
            base.ExposeData();
           // Scribe_Collections.Look(ref plants_and_fertility, "plants_and_fertility", LookMode.Reference, LookMode.Value, ref list1, ref list2);

        }

        public void CheckIfPlantExtraFertile(Plant plant,Map map)
        {
            int num = GenRadial.NumCellsInRadius(3);
            bool waterFound=false;
            for (int i = 0; i < num; i++)
            {
                IntVec3 c = plant.Position + GenRadial.RadialPattern[i];
                if (c.InBounds(map))
                {
                    TerrainDef terrain = c.GetTerrain(map);

                    if (terrain != null && terrain.IsWater)
                    {
                        waterFound = true;
                        break;
                    }

                }

            }
            if (waterFound)
            {
                plants_and_fertility[plant] = true;
            }

        }

        


    }


}
