using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;


namespace AlphaMemes
{
    public class RitualOutcomeEffectWorker_Baptism : RitualOutcomeEffectWorker_FromQuality
    {



        public RitualOutcomeEffectWorker_Baptism()
        {
        }

        public RitualOutcomeEffectWorker_Baptism(RitualOutcomeEffectDef def) : base(def)
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
            RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
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
            Pawn pawn2 = jobRitual.PawnWithRole("moralist");
            Pawn pawn3 = jobRitual.PawnWithRole("baptized");
            if (outcome.Positive)
            {
                pawn3.ideo.SetIdeo(pawn2.Ideo);
            }
            else
            {
                float ideoCertaintyOffset = outcome.ideoCertaintyOffset;
                pawn3.ideo.OffsetCertainty(ideoCertaintyOffset);
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
