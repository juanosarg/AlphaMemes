using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class CompProperties_CorpseContainerMulti : CompProperties
    {

        //spawning properties of the outcome
        public string transformLabel = null;
        public string inspectString = null;
        public int maxCorpse = 5;

        public CompProperties_CorpseContainerMulti()
        {
            this.compClass = typeof(Comp_CorpseContainerMulti);
        }

    }
}
