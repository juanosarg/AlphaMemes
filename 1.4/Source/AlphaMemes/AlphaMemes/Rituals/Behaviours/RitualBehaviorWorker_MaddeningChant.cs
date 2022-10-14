using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;

namespace AlphaMemes
{
	public class RitualBehaviorWorker_MaddeningChant : RitualBehaviorWorker
	{
		private Sustainer soundPlaying;
		public RitualBehaviorWorker_MaddeningChant()
		{
		}

		public RitualBehaviorWorker_MaddeningChant(RitualBehaviorDef def) : base(def)
		{
		}



		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 0)
			{
				if (this.soundPlaying == null || this.soundPlaying.Ended)
				{
					TargetInfo selectedTarget = ritual.selectedTarget;
					this.soundPlaying = InternalDefOf.AM_RitualSustainer_MaddeningChant.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
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
