using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
    //Well I dont have an extension on this def yet...
    //Used to customise spot placement. Remember spot has to be standable
    public class FuneralFramework_BehaviorExtension : DefModExtension
    {

        //public IntVec3 spotOffset = IntVec3.Zero;
        public Dictionary<ThingDef, IntVec3> spotOffset = new Dictionary<ThingDef, IntVec3>();
    }




}
