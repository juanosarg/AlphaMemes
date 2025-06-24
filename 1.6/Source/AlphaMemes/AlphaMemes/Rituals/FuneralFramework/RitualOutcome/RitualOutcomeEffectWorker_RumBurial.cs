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
    
    public class RitualOutcomeEffectWorker_RumBurial : RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_RumBurial() 
        { 
        }
        public RitualOutcomeEffectWorker_RumBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {           
           
        }
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome)
        {
            var rumVat = jobRitual.selectedTarget.Thing as Building_RumVat;
            rumVat.Notify_CorpseBuried();
            rumVat.isFuneral = true;
            base.ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
        }


        public override void ExposeData()
        {
            base.ExposeData();
        }


        
        
    }
}
