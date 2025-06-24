
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class JobDriver_DeliverCorpseToCell : JobDriver
	{
		protected Corpse Takee
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
			base.Map.reservationManager.ReleaseAllForTarget(Takee);
			return base.Map.reservationManager.Reserve(pawn, job, Takee, 1, -1, null, true);
			
        }
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);			
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Jump.JumpIf(gotoCell, () => this.pawn.IsCarryingThing(this.Takee));
			yield return startCarrying;
			yield return gotoCell;
			yield return Toils_General.Do(delegate
			{
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					
					lordJob_Ritual = (((lord != null) ? lord.LordJob : null) as LordJob_Ritual);
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
				}
			});
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield break;
		}



		private const TargetIndex TakeeIndex = TargetIndex.A;

		
		private const TargetIndex TargetCellIndex = TargetIndex.B;
	}
}
