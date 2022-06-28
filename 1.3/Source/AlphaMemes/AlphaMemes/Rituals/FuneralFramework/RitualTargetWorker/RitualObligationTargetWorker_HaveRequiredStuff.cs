using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{

    public class RitualObligationTargetWorker_HaveRequiredStuff : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_HaveRequiredStuff()
        {
        }
        public RitualObligationTargetWorker_HaveRequiredStuff(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {
                return false;           
            }


            OutcomeEffectExtension data = parent.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();            
            StringBuilder failReasons = new StringBuilder();
            foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners.Where(x=> x.stuffCount>0))
            {
                if (!spawner.CanStart())
                {                    
                    failReasons.AppendInNewLine("Funeral_NotEnoughStuff".Translate(string.Join(", ", spawner.stuffCategoryDefs.Select(x => x.label)).Named("STUFF"), spawner.stuffCount.ToString().Named("COUNT")));
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
            //Use extension as this gets called before map is even created so always use defaults
            OutcomeEffectExtension extension = parent.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();            
            foreach(FuneralFramework_ThingToSpawn spawner in extension.outcomeSpawners.Where(x => x.stuffCount > 0))
            {
                string stringReturn = "Funeral_XOf".Translate(spawner.stuffCount.ToString());
                stringReturn = stringReturn + "(" + string.Join(", ", spawner.stuffCategoryDefs.Select(x => x.label)) + ")"; 
                yield return stringReturn;
            }
       
            yield break;
        }

    
        
        
    }
}
