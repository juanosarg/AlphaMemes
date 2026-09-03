using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Networking.UnityWebRequest;
using Verse.AI;


namespace AlphaMemes
{
    public class RitualOutcomeEffectWorker_Flagellation : RitualOutcomeEffectWorker_FromQuality
    {

        public RitualOutcomeEffectWorker_Flagellation()
        {
        }

        public RitualOutcomeEffectWorker_Flagellation(RitualOutcomeEffectDef def) : base(def)
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
          
            Pawn pawn3 = jobRitual.PawnWithRole("flagellant");
            if (outcome.positivityIndex==-1)
            {
                DoScarifications(1, pawn3);
            }else if (outcome.positivityIndex == 1)
            {
                DoScarifications(3, pawn3);
            }
            else if (outcome.positivityIndex == 2)
            {
                DoScarifications(5, pawn3);
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


        public void DoScarifications(int amount,Pawn target)
        {
            for (int j = 0; j < amount; j++)
            {
                if (!(from part in JobDriver_Scarify.GetPartsToApplyOn(target)
                      where JobDriver_Scarify.AvailableOnNow(target, part)
                      select part).TryRandomElement(out BodyPartRecord result) && !JobDriver_Scarify.GetPartsToApplyOn(target).TryRandomElement(out result))
                { 
                    Log.Error("Failed to find body part to scarify");
                }
                JobDriver_Scarify.Scarify(target, result);
                JobDriver_Scarify.CreateHistoryEventDef(target);
            }
            SoundDefOf.Execute_Cut.PlayOneShot(target);
            if (target.RaceProps.BloodDef != null)
            {
                CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
                for (int i = 0; i < 3; i++)
                {
                    IntVec3 randomCell = cellRect.RandomCell;
                    if (randomCell.InBounds(target.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, target.Map))
                    {
                        FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, target.LabelIndefinite());
                    }
                }
            }

        }
    }
}
