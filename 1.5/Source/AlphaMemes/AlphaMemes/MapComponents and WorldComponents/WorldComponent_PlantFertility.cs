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

        public Dictionary<Plant, float> plants_and_fertility = new Dictionary<Plant, float>();
       

        public static WorldComponent_PlantFertility Instance;

        public WorldComponent_PlantFertility(World world) : base(world) => Instance = this;



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
                plants_and_fertility[plant] = 1.2f;
            }
            else
            {
                plants_and_fertility[plant] = 0.8f;

            }

        }

        


    }


}
