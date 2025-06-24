using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class CompAbilityConvertStone : CompAbilityEffect
    {
       

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {

            base.Apply(target, dest);
            foreach (Thing item in GenRadial.RadialDistinctThingsAround(target.Cell, parent.pawn.Map, 3, useCenter: true))
            {
                if (item?.def.defName.Contains("Block")==true)
                {

                    IntVec3 position = item.Position;
                    Map map = item.Map;
                    int stack = item.stackCount;
                    item.Destroy();
                    ThingDef newThing = InternalDefOf.AM_BlocksPristineLimestone;
                    Thing stones = GenSpawn.Spawn(newThing, position, map, WipeMode.Vanish);
                    stones.stackCount = stack;
                }
            }

            








        }



    }
}
