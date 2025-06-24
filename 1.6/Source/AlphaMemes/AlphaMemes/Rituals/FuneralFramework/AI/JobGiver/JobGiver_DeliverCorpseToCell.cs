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
			Corpse pawn2 = pawn.mindState.duty.focusSecond.Pawn?.Corpse ?? null;
			if(pawn2 == null) { return null; }
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
			{
				return null;
			}
			if(pawn2.Position == pawn.mindState.duty.focusThird.Cell && pawn.Position.AdjacentTo8Way(pawn.mindState.duty.focusThird.Thing.InteractionCell))
            {
				return null;
            }
			Lord lord = pawn.GetLord();//adding this cause somehow its still job spamming ughhh
			LordJob_Ritual ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (ritual.PawnTagSet(pawn, "Arrived"))
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focusThird.Thing.Position;
			//Getting custom position is fun
			//Im hoping the index of lordjob always matches the def. it should, unless expose data someone orders them wrong but that seems impossible cause everything would break
			int stage = ritual.StageIndex;
			RitualBehaviorWorker_FuneralFramework behavior = ritual.Ritual.behavior as RitualBehaviorWorker_FuneralFramework;
			string roleId = ritual.assignments.RoleForPawn(pawn).id;
			RitualPosition_CorpsePosition corpsePosition = behavior.def.stages[stage]?.BehaviorForRole(roleId)?.CustomPositionsForReading?.FirstOrDefault() as RitualPosition_CorpsePosition;
			if (corpsePosition != null)
            {
				cell = corpsePosition.CorpseCell(pawn, ritual, null);
            }
            else
            {
				if (!pawn.CanReach(cell, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
				{
					cell = pawn.mindState.duty.focusThird.Thing.InteractionCell;
				}
			}


			Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverCorpseToCell, pawn2, cell, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}


	}	
}
