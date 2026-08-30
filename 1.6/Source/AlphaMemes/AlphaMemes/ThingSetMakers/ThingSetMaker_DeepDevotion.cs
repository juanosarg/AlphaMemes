
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{

    public class ThingSetMaker_DeepDevotion : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {
            Thing thing = ThingMaker.MakeThing(InternalDefOf.Meat_Megaspider, null);
            thing.stackCount = 300;
            Thing thing2 = ThingMaker.MakeThing(InternalDefOf.RawFungus, null);
            thing2.stackCount = 300;
            outThings.Add(thing);
            outThings.Add(thing2);
        }

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            throw new NotImplementedException();
        }



    }
}

