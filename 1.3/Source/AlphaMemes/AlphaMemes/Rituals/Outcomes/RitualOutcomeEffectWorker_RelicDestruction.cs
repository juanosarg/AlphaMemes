using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;


namespace AlphaMemes
{
	public class RitualOutcomeEffectWorker_RelicDestruction : RitualOutcomeEffectWorker_FromQuality
	{



		public RitualOutcomeEffectWorker_RelicDestruction()
		{
		}

		public RitualOutcomeEffectWorker_RelicDestruction(RitualOutcomeEffectDef def) : base(def)
		{
		}

		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{

			float quality = base.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			bool flag = false;
			foreach (Pawn pawn in totalPresence.Keys)
			{

				base.GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
				
			}
            if (StaticCollectionsClass.relicsDestroyedThisGame < 3)
            {
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.InRandomOrder())
				{
					if (pawn.Ideo?.HasMeme(InternalDefOf.AM_Iconoclast) == true)
					{
						Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.AM_IconoclastHediff);
						if(hediff == null)
                        {
							pawn.health.AddHediff(InternalDefOf.AM_IconoclastHediff);

                        }
                        else
                        {
							hediff.Severity += 0.3f;

                        }

					}
				}
			}
			

			Building altar = jobRitual.selectedTarget.Thing as Building;
			CompRelicSmashingContainer comp = altar.TryGetComp<CompRelicSmashingContainer>();
			if (comp != null)
            {
				StaticCollectionsClass.relicsDestroyedThisGame += 1;
				Current.Game.GetComponent<GameComponent_ReligiousMemesAndRelics>().relicsDestroyedThisGame += 1;
				Thing relic = comp.ContainedThing;
				comp.innerContainer.TryDropAll(altar.Position, altar.Map, ThingPlaceMode.Near);
				relic.Destroy();
			}






			string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text3.NullOrEmpty())
			{
				text2 = text2 + "\n\n" + text3;
			}
			if (flag)
			{
				text2 += "\n\n" + "RitualOutcomeExtraDesc_Execution".Translate();
			}
			if (text != null)
			{
				text2 = text2 + "\n\n" + text;
			}
			string text4;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
			if (text4 != null)
			{
				text2 = text2 + "\n\n" + text4;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);




		}


	}
}
