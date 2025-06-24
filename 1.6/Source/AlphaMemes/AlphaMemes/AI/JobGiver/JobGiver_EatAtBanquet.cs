
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AlphaMemes
{
    public class JobGiver_EatAtBanquet : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
            if (!GatheringsUtility.TryFindRandomCellAroundTarget(pawn, lordJob_Ritual.selectedTarget.Thing, out IntVec3 result) && !GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, CellValid, out result))
            {
                return null;
            }
            Job job = JobMaker.MakeJob(InternalDefOf.AM_EatAtBanquet, lordJob_Ritual.selectedTarget.Thing, result);
            job.doUntilGatheringEnded = true;
            job.expiryInterval = lordJob_Ritual.DurationTicks;
            return job;
            bool CellValid(IntVec3 c)
            {
                foreach (IntVec3 item in GenRadial.RadialCellsAround(c, 1f, useCenter: true))
                {
                    if (!pawn.CanReserveSittableOrSpot(item))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}