using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;

namespace AlphaMemes
{
    public class RitualBehaviorWorker_TeaCeremony : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;
        public RitualBehaviorWorker_TeaCeremony()
        {
        }

        public RitualBehaviorWorker_TeaCeremony(RitualBehaviorDef def) : base(def)
        {
        }

        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);
            if (ritual.StageIndex == 1)
            {
                if (this.soundPlaying == null || this.soundPlaying.Ended)
                {
                    TargetInfo selectedTarget = ritual.selectedTarget;
                    this.soundPlaying = InternalDefOf.AM_RitualSustainer_TeaCeremony.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
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
