using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    //If this works its a way to start a ritual from any thing, but that thing wont be the ritual spot
    public class RitualObligationTargetWorker_HighTechThing : RitualObligationTargetFilter
    {
        public RitualObligationTargetWorker_HighTechThing()
        {
        }
        public RitualObligationTargetWorker_HighTechThing(RitualObligationTargetFilterDef def) : base(def)
        {
        }

        public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
        {
            var things = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableAlways);
            for (int i = 0; i < things.Count; i++)
            {
                if (base.CanUseTarget(things[i], obligation).canUse)//Not looping all things for the sake of a lookletter
                {
                    yield return things[i];
                    break;
                }
            }
            yield break;
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if(Find.TickManager.TicksGame < checkTick && cacheTarget == target) 
            {                
                return lastResult;
            }
            checkTick = Find.TickManager.TicksGame + 300;
            cacheTarget = target;
            var thing = target.Thing.GetInnerIfMinified();
            if (!thing.def.alwaysHaulable)
            {
                return lastResult = false;
            }
            if (thing.MarketValue >= 100 && (thing.def.techLevel >= TechLevel.Spacer|| (thing.def.researchPrerequisites?.Any(x=>x.techLevel>=TechLevel.Spacer)?? false)))
            {
                if (filter == null)
                {
                    filterDef = def.GetModExtension<ObligationTargetExtension>().targetFilter;
                    filter = filterDef.GetInstance();
                }
                if (!filter.CanStart(thing, thing, out string reject))
                {
                    return lastResult = reject;
                }
                return lastResult = true;
            }
            return lastResult = false;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            yield return "AM_ScrapperHighTechThing".Translate();
            yield break;
        }
        public override void ExposeData()
        {
            base.ExposeData();
        }
        public RitualTargetFilterDef filterDef;
        public RitualTargetFilter filter;
        public int checkTick;
        public RitualTargetUseReport lastResult;
        private TargetInfo cacheTarget;
    }
}
