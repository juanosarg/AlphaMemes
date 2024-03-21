using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    

    public class RitualObligationTrigger_AlsoAddsDreadnought : RitualObligationTrigger_AlsoAdds
    {

  
        public override bool UseAlternate(Pawn p, RitualObligation obligation)
        {
            ResearchProjectDef research = DefDatabase<ResearchProjectDef>.GetNamed("VFEP_SpacerWarcaskets", false);
            if (!research?.IsFinished ?? true)
            {
                return true;
            }
            return false;
        }


    }


}
