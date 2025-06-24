using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_NutrientPaste : RitualObligationTargetWorker_FuneralThingNeedsPower
    {
        public RitualObligationTargetWorker_NutrientPaste()
        {
        }
        public RitualObligationTargetWorker_NutrientPaste(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {                
                return base.CanUseTargetInternal(target, obligation).failReason;               
            }

            Building_NutrientPasteDispenser thing = target.Thing as Building_NutrientPasteDispenser;
            Pawn pawn = thing.Map.mapPawns.FreeColonistsSpawned.RandomElement();
            
            Building hopper = thing.AdjacentReachableHopper(pawn);
            if(hopper == null)
            {
                return "Funeral_NoHopper";
            }

            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "Funeral_NutrientPaste".Translate(string.Join(", ",this.def.thingDefs.Select(x=> x.label)));
            yield break;
        }
    }
}
