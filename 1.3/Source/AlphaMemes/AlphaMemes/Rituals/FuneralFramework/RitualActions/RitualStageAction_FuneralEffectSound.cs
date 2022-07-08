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
	//Action to add a sound and effect to the behavior
	public class RitualStageAction_FuneralEffectSound : RitualStageAction
	{

		public override void Apply(LordJob_Ritual ritual)
		{
			//Start Sound
			this.behavior = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;
			Sustainer sustainer = behavior.SoundPlaying;
			selectedtarget = ritual.selectedTarget;
			if (sustainer == null)
			{
				//Create the sound and start playing it
				sustainer = NewSustainer(selectedtarget);
				behavior.soundPlaying = sustainer;
			}
			//Start Effect
			
			foreach (Pawn p in ritual.assignments.AssignedPawns(roleID))
            {
				ApplyToPawn(ritual, p);
				
            }

		}
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{

			Effecter effecter = behavior.effecter;
			TargetInfo cell = new TargetInfo(pawn.Dead ? pawn.Corpse.Position : pawn.Position, ritual.Map);

			if (effecter == null)
			{
				effecter = effect.Spawn();
                if (targetRitual)
                {
					effecter.Trigger(cell, selectedtarget);
				}
                else
                {
					effecter.Trigger(cell, cell);
				}
				
				behavior.effecterpawn = pawn;
				behavior.effecter = effecter;
			}

		}
		private Sustainer NewSustainer(TargetInfo selectedtarget)
		{
			return this.sound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedtarget.Cell, selectedtarget.Map, false), MaintenanceType.PerTick));

		}

		public override void ExposeData()
		{
			
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
			Scribe_Defs.Look<EffecterDef>(ref this.effect, "effect");
			Scribe_Values.Look(ref roleID, "roleID");
			Scribe_Values.Look(ref targetRitual, "targetRitual");
		}
		private TargetInfo selectedtarget;
		private RitualBehaviorWorker_FuneralFramework behavior;
		public SoundDef sound;
		public bool targetRitual = true;
		public string roleID;

		public EffecterDef effect;

	}
}

