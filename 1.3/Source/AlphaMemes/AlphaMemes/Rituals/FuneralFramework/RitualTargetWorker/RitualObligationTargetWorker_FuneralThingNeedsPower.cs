using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_FuneralThingNeedsPower : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_FuneralThingNeedsPower()
        {
        }
        public RitualObligationTargetWorker_FuneralThingNeedsPower(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {                
                return base.CanUseTargetInternal(target, obligation).failReason;               
            }
            
            Thing thing = target.Thing;
           
            CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
                    return "Funeral_NeedsPower".Translate(obligation.targetA.Thing.def.label);
                    
                }				
			}
            
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "Funeral_ThingInfoPower".Translate(string.Join(", ",this.def.thingDefs.Select(x=> x.label)));
            yield break;
        }
    }
}
