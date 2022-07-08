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
	public class JobGiver_DeliverStuffToCell : JobGiver_DeliverCorpseToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverStuffToCell JobGiver_DeliverStuffToCell = (JobGiver_DeliverStuffToCell)base.DeepCopy(resolve);
			JobGiver_DeliverStuffToCell.def = this.def;
			JobGiver_DeliverStuffToCell.count = this.count;
			return JobGiver_DeliverStuffToCell;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			//Get things
			LordJob_Ritual ritual = Find.IdeoManager.GetActiveRituals(pawn.Map).First();
			RitualBehaviorWorker_FuneralFramework behavior = ritual.Ritual.behavior as RitualBehaviorWorker_FuneralFramework;
            if (behavior == null)
            {
				return null;
            }
			def = behavior.stuffToUse;
			count = behavior.stuffCount;

			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
            if (lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
            {
				return null;
            }
			IntVec3 cell = pawn.mindState.duty.focusThird.Thing.InteractionCell;
			int toTake = Math.Max(count - pawn.inventory.Count(def), 0);
			if (toTake == 0)
			{
				return null;
			}

			Thing thingToGet = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(this.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
			if (!pawn.CanReach(thingToGet, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
			{
				return null;
			}

			Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverStuffToCell, thingToGet, cell);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = count;
			job.ritualTag = "Arrived";
			return job;
		}

		public ThingDef def;
		public int count = 0;
	}	
}
