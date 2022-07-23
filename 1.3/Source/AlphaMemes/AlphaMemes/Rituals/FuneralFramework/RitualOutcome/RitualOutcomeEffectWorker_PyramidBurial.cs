using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using RimWorld;

namespace AlphaMemes
{
    
    public class RitualOutcomeEffectWorker_PyramidBurial : RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_PyramidBurial() 
        { 
        }
        public RitualOutcomeEffectWorker_PyramidBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {           
            
            
            if (OutcomeChanceWorst(jobRitual, outcome) && outcomeExtension.worstOutcomeDesc != null)
            {                    
                extraOutcomeDesc = outcomeExtension.worstOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                
                CompQuality comp = jobRitual.selectedTarget.Thing.TryGetComp<CompQuality>();
                if (comp != null && comp.Quality>QualityCategory.Awful)
                {
                    comp.SetQuality(comp.Quality - 1,ArtGenerationContext.Colony);
                }

            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                CompQuality comp = jobRitual.selectedTarget.Thing.TryGetComp<CompQuality>();
                if (comp != null && comp.Quality < QualityCategory.Legendary)
                {
                    comp.SetQuality(comp.Quality + 1, ArtGenerationContext.Colony);
                }
            }
           
            
           
        }
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            Comp_CorpseContainerMulti comp = jobRitual.selectedTarget.Thing.TryGetComp<Comp_CorpseContainerMulti>();
            if(comp != null)
            {
                comp.InitComp_CorpseContainer(corpse);
            }
            else
            {
                Comp_CorpseContainer singeComp = jobRitual.selectedTarget.Thing.TryGetComp<Comp_CorpseContainer>();
                if (singeComp != null)
                {
                    singeComp.InitComp_CorpseContainer(corpse);
                }
            }

            base.ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
        }


        public override void ExposeData()
        {
            base.ExposeData();
        }


        
        
    }
}
