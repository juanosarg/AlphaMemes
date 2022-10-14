using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_CorpseContainerMulti : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_CorpseContainerMulti()
        {
        }
        public RitualObligationTargetWorker_CorpseContainerMulti(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {                
                return base.CanUseTargetInternal(target, obligation).failReason;               
            }

            Comp_CorpseContainerMulti comp = target.Thing.TryGetComp<Comp_CorpseContainerMulti>();
            if (comp == null)
            {
                var singleComp = target.Thing.TryGetComp<Comp_CorpseContainer>();
                if (singleComp == null)
                {
                    return false;
                }
                if (singleComp.Active)
                {
                    return "Funeral_CorpseContainerFull".Translate(target.Thing.Label);
                }
                return true;
            }
            if (!comp.NotFull)
            {
                return "Funeral_CorpseContainerFull".Translate(target.Thing.Label);
            }            

            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            return base.GetTargetInfos(obligation);
        }
    }
}
