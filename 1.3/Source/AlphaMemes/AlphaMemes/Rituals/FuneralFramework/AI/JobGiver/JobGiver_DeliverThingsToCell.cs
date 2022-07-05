using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI.Group;
using Verse;
using Verse.AI;
namespace AlphaMemes
{
	public class JobGiver_DeliverThingsToCell : JobGiver_DeliverCorpseToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverThingsToCell JobGiver_DeliverThingsToCell = (JobGiver_DeliverThingsToCell)base.DeepCopy(resolve);
			return JobGiver_DeliverThingsToCell;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			//Get things
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focusThird.Thing.InteractionCell;
			Dictionary<ThingDef, int> thingsToGet = new Dictionary<ThingDef, int>();
			List<ThingCount> chosen = new List<ThingCount>();
			List<IngredientCount> thingCounts = new List<IngredientCount>();
			OutcomeEffectExtension extension = lordJob_Ritual.Ritual.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();
			foreach(FuneralFramework_ThingToSpawn spawner in extension.outcomeSpawners)
            {
				foreach(KeyValuePair<ThingDef,int> kv in spawner.thingsRequired)
                {
                    if (thingsToGet.ContainsKey(kv.Key))
                    {
						thingsToGet[kv.Key] += kv.Value;
                    }
                    else
                    {
						thingsToGet.Add(kv.Key, kv.Value);
                    }
				}
            }
			foreach (var data in thingsToGet) thingCounts.Add(new ThingDefCountClass(data.Key, data.Value).ToIngredientCount());
			
			if (!FuneralHaulUtility.TryFindBestBillIngredients(thingCounts, pawn, pawn.mindState.duty.focusThird.Thing, chosen))
			{
				return null;
			}
			else if (chosen.Any(x => !pawn.CanReserveAndReach(x.Thing, PathEndMode.ClosestTouch, Danger.Deadly)))
				return null;
			else
			{ 
				Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverThingsToCell);
				job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
				job.expiryInterval = jobMaxDuration;
				job.targetA = pawn.mindState.duty.focusThird.Thing;
				job.targetQueueB = new List<LocalTargetInfo>(chosen.Count);
				job.targetC = pawn.mindState.duty.focusThird.Thing.Position;
				job.countQueue = new List<int>(chosen.Count);
				for(int i = 0; i < chosen.Count; i++)
                {
					job.targetQueueB.Add(chosen[i].Thing);
					job.countQueue.Add(chosen[i].Count);
				}
				job.ritualTag = "Arrived";
				return job;
			}
		}


	}	
}
