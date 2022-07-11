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
            if (obligation?.targetA.Thing.ParentHolder is Building_CryptosleepCasket)
            {
                return false;//If cryptosleeped dont show the gizmo
            }
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
            ResearchProjectDef research = FuneralFrameWork_StaticStartup.VFEP_SpacerWarcaskets;
            if (!research?.IsFinished ?? true)
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


        public BodyPartRecord brain;
        public int lastCheck;
        
    }
}
