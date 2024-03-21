using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
namespace AlphaMemes
{
	public class JobGiver_AnimaBurialLinking : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Thing thing = duty.focusSecond.Thing;


			return JobMaker.MakeJob(InternalDefOf.AM_AnimaBurialLink, duty.focusSecond, duty.focus);
		}


	}	
}
