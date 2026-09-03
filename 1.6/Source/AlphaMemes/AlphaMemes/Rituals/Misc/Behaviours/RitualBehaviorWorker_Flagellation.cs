using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace AlphaMemes
{
    public class RitualBehaviorWorker_Flagellation : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;

     

        public RitualBehaviorWorker_Flagellation()
        {
        }

        public RitualBehaviorWorker_Flagellation(RitualBehaviorDef def)
            : base(def)
        {
        }

        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);
            if (ritual.StageIndex == 2)
            {
                if (this.soundPlaying == null || this.soundPlaying.Ended)
                {
                    TargetInfo selectedTarget = ritual.selectedTarget;
                    this.soundPlaying = InternalDefOf.AM_RitualSustainer_Flagellation.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
                }
                Sustainer sustainer = this.soundPlaying;
                if (sustainer == null)
                {
                    return;
                }
                sustainer.Maintain();
            }
        }
    }
}