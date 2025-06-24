using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_FuneralHolderThings : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_FuneralHolderThings()
        {
        }
        public RitualObligationTargetWorker_FuneralHolderThings(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {                
                return base.CanUseTargetInternal(target, obligation).failReason;               
            }
            
            IThingHolder thing = target.Thing as IThingHolder;
            if(thing == null)
            {
                return false;
            }
            if (thing.GetDirectlyHeldThings().Any)
            {
                return "Funeral_ThingNotEmpty".Translate(target.Thing.Label);
            }
                        
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            return base.GetTargetInfos(obligation);
        }
    }
}
