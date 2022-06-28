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
	public class JobGiver_LoadCorpseToThing : JobGiver_DeliverPawnToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_LoadCorpseToThing JobGiver_LoadCorpseToThing = (JobGiver_LoadCorpseToThing)base.DeepCopy(resolve);	
			return JobGiver_LoadCorpseToThing;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Corpse pawn2 = pawn.mindState.duty.focusSecond.Pawn.Corpse;
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				return null;
			}


			//Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, pawn2, pawn.mindState.duty.focusThird); Just once i'd like some vanilla to work for me
			Job job = JobMaker.MakeJob(InternalDefOf.AM_LoadCorpseToThing, pawn2, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.haulMode = HaulMode.ToContainer;
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}


	}	
}
