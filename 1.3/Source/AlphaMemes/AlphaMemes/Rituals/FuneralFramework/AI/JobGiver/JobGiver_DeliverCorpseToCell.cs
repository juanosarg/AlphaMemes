using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
	public class JobGiver_DeliverCorpseToCell : JobGiver_DeliverPawnToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverCorpseToCell JobGiver_DeliverCorpseToCell = (JobGiver_DeliverCorpseToCell)base.DeepCopy(resolve);	
			return JobGiver_DeliverCorpseToCell;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Corpse pawn2 = pawn.mindState.duty.focusSecond.Pawn.Corpse;
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
			{
				return null;
			}
			if(pawn2.Position == pawn.mindState.duty.focusThird.Cell && pawn.Position.AdjacentTo8Way(pawn.mindState.duty.focusThird.Thing.InteractionCell))
            {
				return null;
            }
			Lord lord = pawn.GetLord();//adding this cause somehow its still job spamming ughhh
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focusThird.Thing.Position;

			Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverCorpseToCell, pawn2, cell, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}


	}	
}
