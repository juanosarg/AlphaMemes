
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class JobDriver_VisitCorpseContainer : JobDriver_VisitJoyThing
	{
		protected Building Container
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

        protected override void WaitTickAction()
        {
			float extraJoyGainFactor = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				extraJoyGainFactor *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, Container);
		}

		public override object[] TaleParameters()
		{
			Comp_CorpseContainer comp = Container.TryGetComp<Comp_CorpseContainer>();
			Pawn containerPawn = comp.VisitedPawn;
			return new object[]
			{
				this.pawn,				
				(containerPawn != null) ? containerPawn : null
			};
		}


	}
}
