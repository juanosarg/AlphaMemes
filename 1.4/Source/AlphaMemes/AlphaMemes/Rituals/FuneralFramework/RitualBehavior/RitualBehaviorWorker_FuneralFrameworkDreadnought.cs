using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using UnityEngine;
using Verse.AI;
using System.Reflection;
using HarmonyLib;
using Verse.AI.Group;
using RimWorld;


namespace AlphaMemes
{
    public class RitualBehaviorWorker_FuneralFrameworkDreadnought : RitualBehaviorWorker_FuneralFramework
    {
        public RitualBehaviorWorker_FuneralFrameworkDreadnought()
        {
        }
        public RitualBehaviorWorker_FuneralFrameworkDreadnought(RitualBehaviorDef def) : base(def)
        {
        }
        //Things going to get a bit tricky here
        public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
        {
            extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
            if (CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
            {
                return;
            }            
            RitualObligation obligationToUse = obligation;
            foreach (RitualObligation obligationTemp in ritual.activeObligations)
            {
                if (obligationTemp.targetA.Thing == assignments.AssignedPawns(extension.corpseRitualRoleID).First().Corpse)
                {
                    obligationToUse = obligationTemp;
                    corpse = assignments.AssignedPawns(extension.corpseRitualRoleID).First();
                }
            }
            if (!base.CanExecuteOn(target, obligationToUse))
            {
                return;
            }



            //Get defs           
            var helmetDef = DefDatabase<ThingDef>.GetNamed("VFEP_WarcasketHelmet_Sarcophagus");
            var shoulderDef = DefDatabase<ThingDef>.GetNamed("VFEP_WarcasketShoulders_Sarcophagus");
            var bodyDef = DefDatabase<ThingDef>.GetNamed("VFEP_Warcasket_Sarcophagus");
            Type type = AccessTools.TypeByName("VFEPirates.WarcasketProject");
            if (type == null)
            {
                Messages.Message("Error in Alphamemes VFE Pirates Integration can't proceed", MessageTypeDefOf.CautionInput);
                Log.Error("Alpha Memes - Unable to create VFE Pirates Project");
                return;
            }
            var project1 = Activator.CreateInstance(type, new object[] { corpse, bodyDef, shoulderDef, helmetDef });


            project = project1; 
            previousApparels = corpse.apparel.WornApparel.ListFullCopy();             
            foreach (var apparel in previousApparels)
            {
                corpse.apparel.Remove(apparel);
            }
            //more reflection
            bool canceled = false;
            type = AccessTools.TypeByName("VFEPirates.Buildings.Dialog_WarcasketCustomization");
            if (type == null)
            {
                Messages.Message("Error in Alphamemes VFE Pirates Integration can't proceed", MessageTypeDefOf.CautionInput);
                Log.Error("Alpha Memes - Unable to create VFE Pirates Dialog");
                return;
            }
            Action onAccept = () => 
            {
                //Dont do the apparel spawn stuff here cause when it gets called its before the corpse is in the right spot
                foreach (var apparel in corpse.apparel.LockedApparel.ToList())
                    apparel.Destroy();
                foreach (var apparel in corpse.apparel.WornApparel.ToList())                        
                    apparel.Destroy();
                foreach (var apparel in previousApparels)
                    corpse.apparel.Wear(apparel);

            };
            Action onCancel = () =>
            {
                foreach (var apparel in corpse.apparel.LockedApparel.ToList())
                    corpse.apparel.Unlock(apparel);
                foreach (var apparel in corpse.apparel.WornApparel.ToList())
                    apparel.Destroy();
                foreach (var apparel in previousApparels)
                    corpse.apparel.Wear(apparel);
                canceled=true;
            };
            if (canceled) { return; }//Not sure why onCancel return stopped working.
            var dialog = (Window)Activator.CreateInstance(type, new object[] { project1, corpse, onAccept, onCancel });

            //Rather then harmony patch just resseting lists after creating
            System.Collections.IList helmDefs = (System.Collections.IList)AccessTools.Field(type, "helmets").GetValue(dialog);
            helmDefs?.Clear();
            helmDefs?.Add(helmetDef);
            System.Collections.IList armordefs = (System.Collections.IList)AccessTools.Field(type, "armors").GetValue(dialog);
            armordefs?.Clear();
            armordefs?.Add(bodyDef);
            System.Collections.IList shoulderdefs = (System.Collections.IList)AccessTools.Field(type, "shoulders").GetValue(dialog);
            shoulderdefs?.Clear();
            shoulderdefs?.Add(shoulderDef);
            Find.WindowStack.Add(dialog);




            LordJob_Ritual_FuneralFramework lordJob = (LordJob_Ritual_FuneralFramework)this.CreateLordJob(target, organizer, ritual, obligationToUse, assignments);            
            LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate (Pawn p)
            {
                bool flag = p.Dead;
                return !flag;
            }));

            FuneralFramework_PreparePawn(assignments);            
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
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            ResearchProjectDef research = FuneralFrameWork_StaticStartup.VFEP_SpacerWarcaskets;
            if (!research?.IsFinished ?? true)
            {
                return "Funeral_ResearchNotCompleted".Translate(research.label);
            }
            return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
        }
        public override void Cleanup(LordJob_Ritual ritual)
        {
            base.Cleanup(ritual);
            if(foundry != null)
            {
                AccessTools.Field(foundry.GetType(), "occupant").SetValue(foundry, null);
                AccessTools.Field(foundry.GetType(), "curWarcasketProject").SetValue(foundry, null);
            }

            foundry = null;
            project = null;

        }
        protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
        {           
       

        }
        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);


        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref project, "project");
        }

        
        public object project;
        public List<Apparel> previousApparels;

        public Thing foundry;

    }
}
