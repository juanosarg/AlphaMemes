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
    
    public class RitualOutcomeEffectWorker_AnimaBurial: RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_AnimaBurial() 
        { 
        }
        public RitualOutcomeEffectWorker_AnimaBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {           
            
            
            if (OutcomeChanceWorst(jobRitual, outcome) && outcomeExtension.worstOutcomeDesc != null)
            {                    
                extraOutcomeDesc = outcomeExtension.worstOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                foreach (Pawn pawn2 in jobRitual.Map.mapPawns.AllPawnsSpawned)
                {
                    Pawn_NeedsTracker needs = pawn2.needs;
                    if (needs != null)
                    {
                        Need_Mood mood = needs.mood;
                        if (mood != null)
                        {
                            mood.thoughts.memories.TryGainMemory(InternalDefOf.AM_AnimaScreamLesser, null, jobRitual.Ritual);
                        }
                    }
                }
                DefDatabase<SoundDef>.AllDefsListForReading.First(x => x.defName == "AnimaTreeScream").PlayOneShotOnCamera(jobRitual.Map);
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                Pawn shaman = jobRitual.assignments.AssignedPawns("AM_Shaman").First();
                if(shaman.GetPsylinkLevel()== shaman.GetMaxPsylinkLevel())
                {
                    //Get random spectator if already maxed
                    jobRitual.assignments.SpectatorsForReading.RandomElement().ChangePsylinkLevel(1);
                }
                shaman.ChangePsylinkLevel(1);
            }
            if(outcome.positivityIndex == 2)//Grow some grass
            {
                Thing tree = jobRitual.selectedTarget.Thing;
                CompSpawnSubplant comp = tree.TryGetComp<CompSpawnSubplant>();
                int grass = comp.SubplantsForReading.Count;
                int grassToAdd = 5; //Its supposed to be 5 but its 4.6 due to multiplilier effect that cant be overriden (even though method has ignore multiplier flag) zzz
                if(grass + grassToAdd > 20)
                {
                    grassToAdd = 20 - grass;
                }
                for (int i = 0; i < grassToAdd-1; i++)
                {
                    comp.AddProgress(1f, true);
                }
            }
            
           
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
