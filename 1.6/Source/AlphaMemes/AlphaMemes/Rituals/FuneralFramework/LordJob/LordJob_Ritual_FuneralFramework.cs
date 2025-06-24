using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Verse.AI;
using HarmonyLib;
using Verse.AI.Group;
using RimWorld;


namespace AlphaMemes
{
    //made my own to try an get a bit more control over things
	//1 harmony patch still because base has lots of private fields and it was just easier
	//20241216 added annoying verbose logging to try and identify why rituals are failing silently sometimes for users.
	//Not helpful or enough info to inform user yet, but can be used to inform why so I can implment something for the user
    public class LordJob_Ritual_FuneralFramework : LordJob_Ritual
    {

        public LordJob_Ritual_FuneralFramework(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments,Pawn corpse, IntVec3 spot ,Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer)
        {
            this.corpse = corpse;
			this.spot = spot;
		}
		public LordJob_Ritual_FuneralFramework()
        {
        }
		public override void Cleanup()
        {
            base.Cleanup();

        }
        protected override bool ShouldBeCalledOff()
		{
			//Adding logging here too give some hints on the many silent cancels/errors encountered
			if (lord.ownedPawns.Count == 0)
			{
				Log.Message("C1 Alpha Memes Funeral Cancelled because ownedpawns count = 0");
				return true;
			}
			if (selectedTarget.ThingDestroyed)
			{
                Log.Message("C2 Alpha Memes Funeral Cancelled because selected Thing destroyed");
                return true;
			}
			if (organizer != null && this.organizer.Downed)
			{
                Log.Message("C3 Alpha Memes Funeral Cancelled because organizer downed or doesnt exist");
                return true;
			}
			foreach (RitualRole role in this.assignments.AllRolesForReading)
			{
				if (role.required)
				{
					Pawn p = this.assignments.FirstAssignedPawn(role);
					if (p != null && !this.lord.ownedPawns.Contains(p) && ShouldCallOffBecausePawnNoLongerOwned(p))
					{
                        Log.Message("C4 Alpha Memes Funeral Cancelled because required Pawn no longer exists");
                        return true;
					}
				}
			}
			foreach (Pawn p2 in this.assignments.ExtraRequiredPawnsForReading)
			{
				if (!lord.ownedPawns.Contains(p2) && ShouldCallOffBecausePawnNoLongerOwned(p2))
				{
                    Log.Message("C5 Alpha Memes Funeral Cancelled because required Pawn no longer owned");
                    return true;
				}
				IThingHolder thingHolder;
				if (p2.Corpse == null || p2.Corpse.ParentHolder == null)
				{
					IThingHolder carriedBy = p2.CarriedBy;
					thingHolder = carriedBy;
				}
				else
				{
					thingHolder = p2.Corpse.ParentHolder.ParentHolder;
				}
				Pawn p3 = thingHolder as Pawn;
				if (p3 != null && p3.GetLord() != this.lord)
				{
                    Log.Message("C6 Alpha Memes Funeral Cancelled because corpse thingholder doesnt exist");
                    return true;
				}
			}
			foreach (Pawn participant in this.assignments.Participants)
			{
				if (participant.Map != null)
				{
					RitualRole ritualRole = this.assignments.RoleForPawn(participant, true);
					if (((ritualRole != null && ritualRole.required) || this.assignments.Required(participant) || this.assignments.ExtraRequiredPawnsForReading.Contains(participant)) && SelfDefenseUtility.ShouldStartFleeing(participant))
					{
                        Log.Message("C7 Alpha Memes Funeral Cancelled because particpant " + participant.Name + " either required or started fleeing");
                        return true;
					}
				}
			}
			return false;
		}
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
        {
            return p != corpse;
        }
        public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
        {
            base.Notify_PawnLost(p, condition);
			if(p == organizer && organizer != null)
			{
                Log.Message("C8 Alpha Memes Funeral Cancelled because particpant " + p.Name + " has left the lord and is required");
            }
        }
        public override void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
        {
            base.ApplyOutcome(progress, showFinishedMessage, showFailedMessage, cancelled);
			
			if(progress <1f) //These are all based on the various transitions delegates that can cause a failed outcome without messages.
			{
                if (cancelled && !showFailedMessage) { Log.Message("C9 Alpha Memes Funeral Cancelled from transition3"); }
                if (cancelled && showFailedMessage) { Log.Message("C10 Alpha Memes Funeral interrupted from transition2 cancel signal"); }
                if (!cancelled && showFailedMessage) { Log.Message("C11 Alpha Memes Funeral interrupted from transition call off signals"); }
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ThingDef>(ref stuffToUse, "stuffToUse");
            Scribe_Values.Look(ref stuffCount, "stuffCount");
            Scribe_References.Look<Pawn>(ref corpse, "corpse",true);

        }
        public Pawn corpse;
        public FuneralPreceptExtension extension;
        public ThingDef stuffToUse;
        public int stuffCount;
    }
}
