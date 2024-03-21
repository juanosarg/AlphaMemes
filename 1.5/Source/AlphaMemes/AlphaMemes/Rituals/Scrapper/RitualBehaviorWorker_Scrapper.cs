using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.Sound;

namespace AlphaMemes
{
    public class RitualBehaviorWorker_Scrapper : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_Scrapper()
        {
        }
        public RitualBehaviorWorker_Scrapper(RitualBehaviorDef def) : base(def)
        {
        }

        public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
        {
            var targetWorker = ritual.obligationTargetFilter as RitualObligationTargetWorker_HighTechThing;
            TargetInfo updatedTarget = targetWorker.filter.BestTarget(target, target);
            toScrap = target.Thing;
            base.TryExecuteOn(updatedTarget, organizer, ritual, obligation, assignments, playerForced);
        }
        public override Sustainer SoundPlaying
        {
            get
            {
                return this.soundPlaying;
            }
        }
        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);

            if (soundPlaying != null)
            {
                soundPlaying.Maintain();
            }
            if (effecter != null)
            {
                effecter.EffectTick(effecterpawn, ritual.selectedTarget);
            }
        }
        public override void Cleanup(LordJob_Ritual ritual)
        {
            base.Cleanup(ritual);
            toScrap = null;
            if (this.soundPlaying != null)
            {
                soundPlaying.End();
                soundPlaying = null;
            }
            if (effecter != null)
            {
                effecter.Cleanup();
                effecter = null;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref toScrap, "toScrap");
        }
        public Sustainer soundPlaying; //Cant override set        
        public Effecter effecter;
        public Pawn effecterpawn;
        public Thing toScrap;
    }
}
