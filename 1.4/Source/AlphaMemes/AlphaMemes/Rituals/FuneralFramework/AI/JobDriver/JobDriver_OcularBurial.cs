
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using Verse.Sound;

namespace AlphaMemes
{
    public class JobDriver_OcularBurial : JobDriver
    {
		private Thing Corpse
		{
			get
			{
				return base.TargetA.Thing;
			}
		}



		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(LinkSpot, job, 1, -1, null, errorOnFailed);
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(3750, TargetIndex.None);
			toil.tickAction = delegate ()
			{
				this.pawn.rotationTracker.FaceTarget(this.Corpse);
				if (Find.TickManager.TicksGame % 240 == 0)
				{
					var pos = pawn.Position.RandomAdjacentCell8Way();
                    Thing thing = ThingMaker.MakeThing(ThingDef.Named("AA_RedGas"), null);
					GenSpawn.Spawn(thing, pos, Map);                   
                }
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}
    }
}
