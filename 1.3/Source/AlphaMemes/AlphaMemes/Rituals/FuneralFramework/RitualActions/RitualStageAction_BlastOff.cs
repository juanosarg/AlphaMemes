using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.AI.Group;
using RimWorld;

namespace AlphaMemes
{
	//Action to launch the pod
	public class RitualStageAction_BlastOff : RitualStageAction
	{

		public override void Apply(LordJob_Ritual ritual)
		{

			Thing transporter = ritual.selectedTarget.Thing;
			CompLaunchable compLaunchable = transporter.TryGetComp<CompLaunchable>();
			CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
			IntVec3 cell = transporter.Position;

			float fuel = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist(compLaunchable.MaxLaunchDistance), 1f);
			compTransporter.Launchable.FuelingPortSource?.TryGetComp<CompRefuelable>().ConsumeFuel(fuel);

			ActiveDropPod innerthing = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			innerthing.Contents = new ActiveDropPodInfo();
			innerthing.Contents.innerContainer.TryAddRangeOrTransfer(compTransporter.GetDirectlyHeldThings());			

			FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(compLaunchable.Props.skyfallerLeaving, innerthing);
			flyShipLeaving.createWorldObject = false;
			transporter.Destroy();
			GenSpawn.Spawn(flyShipLeaving, cell, ritual.Map, WipeMode.Vanish);




		}
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
		}

		public override void ExposeData()
		{

		}



	}
}

