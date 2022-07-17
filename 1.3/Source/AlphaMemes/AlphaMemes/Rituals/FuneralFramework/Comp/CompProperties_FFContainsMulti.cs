using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class CompProperties_CorpseContainerMulti : CompProperties_CorpseContainer
    {

        //spawning properties of the outcome
        public int maxCorpse = 5;

        public CompProperties_CorpseContainerMulti()
        {
            this.compClass = typeof(Comp_CorpseContainerMulti);
        }

    }
}
