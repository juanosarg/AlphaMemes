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

            extension = def.GetModExtension<ObligationTargetExtension>();

            RitualTargetUseReport temp;
            bool canUse = true;
            StringBuilder failReasons = new StringBuilder();

            foreach(RitualObligationTargetFilter worker in filters)
            {
                temp = worker.CanUseTarget(target, obligation);
                if(!temp.canUse) { canUse = false; }
                if(temp.failReason != null)
                {
                    failReasons.AppendInNewLine(temp.failReason);                   
                }
            }
            //Do Corpse reasons
            if(extension.validCorpse != null)
            {
                temp = extension.validCorpse.CanUseTarget(target, obligation);
                if (!temp.canUse) { canUse = false; }
                if (temp.failReason != null)
                {
                    failReasons.AppendInNewLine(temp.failReason);
                }
            }
            if (failReasons.Length>0)
            {
                return failReasons.ToStringSafe();
            }
            else if (!canUse) { return false; }
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
    }
}
