using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;
using Verse.AI;

using Verse.AI.Group;
using RimWorld;


namespace AlphaMemes
{
    public class RitualBehaviorWorker_FuneralFramework : RitualBehaviorWorker
    {
        //Cache stuff
        public int checkTick;
        public string lastResult;
        public Pawn corpse;


        //Effect stuff
        public bool deregisteredCorpse = false;
        public List<Thing> spawnEffectThings = new List<Thing>();//things spawned that need cleanup just in case
        public Sustainer soundPlaying; //Cant override set        
        public Effecter effecter;
        public Pawn effecterpawn;

        //Spawn stuff
        public ThingDef stuffToUse;
        public int stuffCount;
        public OutcomeEffectExtension outcomeExt;
        public FuneralPreceptExtension extension;

        public RitualBehaviorWorker_FuneralFramework()
        {
        }
        public RitualBehaviorWorker_FuneralFramework(RitualBehaviorDef def) : base(def)
        {
        }
        //Overcomes checks on corpses
        public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
        {
            extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
            if (CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
            {
                return;
            }
            //So it turns out obligation doesnt change when you change assigment. Which sucks. I hope I can do it here and it will flow everywher
            RitualObligation obligationToUse = obligation;
            foreach (RitualObligation obligationTemp in ritual.activeObligations)
            {
                if (obligationTemp.targetA.Thing == assignments.AssignedPawns(extension.corpseRitualRoleID).First())
                {
                    
                    obligationToUse = obligationTemp;
                }
            }
            if (!base.CanExecuteOn(target, obligationToUse))
            {
                return;
            }
            
            LordJob_Ritual_FuneralFramework lordJob = (LordJob_Ritual_FuneralFramework)CreateLordJob(target, organizer, ritual, obligationToUse, assignments);            
            LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate (Pawn p)
            {
                bool flag = p.Dead;
                return !flag;
            }));

            FuneralFramework_PreparePawn(assignments);
            PostExecute(target, organizer, ritual, obligationToUse, assignments);
            if (playerForced)
            {
                foreach (Pawn pawn in assignments.Participants)
                {
                    if (!pawn.Dead) 
                    { 
                        pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
                    }
                }
            }

        }

        protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
        {
            corpse = assignments.AssignedPawns(extension.corpseRitualRoleID).First();
            IntVec3 spot = RitualSpot(target, organizer, ritual, obligation, assignments);
            return new LordJob_Ritual_FuneralFramework(target, ritual, obligation, def.stages, assignments, corpse, spot, organizer);
        }
        public virtual IntVec3 RitualSpot(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
        {
            Thing thing = target.Thing;
            IntVec3 cell = thing.OccupiedRect().CenterCell;
            if (def.HasModExtension<FuneralFramework_BehaviorExtension>())
            {
                var extension = def.GetModExtension<FuneralFramework_BehaviorExtension>();
                if (extension.spotOffset.TryGetValue(target.Thing.def, out var offset))
                {
                    cell = thing.Position + offset.RotatedBy(thing.Rotation);
                }                
            }
            return cell;
        }
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
            if (ritual.activeObligations == null)
            {
                return null;
            }

            bool flag = false;
            if (Find.TickManager.TicksGame < checkTick)
            {                
                return lastResult;
            }
            checkTick = Find.TickManager.TicksGame + 300;//This is too heavy to check often

            using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(target.Map).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Ritual == ritual)
                    {
                        return lastResult = "CantStartRitualAlreadyInProgress".Translate(ritual.Label).CapitalizeFirst();
                    }
                }
            }
            //Same as harmony patch use obligation as that basically saved our criteria for animals so no need to recheck everything
            foreach (RitualObligation obligation in ritual.activeObligations)
            {
                Corpse corpse = (obligation.targetA.Thing as Pawn)?.Corpse;
                if (corpse != null && !corpse.Destroyed)
                {
                    if (corpse.ParentHolder as Building_Casket == null)//Can be buried is a lie
                    {
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                return lastResult = "Funeral_DisabledCorpseInaccessible".Translate();
            }

            if (ritual.outcomeEffect.def.HasModExtension<OutcomeEffectExtension>())
            {
                outcomeExt = ritual.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();
                if(outcomeExt.outcomeSpawners?.Any(x=>x.stuffCount >0)??false)
                {
                    
                    return lastResult = CanStartStuff(target, ritual, selectedPawn, forcedForRole);
                }

                if (outcomeExt.outcomeSpawners?.Any(x => x.thingsRequired.Count > 0)?? false)
                {
                    return lastResult = CanStartThing(target, ritual, selectedPawn, forcedForRole);
                }
            }

            return lastResult = null; 
        }
        //No longer using obligation target workers for stuff/things/corpses. This only gets call 1 per frame, not n*obligation+1 plus giving it long wait between checks
        public virtual string CanStartStuff(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            OutcomeEffectExtension data = outcomeExt;
            StringBuilder failReasons = new StringBuilder();
            foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners.Where(x => x.stuffCount > 0))
            {
                if (!spawner.CanStartStuff())
                {
                    failReasons.AppendInNewLine("Funeral_NotEnoughStuff".Translate(string.Join(", ", spawner.stuffCategoryDefs.Select(x => x.label)).Named("STUFF"), spawner.stuffCount.ToString().Named("COUNT")));
                }
            }

            if (failReasons.Length > 0)
            {
                lastResult = failReasons.ToString();
                return failReasons.ToString();
            }
            return null;
        }

        public virtual string CanStartThing(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            OutcomeEffectExtension data = outcomeExt;
            StringBuilder failReasons = new StringBuilder();
            foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners.Where(x => x.thingsRequired.Count > 0))
            {
                AcceptanceReport report = spawner.CanStartThings();
                if (!report.Accepted)
                {
                    failReasons.AppendInNewLine("Funeral_NotEnoughThings".Translate(report.Reason.Named("Missing")));
                }
            }

            if (failReasons.Length > 0)
            {
                return failReasons.ToString();
            }
            return null;
        }


        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);

            if (soundPlaying != null)
            {
                soundPlaying.Maintain();
            }
            if(effecter != null)
            {
                effecter.EffectTick(effecterpawn, ritual.selectedTarget);
            }

        }
        public override Sustainer SoundPlaying
        {
            get
            {
                return this.soundPlaying;
            }
        }

        public override void Cleanup(LordJob_Ritual ritual)
        {
            base.Cleanup(ritual);
            if (deregisteredCorpse)
            {
                if(!corpse.Corpse?.Bugged ?? false && !ritual.Map.dynamicDrawManager.DrawThingsForReading.Contains(corpse.Corpse))
                {
                    ritual.Map.dynamicDrawManager.RegisterDrawable(corpse.Corpse);
                }                  
            }
            if (this.soundPlaying != null)
            {
                soundPlaying.End();
                soundPlaying = null;
            }
            if(effecter != null)
            {
                effecter.Cleanup();
                effecter = null;
            }
            foreach(Thing thing in spawnEffectThings)
            {
                if (!thing.Destroyed)
                {
                    thing.Destroy();
                }
            }
            stuffToUse = null;
            corpse = null;
            stuffCount = 0;
        }
        protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
        {
        }
        public void FuneralFramework_PreparePawn(RitualRoleAssignments assignments)
        {
            foreach (Pawn participant in assignments.Participants)
            {
                if (participant.drafter != null)
                {
                    participant.drafter.Drafted = false;
                }
                if (!participant.Awake() && !participant.Dead)
                {
                    RestUtility.WakeUp(participant);
                }
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ThingDef>(ref stuffToUse, "stuffToUse");
            Scribe_Values.Look(ref stuffCount, "stuffCount");
            Scribe_References.Look<Pawn>(ref corpse, "corpse",true);
            Scribe_References.Look(ref effecterpawn, "effecterpawn", true);            
            Scribe_Values.Look(ref deregisteredCorpse, "deregisteredCorpse");
            Scribe_Collections.Look(ref spawnEffectThings, false, "spawnEffectThings", LookMode.Reference);
        }

             
    }
}
