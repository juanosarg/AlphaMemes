using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_FuneralThingExtended : RitualObligationTargetFilter
    {
        public RitualObligationTargetWorker_FuneralThingExtended()
        {
        }
        public RitualObligationTargetWorker_FuneralThingExtended(RitualObligationTargetFilterDef def) : base(def)
        {
            extension = def.GetModExtension<ObligationTargetExtension>();
        }
        //would love to do this at construction but ritual hasnt been passed until after
        public virtual void initFilters(Precept_Ritual ritual)
        {
            extension = def.GetModExtension<ObligationTargetExtension>();

            foreach (RitualObligationTargetFilterDef filter in extension.filters)
            {
                RitualObligationTargetFilter instance = filter.GetInstance();
                if (instance != null)
                {
                    instance.parent = ritual;
                    filters.Add(instance);                    
                }
            }
        }


        public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
        {
            extension = def.GetModExtension<ObligationTargetExtension>();

            foreach (RitualObligationTargetFilter worker in filters)
            {
                foreach(TargetInfo target in worker.GetTargets(obligation, map))
                {
                    yield return target;
                }              
            }
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            //Slight change here making note of. Return of false = dont show gizo, return of fail string = show gizmo + fail reason. If any of the filters return false we need to not show gizmo at all even if another returns string
            if(Find.TickManager.TicksGame < checkTick && cacheTarget == target && cacheObli == obligation) //In profiling this 1 method can be .1-.2 ms because it gets called every frame when any thing is selected * # of funerals.
            {                
                return lastResult;
            }
            checkTick = Find.TickManager.TicksGame + 300;
            cacheTarget = target;
            cacheObli = obligation;
            extension = def.GetModExtension<ObligationTargetExtension>();

            RitualTargetUseReport temp;
            bool cansShowGizmo = true;
            StringBuilder failReasons = new StringBuilder();

            foreach(RitualObligationTargetFilter worker in filters)
            {
                temp = worker.CanUseTarget(target, obligation);
                if(!temp.canUse && temp.failReason == null) { cansShowGizmo = false; }
                if(temp.failReason != null)
                {
                    failReasons.AppendInNewLine(temp.failReason);                   
                }
            }
            //Do Corpse reasons
            if(extension.validCorpse != null)
            {
                temp = extension.validCorpse.CanUseTarget(target, obligation);
                if (!temp.canUse && temp.failReason == null) { cansShowGizmo = false; }
                if (temp.failReason != null)
                {
                    failReasons.AppendInNewLine(temp.failReason);
                }
            }
            if (!cansShowGizmo)
            {
                lastResult = false;
                return false;
            }
            else if (failReasons.Length > 0) 
            {
                lastResult = failReasons.ToStringSafe();
                return failReasons.ToStringSafe();
            }
            lastResult = true;
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {

            extension = def.GetModExtension<ObligationTargetExtension>();
            foreach (RitualObligationTargetFilter worker in filters)
            {
                foreach(string str in worker.GetTargetInfos(obligation))
                {
                    yield return str;
                }
            }
            if (extension.validCorpse != null)
            {
                foreach (string str in extension.validCorpse.GetTargetInfos(obligation))
                {
                    yield return str;
                }
            }
            yield break;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<RitualObligationTargetFilter>(ref filters, "filters", LookMode.Deep, Array.Empty<object>());
        }
        ObligationTargetExtension extension;
        List<RitualObligationTargetFilter> filters = new List<RitualObligationTargetFilter>();
        public int checkTick;
        public RitualTargetUseReport lastResult;
        private TargetInfo cacheTarget;
        private RitualObligation cacheObli;
    }
}
