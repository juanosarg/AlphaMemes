using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using HarmonyLib;
using Verse.Sound;
using Verse.AI.Group;
using RimWorld;

namespace AlphaMemes
{

	public class RitualStageAction_Dreadnought: RitualStageAction_StripCorpse
	{
		
		public override void Apply(LordJob_Ritual ritual)
		{
			var funeral = ritual as LordJob_Ritual_FuneralFramework;

			Messages.Message(this.text.Formatted(ritual.Ritual.Label).CapitalizeFirst(), ritual.selectedTarget, this.messageTypeDef, false);
			base.Apply(ritual);
			Pawn pawn = funeral.corpse;
			Thing thing = ritual.selectedTarget.Thing;

			var behavior = ritual.Ritual.behavior as RitualBehaviorWorker_FuneralFrameworkDreadnought;
			behavior.foundry = thing;
			AccessTools.Field(thing.GetType(), "occupant").SetValue(thing, pawn);
			AccessTools.Field(thing.GetType(), "curWarcasketProject").SetValue(thing, behavior.project);
			funeral.Map.dynamicDrawManager.DeRegisterDrawable(pawn.Corpse);//This is to hide the corpse so I can draw it in the right spot
			behavior.deregisteredCorpse = true;
		}

		public override void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageTypeDef, "messageTypeDef");
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
		}


		[MustTranslate]
		public string text;



		// Token: 0x040037CD RID: 14285
		public MessageTypeDef messageTypeDef;
	}
}

