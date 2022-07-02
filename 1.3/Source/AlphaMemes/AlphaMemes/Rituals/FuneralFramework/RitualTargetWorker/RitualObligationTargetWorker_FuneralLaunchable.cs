using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_Launchable : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_Launchable()
        {
        }
        public RitualObligationTargetWorker_Launchable(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {                
                return false;               
            }
            
            Thing thing = target.Thing;

            CompLaunchable compLaunchable= thing.TryGetComp<CompLaunchable>();


            if (compLaunchable != null)
			{
				if (!compLaunchable.FuelingPortSourceHasAnyFuel)
				{
                    return "Funeral_LaunchableNoFuel".Translate(thing.def.label);
                    
                }
                if(thing.Position.Roofed(thing.Map))
                {
                    return "Funeral_LaunchableRoofed".Translate(thing.def.label);
                }

            }
            
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "Funeral_ThingLaunchable".Translate(string.Join(", ",this.def.thingDefs.Select(x=> x.label)));
            yield break;
        }
    }
}
