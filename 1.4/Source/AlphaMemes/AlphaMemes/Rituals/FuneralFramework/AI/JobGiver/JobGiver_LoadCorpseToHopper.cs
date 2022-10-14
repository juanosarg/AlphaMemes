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
	public class JobGiver_LoadCorpseToHopper : JobGiver_DeliverPawnToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_LoadCorpseToHopper JobGiver_LoadCorpseToHopper = (JobGiver_LoadCorpseToHopper)base.DeepCopy(resolve);	
			return JobGiver_LoadCorpseToHopper;
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
			Building_NutrientPasteDispenser thing = pawn.mindState.duty.focusThird.Thing as Building_NutrientPasteDispenser;
			Building hopper = thing.AdjacentReachableHopper(pawn);
			if(hopper == null)
            {
				return null;
            }

			//Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, pawn2, pawn.mindState.duty.focusThird); Just once i'd like some vanilla to work for me
			Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverCorpseToCell, pawn2, hopper);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}


	}	
}
