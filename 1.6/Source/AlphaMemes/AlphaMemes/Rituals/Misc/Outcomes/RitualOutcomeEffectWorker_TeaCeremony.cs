using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;
using System.Security.Principal;


namespace AlphaMemes
{
    public class RitualOutcomeEffectWorker_TeaCeremony : RitualOutcomeEffectWorker_FromQuality
    {



        public RitualOutcomeEffectWorker_TeaCeremony()
        {
        }

        public RitualOutcomeEffectWorker_TeaCeremony(RitualOutcomeEffectDef def) : base(def)
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
            ThingDef teaToMake=null;
            int numberToMake=1;
            switch(outcome.positivityIndex)

                    {

                case -1:
                    teaToMake = InternalDefOf.VBE_Tea;
                    numberToMake = 5;
                    break;
                case 1:
                    teaToMake = InternalDefOf.VBE_Tea;
                    numberToMake = 20;

                    break;
                case 2:
                    if(StaticCollections.specialtyTeas.Count > 0)
                    {
                        teaToMake = StaticCollections.specialtyTeas.RandomElement();
                        numberToMake = 25;
                    }
                    else
                    {
                        teaToMake = InternalDefOf.VBE_Tea;
                        numberToMake = 25;

                    }

                    break;
                }
            if(teaToMake != null)
            {
                Thing thing = ThingMaker.MakeThing(teaToMake);
                thing.stackCount = numberToMake;
                GenPlace.TryPlaceThing(thing, pawn2.Position, pawn2.Map, ThingPlaceMode.Near);

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
