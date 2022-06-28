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
	public class RitualStageAction_ThingEffects : RitualStageAction
	{

		public override void Apply(LordJob_Ritual ritual)
		{
			//Start Sound
			behavior = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;

			selectedtarget = ritual.selectedTarget;
			string target;
			//Start Effect
			IntVec3 cell;
			foreach (KeyValuePair<string,ThingDef> kv in thingsToSpawnOn)
            {
				//Parse out p_ = pawn role, t_target = ritual target, r_# random spot within # cells of ritual target
				target = kv.Key.Substring(2);
				cell = IntVec3.Invalid;
				switch (kv.Key.Substring(0, 2))
				{
					case "p_":
                        if(ritual.assignments.AnyPawnAssigned(target))
						{//Make sure its valid
							foreach( Pawn pawn in ritual.assignments.AssignedPawns(target))
                            {
								cell = pawn.Dead ? pawn.Corpse.Position : pawn.Position;
								SpawnThing(ritual, cell, kv.Value);

							}
                        }
						break;
					case "t_":
						SpawnThing(ritual, selectedtarget.Cell, kv.Value);
						break;

					case "r_":
						int distance;
						
						bool isParsed = int.TryParse(target,out distance);
                        if (isParsed)
                        {
							if(CellFinder.TryFindRandomCellNear(selectedtarget.Cell, ritual.Map, distance, (IntVec3 c) => ValidSpotForThing(c), out cell)){
								SpawnThing(ritual,cell, kv.Value);
                            }

						}
						break;


				}


			}

		}
		public virtual void SpawnThing(LordJob_Ritual ritual, IntVec3 cell, ThingDef thingDef)
		{

			Thing thing = GenSpawn.Spawn(thingDef, cell, ritual.Map);
			CompProperties_DestroyAfterDelay props = thing.TryGetComp<CompDestroyAfterDelay>()?.props as CompProperties_DestroyAfterDelay;
			if(props != null)
            {
				props.delayTicks = ritual.TicksLeft;
			}			
			behavior.spawnEffectThings.Add(thing);

		}
		public static bool ValidSpotForThing(IntVec3 cell)
        {

            if (cell.Filled(Find.CurrentMap))
            {
				return false;
			}
			return true;
        }

		public override void ExposeData()
		{

		}
		private TargetInfo selectedtarget;
		private RitualBehaviorWorker_FuneralFramework behavior;
		public Dictionary<string,ThingDef> thingsToSpawnOn;


	}
}

