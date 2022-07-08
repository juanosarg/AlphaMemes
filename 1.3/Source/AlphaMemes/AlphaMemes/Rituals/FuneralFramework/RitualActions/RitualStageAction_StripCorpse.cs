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

	public class RitualStageAction_StripCorpse : RitualStageAction
	{

		public override void Apply(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.assignments.Participants.First(x => x.Dead);
			ApplyToPawn(ritual,pawn);


		}
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			pawn.Corpse.Strip();
		}

		public override void ExposeData()
		{

		}



	}
}

