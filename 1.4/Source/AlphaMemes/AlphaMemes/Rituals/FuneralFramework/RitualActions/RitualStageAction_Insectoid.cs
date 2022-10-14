using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Verse.AI.Group;
using RimWorld;

namespace AlphaMemes
{
	//Didnt make thie generic at all but its making filth and playing oneshot
	public class RitualStageAction_Insectoid : RitualStageAction
	{

		public override void Apply(LordJob_Ritual ritual)
		{
			var behavior = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;
			SoundDefOf.Hive_Spawn.PlayOneShot(SoundInfo.InMap(ritual.selectedTarget));
			FilthMaker.TryMakeFilth(ritual.selectedTarget.Cell, ritual.Map, ThingDefOf.Filth_Slime,3);
			ritual.Map.dynamicDrawManager.DeRegisterDrawable(behavior.corpse.Corpse);
			behavior.deregisteredCorpse = true;
		}

		public override void ExposeData()
		{
	
		}


	}
}

