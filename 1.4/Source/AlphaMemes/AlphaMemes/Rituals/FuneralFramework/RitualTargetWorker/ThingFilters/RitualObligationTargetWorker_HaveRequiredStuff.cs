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
        public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
        {
            return base.GetTargets(obligation, map);
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            //No longer using, use behavior worker canStartNow

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
