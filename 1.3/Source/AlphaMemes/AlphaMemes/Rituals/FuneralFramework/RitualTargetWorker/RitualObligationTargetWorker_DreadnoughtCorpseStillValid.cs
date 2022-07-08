using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    
    public class RitualObligationTarget_DreadnoughtCorpseStillValid : RitualObligationTarget_CorpseStillValid
    {
        public RitualObligationTarget_DreadnoughtCorpseStillValid()
        {
        }
        public override RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
        {
            //Not desiccated, has brain, has research
            Corpse corpse = (Corpse)obligation.targetA.Thing;
            if (corpse.Destroyed)
            {
                return true; //stop gap for obligations on destroyed corpses
            }
            if (corpse.IsDessicated() && !allowDesiccated)
            {
                return "Funeral_DreadnoughtDessicated".Translate(corpse.InnerPawn.NameFullColored.Named("CORPSE"));
            }
            if(corpse.InnerPawn.health.hediffSet.GetBrain() == null)
            {
                return "Funeral_DreadnoughtBrainGone".Translate(corpse.InnerPawn.NameFullColored.Named("CORPSE"));
            }
            ResearchProjectDef research = DefDatabase<ResearchProjectDef>.GetNamed("VFEP_SpacerWarcaskets",false);
            if(!research?.IsFinished ?? true)
            {
                return "Funeral_ResearchNotCompleted".Translate(research.label);
            }
            return true;
		}


        
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            yield return "Funeral_DreadnoughtViable".Translate();
            yield break;
        }

       

        
    }
}
