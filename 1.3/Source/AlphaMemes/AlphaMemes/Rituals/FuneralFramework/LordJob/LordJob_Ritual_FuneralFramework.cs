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
    public class LordJob_Ritual_FuneralFramework : LordJob_Ritual
    {

        public LordJob_Ritual_FuneralFramework(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments,Pawn corpse, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer)
        {
            this.corpse = corpse;
            if (selectedTarget.HasThing)
            {
                Thing thing = selectedTarget.Thing;
                IntVec3 cell = thing.OccupiedRect().CenterCell;

                if (thing.def.passability != Traversability.Standable)
                {
                    if (!thing.def.hasInteractionCell)
                    {
                        CellFinder.TryFindRandomReachableCellNear(thing.Position, thing.Map, 1, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), null, null, out cell);//Random spot can cause weird positioning of the progress bar
                    }
                    else
                    {
                        cell = thing.InteractionCell;
                    }

                    spot = cell;
                }
            }

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
			if (lord.ownedPawns.Count == 0)
			{
				return true;
			}
			if (selectedTarget.ThingDestroyed)
			{
				return true;
			}
			if (organizer != null && this.organizer.Downed)
			{
				return true;
			}
			foreach (RitualRole role in this.assignments.AllRolesForReading)
			{
				if (role.required)
				{
					Pawn p = this.assignments.FirstAssignedPawn(role);
					if (p != null && !this.lord.ownedPawns.Contains(p) && ShouldCallOffBecausePawnNoLongerOwned(p))
					{
						return true;
					}
				}
			}
			foreach (Pawn p2 in this.assignments.ExtraRequiredPawnsForReading)
			{
				if (!this.lord.ownedPawns.Contains(p2) && this.ShouldCallOffBecausePawnNoLongerOwned(p2))
				{
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
						return true;
					}
				}
			}
			return false;
		}
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
        {
            return !(p == corpse);
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
