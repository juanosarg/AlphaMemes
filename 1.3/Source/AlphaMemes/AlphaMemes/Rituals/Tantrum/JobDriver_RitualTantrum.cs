
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class JobDriver_RitualTantrum : JobDriver
	{
		protected Thing Thing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
			return base.Map.reservationManager.Reserve(pawn, job, Thing, 1, -1, null, true);
        }
        protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return toil;
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate ()
			{
				if (pawn.meleeVerbs.TryMeleeAttack(Thing, job.verbToUse, true))
                {
					if (Thing.DestroyedOrNull())
                    {
						Lord lord = pawn.GetLord();
						if (((lord != null) ? lord.LordJob : null) != null && (lord.LordJob is LordJob_Ritual ritual))
						{
							ritual.usedThings.Add(Thing);
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
                }
			}).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield break;
		}
        public override bool IsContinuation(Job j)
        {
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A) || !Thing.Destroyed; //So when explody thing forces job override it doesnt decide I to do a different target
		}


	}
	
}
