using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{

    public class RitualObligationTargetWorker_HaveRequiredThings : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_HaveRequiredThings()
        {
        }
        public RitualObligationTargetWorker_HaveRequiredThings(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (!def.thingDefs.NullOrEmpty())
            {
                if (!base.CanUseTargetInternal(target, obligation).canUse)
                {
                    return false;
                }
            }
            OutcomeEffectExtension data = parent.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();            
            StringBuilder failReasons = new StringBuilder();
            foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners.Where(x=> x.thingsRequired.Count>0))
            {
                AcceptanceReport report = spawner.CanStartThings();
                if(!report.Accepted)
                {                    
                    failReasons.AppendInNewLine("Funeral_NotEnoughThings".Translate(report.Reason.Named("Missing")));
                }
            }

            if (failReasons.Length > 0)
            {
                return failReasons.ToString();
            }
            return true;
		}


        
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            OutcomeEffectExtension extension = parent.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();
            StringBuilder targetInfos = new StringBuilder("Funeral_XOfThing".Translate());
            foreach (FuneralFramework_ThingToSpawn spawner in extension.outcomeSpawners.Where(x => x.thingsRequired.Count > 0))
            {
                foreach (KeyValuePair<ThingDef, int> thing in spawner.thingsRequired)
                {
                    targetInfos.AppendInNewLine(thing.Key.LabelCap + ": " + thing.Value.ToString());
                }
                yield return targetInfos.ToString();
            }
       
            yield break;
        }

    
        
        
    }
}
