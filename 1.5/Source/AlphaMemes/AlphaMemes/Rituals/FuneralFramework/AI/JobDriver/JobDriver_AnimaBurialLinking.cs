
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using Verse.Sound;
namespace AlphaMemes
{
    public class JobDriver_AnimaBurialLinking : JobDriver_LinkPsylinkable
	{
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
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
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				yield break;
			}			
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(3750, TargetIndex.None);
			toil.tickAction = delegate ()
			{
				this.pawn.rotationTracker.FaceTarget(this.PsylinkableThing);
				if (Find.TickManager.TicksGame % 720 == 0)
				{
					Vector3 vector = this.pawn.TrueCenter();
					vector += (this.PsylinkableThing.TrueCenter() - vector) * Rand.Value;
					FleckMaker.Static(vector, this.pawn.Map, FleckDefOf.PsycastAreaEffect, 0.5f);
					this.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.PsylinkableThing), MaintenanceType.None));
				}
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}
    }
}
