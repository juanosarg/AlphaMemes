using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    
    public class RitualOutcomeEffectWorker_FuneralFramework: RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_FuneralFramework() 
        { 
        }
        public RitualOutcomeEffectWorker_FuneralFramework(RitualOutcomeEffectDef def) : base(def)
        {
        }
        //Extends the functionality of apply extra outcome
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            //Getting all the data                        
            outcomeExtension = def.GetModExtension<OutcomeEffectExtension>();            
            bool worstOutcome = OutcomeChanceWorst(jobRitual, outcome);
            bool bestOutcome = outcome.BestPositiveOutcome(jobRitual);
            extraOutcomeDesc = null;
            Pawn pawn = null;
            Corpse corpse = jobRitual.assignments.Participants.First(x => x.Dead).Corpse;//Only one corpse
            if (outcomeExtension.roleToSpawnOn != null)
            {
                pawn = jobRitual.PawnWithRole(outcomeExtension.roleToSpawnOn);
            }
            else
            {
                pawn = corpse.InnerPawn;
            }
           
                   
            ExtraOutcomeDesc(pawn, corpse,totalPresence,jobRitual,outcome, ref extraOutcomeDesc, ref letterLookTargets);
            List<Thing> thingsToSpawn = new List<Thing>();
            if (!outcomeExtension.outcomeSpawners.NullOrEmpty())
            {
                foreach (FuneralFramework_ThingToSpawn getThing in outcomeExtension.outcomeSpawners)
                {
                    getThing.DestroyThingsUsed(jobRitual, bestOutcome, worstOutcome);
                    Thing thingToSpawn = getThing?.GetThingToSpawn(jobRitual, bestOutcome, worstOutcome, pawn, corpse);                    
                    if (thingToSpawn != null)
                    {
                        getThing.initComps(thingToSpawn, corpse, jobRitual, bestOutcome, worstOutcome, outcome,this);
                        thingsToSpawn.Add(thingToSpawn);
                    }
                }
            }

            
            ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);

        }
        //Outcome worker doesnt have this and it was annoying me needing for loops always
        public T GetComp<T>() where T : RitualOutcomeComp 
        {
            if(def.comps != null)
            {
                int index = 0;
                int count = def.comps.Count;
                while (index < count)
                {
                    T comp = def.comps[index] as T;
                    if (comp != null)
                    {
                        return comp;
                    }
                    index++;
                }
            }
            return default(T);
        }
        public virtual void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            if (outcomeExtension.stripcorpse == true)
            {
                corpse.Strip();
            }
            IntVec3 cell = pawn.Position;
            if (pawn.Dead)
            {
                cell = pawn.Corpse.Position;
            }
            if (thingsToSpawn.Count > 0)
            {
                foreach (Thing thingToSpawn in thingsToSpawn)
                {
                    GenPlace.TryPlaceThing(thingToSpawn, cell, jobRitual.Map, ThingPlaceMode.Near);
                }                
            }
            if (outcomeExtension.despawnCorpse)
            {
                corpse.DeSpawn();
                return;
            }
            if (outcomeExtension.destroyCorpse)
            {
                corpse.Destroy();
            }
            
           
            

        }

        
        public virtual void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            if (OutcomeChanceWorst(jobRitual, outcome) && outcomeExtension.worstOutcomeDesc != null)
            {
               
                extraOutcomeDesc = outcomeExtension.worstOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"),pawn.Name.Named("SPAWNPAWN"));
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"), pawn.Name.Named("SPAWNPAWN"));
            }
        }

        public bool OutcomeChanceWorst(LordJob_Ritual jobRitual, OutcomeChance outcome) //Doing opposite of Vanilla check if best
        {
            using (List<OutcomeChance>.Enumerator enumerator = jobRitual.Ritual.outcomeEffect.def.outcomeChances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.positivityIndex < outcome.positivityIndex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override void ExposeData()
        {
            base.ExposeData();
        }

        public OutcomeEffectExtension  outcomeExtension;
        
        
    }
}
