
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{

    public class ThingSetMaker_StartingItemsNonViolent : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {
            ThingDef stuff = GenStuff.RandomStuffByCommonalityFor(InternalDefOf.AM_TrapBlunt, TechLevel.Undefined);
            Thing thing = ThingMaker.MakeThing(InternalDefOf.AM_TrapBlunt, stuff);
            Thing thing2 = ThingMaker.MakeThing(InternalDefOf.AM_TrapBlunt, stuff);
            Thing thing3 = ThingMaker.MakeThing(InternalDefOf.AM_TrapBlunt, stuff);
            Thing thing4 = ThingMaker.MakeThing(InternalDefOf.AM_TrapBlunt, stuff);

            outThings.Add(thing);
            outThings.Add(thing2);
            outThings.Add(thing3);
            outThings.Add(thing4);

        }

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            throw new NotImplementedException();
        }



    }
}

