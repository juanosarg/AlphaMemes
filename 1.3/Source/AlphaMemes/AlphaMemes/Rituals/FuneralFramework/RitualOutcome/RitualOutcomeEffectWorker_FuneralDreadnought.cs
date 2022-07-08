using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    
    public class RitualOutcomeEffectWorker_FuneralDreadnought: RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_FuneralDreadnought() 
        { 
        }
        public RitualOutcomeEffectWorker_FuneralDreadnought(RitualOutcomeEffectDef def) : base(def)
        {
        }
     
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            pawn = corpse.InnerPawn;
            if (OutcomeChanceWorst(jobRitual, outcome))
            {
                //Not going to destory corpse, but destory brain so impossible to try again
                //Maybeee I'll give some resources back, but dunno
                BodyPartRecord brain = corpse.InnerPawn.health.hediffSet.GetBrain();
                corpse.InnerPawn.health.AddHediff(HediffDefOf.MissingBodyPart, brain, null, null);
                Messages.Message("Funeral_DreadnoughtFailure".Translate(pawn.NameFullColored.Named("CORPSE")), jobRitual.selectedTarget, MessageTypeDefOf.NegativeEvent);
                return;
            }
            if(outcome.BestPositiveOutcome(jobRitual))
            {
                ResurrectionUtility.Resurrect(pawn);
            }
            else
            {
                ResurrectionUtility.ResurrectWithSideEffects(pawn);
                if (!outcome.Positive)//5 days to recover if bad
                {
                    var curHediff = pawn.health.hediffSet.hediffs.FirstOrDefault(x => x.def == HediffDefOf.ResurrectionSickness); //If they got a worse outcome of resurrect then that's fine
                    if(curHediff != null)
                    {
                        curHediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 300000;
                    }

                }
            }
            Hediff lifeSupport = HediffMaker.MakeHediff(FuneralFrameWork_StaticStartup.AM_WarCasketLifeSupport, pawn);
            pawn.health.hediffSet.hediffs.Add(lifeSupport);//So they cant be removed from the warcasket
            RitualBehaviorWorker_FuneralFrameworkDreadnought behavior = jobRitual.Ritual.behavior as RitualBehaviorWorker_FuneralFrameworkDreadnought;
            object project = behavior.project;
            AccessTools.Method(project.GetType(), "ApplyOn").Invoke(project, new object[] { pawn }); ;
            behavior.project = null;

        }
/*        //This is maybe a bad idea. But If there's an alternative funeral. It will create the obligation for them now it cant create this one becaue brains gone
        //Could possibly make this smarter by looping just my extensions. But for compatability reasons I wont until there's a clear reason to.
        //It should be fine based on what I can tell. - it wasnt
        //To clarify it actually worked fine but I blow away the obligations I just made with the postfix \o/ I might still do it through other means but really not important
        public void AlternateObligation(Pawn pawn)
        {  
            pawn.ideo.Ideo.Notify_MemberDied(pawn);
        }*/
        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            if (OutcomeChanceWorst(jobRitual, outcome) && outcomeExtension.worstOutcomeDesc != null)
            {
               
                extraOutcomeDesc = outcomeExtension.worstOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"),pawn.Name.Named("SPAWNPAWN"));
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"), pawn.Name.Named("SPAWNPAWN"));
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }
        
        
    }
}
