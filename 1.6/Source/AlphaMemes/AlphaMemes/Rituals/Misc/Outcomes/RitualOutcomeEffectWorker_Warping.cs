using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class RitualOutcomeEffectWorker_Warping : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_Warping()
        {
        }

        public override bool SupportsAttachableOutcomeEffect
        {
            get
            {
                return false;
            }
        }
        public RitualOutcomeEffectWorker_Warping(RitualOutcomeEffectDef def) : base(def)
        {
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

          
            Pawn sacrifice = jobRitual.PawnWithRole("animal");         
            float size = sacrifice.RaceProps.baseBodySize;
            PawnGenerationRequest request = new PawnGenerationRequest();
            string str = outcome.label + " " + jobRitual.Ritual.Label;
            TaggedString taggedString = outcome.description.Formatted(jobRitual.Ritual.Label);
            PawnKindDef chosenPawn = InternalDefOf.AA_Eyeling;
            switch (outcome.positivityIndex)
            {

                case -2:
                    if (size > 1.5) { 
                        if(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedGoo") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedGoo");
                        }
                    }
                    break;
                case -1:
                    if (size < 0.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedGoo") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedGoo");
                        }
                    }
                    else if (size < 1.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularJelly") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularJelly");
                        }
                    }
                    else
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularNightling") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularNightling");
                        }
                    }
                    break;
                case 1:
                    if (size < 0.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedSpore") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_RedSpore");
                        }
                    }
                    else if (size < 1.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularNightling") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularNightling");
                        }
                    }
                    else
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_EngorgedTentacularAberration") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_EngorgedTentacularAberration");
                        }
                    }
                    break;
                case 2:
                    if (size < 0.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularJelly") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_OcularJelly");
                        }
                    }
                    else if (size < 1.5)
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_EngorgedTentacularAberration") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_EngorgedTentacularAberration");
                        }
                    }
                    else
                    {
                        if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_UnblinkingEye") != null)
                        {
                            chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_UnblinkingEye");
                        }
                    }
                    break;
                case 3:

                    if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_UnblinkingEye") != null)
                    {
                        chosenPawn = DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_UnblinkingEye");
                    }

                    break;
               

            }
            request = new PawnGenerationRequest(chosenPawn, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, 1f, false, true, true, false, false);

            Pawn pawn = PawnGenerator.GeneratePawn(request);
            GenSpawn.Spawn(pawn, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
            ChoiceLetter let = LetterMaker.MakeLetter(str, taggedString, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null);
            Find.LetterStack.ReceiveLetter(let, null);




        }



    }
}
