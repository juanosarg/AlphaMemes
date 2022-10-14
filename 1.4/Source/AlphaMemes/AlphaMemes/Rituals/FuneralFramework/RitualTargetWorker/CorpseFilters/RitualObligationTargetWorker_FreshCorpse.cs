using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{

    public class RitualObligationTarget_FreshCorpse : RitualObligationTarget_CorpseStillValid
    {
        public RitualObligationTarget_FreshCorpse()
        {
        }


        public override RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
        {
            Corpse corpse = obligation.targetA.Thing as Corpse;
            if (corpse.IsNotFresh())
            {
                return "Funeral_CorpseNotFresh".Translate(corpse.InnerPawn.NameShortColored.Named("CORPSE"));
            }
            return true;
		}



        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {

            yield return "Funeral_FreshCorpse".Translate();
            yield break;
        }


        
    }
}
