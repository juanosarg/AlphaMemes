
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{

    public class ThingSetMaker_StartingItemsMadness : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {           
            Thing thing = ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null);         
            outThings.Add(thing);         
        }

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            throw new NotImplementedException();
        }



    }
}

