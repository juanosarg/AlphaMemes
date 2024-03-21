using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_GauralenOrAltar : RitualObligationTargetWorker_AnyRitualSpotOrAltar
    {
        public RitualObligationTargetWorker_GauralenOrAltar()
        {
        }
        public RitualObligationTargetWorker_GauralenOrAltar(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (target.HasThing && target.Thing.def == ThingDefOf.Plant_TreeGauranlen)
            {
                return true;
            }            
            return base.CanUseTargetInternal(target, obligation).failReason;               
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "Funeral_Gauralen".Translate();
            foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
            {
                yield return text;
            }
            yield return ThingDefOf.RitualSpot.LabelCap;
            yield break;
        }
    }
}
