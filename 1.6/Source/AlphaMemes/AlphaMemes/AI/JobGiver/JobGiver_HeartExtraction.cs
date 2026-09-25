
using RimWorld;
using Verse;
using Verse.AI;
namespace AlphaMemes
{
    public class JobGiver_HeartExtraction : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
            if (!pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None))
            {
                return null;
            }
            return JobMaker.MakeJob(InternalDefOf.AM_HeartExtraction, pawn2, pawn.mindState.duty.focus);
        }
    }
}