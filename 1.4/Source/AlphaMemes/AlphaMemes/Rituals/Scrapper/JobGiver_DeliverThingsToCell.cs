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
	//Some day I might have this generic and not specific to corpes
	public class JobGiver_ScrapperDeliverThing : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{

			//Get things
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				return null;
			}
			var behavior = lordJob_Ritual.Ritual.behavior as RitualBehaviorWorker_Scrapper;
			var toTake = behavior.toScrap;
			if (!pawn.CanReserveAndReach(toTake, PathEndMode.Touch, Danger.Deadly))
            {
				lordJob_Ritual.AddTagForPawn(pawn, "Fail");
				return null;
			}
			Job job = JobMaker.MakeJob(InternalDefOf.AM_DeliverStuffToCell, toTake, pawn.mindState.duty.focus);
			job.locomotionUrgency = LocomotionUrgency.Jog;
			job.expiryInterval = 1000;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;

		}


	}	
}
