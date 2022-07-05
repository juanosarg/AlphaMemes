using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
    //Used to extend functionality of obligation to allow more then 1 filter per pattern
    public class ObligationTargetExtension : DefModExtension
    {

        public List<RitualObligationTargetFilterDef> filters;
        //Target
        public RitualObligationTarget_CorpseStillValid validCorpse;
    }




}
