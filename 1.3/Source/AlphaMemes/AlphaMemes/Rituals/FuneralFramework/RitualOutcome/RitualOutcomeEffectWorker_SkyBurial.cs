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
    
    public class RitualOutcomeEffectWorker_SkyBurial : RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_SkyBurial() 
        { 
        }
        public RitualOutcomeEffectWorker_SkyBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {           
            //Best outcome makes some animals wander in
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                //Took this from Wild Animal Spawner
                TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false).WithFenceblocked(true);
                if(RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 result,jobRitual.Map,CellFinder.EdgeRoadChance_Animal,true, (IntVec3 cell) => jobRitual.Map.reachability.CanReachMapEdge(cell, traverseParms)))
                {
                    jobRitual.Map.wildAnimalSpawner.SpawnRandomWildAnimalAt(result);
                    Messages.Message("Funeral_SkyburialAnimalsWanderIn".Translate(), new LookTargets(result, jobRitual.Map), MessageTypeDefOf.PositiveEvent);
                    //I'm going to chuckle when this ends up being a bad result because it picked some nasty predator XD
                }
                
            }         
           
        }
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            var skyBurialSpot = jobRitual.selectedTarget.Thing as Building_SkyBurialSpot;
            skyBurialSpot.Notify_CorpseBuried();
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
