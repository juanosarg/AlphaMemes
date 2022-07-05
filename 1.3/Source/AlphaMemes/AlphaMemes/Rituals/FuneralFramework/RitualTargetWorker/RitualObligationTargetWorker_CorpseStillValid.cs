using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    //Not Implemented for anything by itself just base for dreadnought for now
    //Making this its own class as part of the mod extension as its not a proper target worked that can work by itself
    //Same return types for 2 relevant parts
    public class RitualObligationTarget_CorpseStillValid 
    {
        public RitualObligationTarget_CorpseStillValid()
        {
        }


        public virtual RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
        {

            return true;
		}



        public virtual IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {

            yield return "Funeral_CorpseViable".Translate();
            yield break;
        }


        public bool allowDesiccated = true;

        
    }
}
