using RimWorld;
using System;

using Verse;
using Verse.AI;
namespace AlphaMemes
{
	public class JobGiver_OcularBurial : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}


			return JobMaker.MakeJob(FuneralFrameWork_StaticStartup.AM_OcularBurial, duty.focusThird, duty.focus);
		}


	}	
}
