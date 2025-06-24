using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
	public class JobGiver_AnimaBurialLinking : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			var ritual = pawn.GetLord()?.LordJob as LordJob_Ritual_FuneralFramework;
			
			if (duty == null || ritual == null)
			{
				return null;
			}
			var position = pawn.Position;
            if (!pawn.CanReserve(pawn.Position, 1, -1, null, false))
            {
                CellFinder.TryRandomClosewalkCellNear(position, pawn.Map, 2, out position, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false));
            }

            return JobMaker.MakeJob(InternalDefOf.AM_AnimaBurialLink, ritual.selectedTarget.Thing, position);
        }


	}	
}
