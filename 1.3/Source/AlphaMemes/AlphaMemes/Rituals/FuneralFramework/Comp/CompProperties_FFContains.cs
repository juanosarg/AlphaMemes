using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class CompProperties_CorpseContainer : CompProperties
    {

        //spawning properties of the outcome
        public string transformLabel = null;
        public string inspectString = null;
        public RulePackDef descriptionMaker;
        public int ticksToOpen = 300;
        public bool destroyOnOpen = false;
        public CompProperties_CorpseContainer()
        {
            this.compClass = typeof(Comp_CorpseContainer);
        }

    }
}
