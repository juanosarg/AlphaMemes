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
            if (Find.TickManager.TicksGame < checkTick)
            {                
                return lastResult;
            }
            checkTick = Find.TickManager.TicksGame + 600;//None of this is changing very often
            //Not desiccated, has brain, has research
            if (obligation?.targetA.Thing.ParentHolder is Building_CryptosleepCasket)
            {
                return lastResult = false;//If cryptosleeped dont show the gizmo
            }
            Corpse corpse = (Corpse)obligation.targetA.Thing;
            if (corpse.IsDessicated() && !allowDesiccated)
            {
                return lastResult = "Funeral_DreadnoughtDessicated".Translate(corpse.InnerPawn.NameFullColored.Named("CORPSE"));
            }
            if(corpse.InnerPawn.health.hediffSet.GetBrain() == null)
            {
                return lastResult = "Funeral_DreadnoughtBrainGone".Translate(corpse.InnerPawn.NameFullColored.Named("CORPSE"));
            }
            ResearchProjectDef research = FuneralFrameWork_StaticStartup.VFEP_SpacerWarcaskets;
            if (!research?.IsFinished ?? true)
            {
                return lastResult = "Funeral_ResearchNotCompleted".Translate(research.label);
            }

            return lastResult = true;
		}


        
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            yield return "Funeral_DreadnoughtViable".Translate();
            yield break;
        }


        public int checkTick;
        public RitualTargetUseReport lastResult;

    }
}
