
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{

    public class ThingSetMaker_StartingItemsPainIsDivine : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {
            Thing thing = ThingMaker.MakeThing(InternalDefOf.Mindscrew, null);
            outThings.Add(thing);
        }

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            throw new NotImplementedException();
        }



    }
}

