
using System.Collections.Generic;
using System;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class JobDriver_DeliverThingsToCell : JobDriver
	{
		protected Thing Target
		{
			get
			{
				return job.GetTarget(TargetIndex.A).Thing;
			}
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
			pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
			foreach (var target in job.GetTargetQueue(TargetIndex.B))
			{
				pawn.Map.physicalInteractionReservationManager.Reserve(pawn, job, target.Thing);
			}
			return true;
        }
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);			
			Toil toilGoTo = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			if(job.targetQueueB != null)
            {
				yield return Toils_Jump.JumpIf(toilGoTo, () => job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
				foreach (Toil item in CollectIngredientsToils(TargetIndex.B, TargetIndex.A, TargetIndex.C))
                {
					yield return item;
                }
            }

			yield return toilGoTo;
			yield return Toils_General.Do(delegate
			{
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					foreach(ThingCountClass kv in job.placedThings)
                    {
						lordJob_Ritual.usedThings.Add(kv.thing);
					}
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
					pawn.Map.physicalInteractionReservationManager.ReleaseClaimedBy(pawn, job);
				}
			});
			yield break;
		}

		public IEnumerable<Toil> CollectIngredientsToils(TargetIndex ingredientInd, TargetIndex billGiverInd, TargetIndex ingredientPlaceCellInd, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = true)
        {
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(ingredientInd, true);
			yield return extract;
			Toil getToHaulTarget = Toils_Goto.GotoThing(ingredientInd, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(ingredientInd).FailOnSomeonePhysicallyInteracting(ingredientInd);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(ingredientInd, true, subtractNumTakenFromJobCount, failIfStackCountLessThanJobCount);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(billGiverInd, PathEndMode.InteractionCell).FailOnDestroyedOrNull(ingredientInd);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(billGiverInd, ingredientInd, ingredientPlaceCellInd);
			yield return findPlaceTarget;
			yield return PlaceHauledThingInCell(ingredientPlaceCellInd, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(ingredientInd, extract);
			yield break;
		}
		//stupid hardcoding of defs making me copy whole blocks from dnspy
		public static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode, bool tryStoreInSameStorageIfSpotCantHoldWholeStack = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate ()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				IntVec3 cell = curJob.GetTarget(cellInd).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in cell but is not hauling anything.");
					return;
				}
				SlotGroup slotGroup = actor.Map.haulDestinationManager.SlotGroupAt(cell);
				if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
				{
					actor.Map.designationManager.TryRemoveDesignationOn(actor.carryTracker.CarriedThing, DesignationDefOf.Haul);
				}
				Action<Thing, int> placedAction = null; //Changed here to remove the if specifics defs				
				placedAction = delegate (Thing th, int added)
				{
					if (curJob.placedThings == null)
					{
						curJob.placedThings = new List<ThingCountClass>();
					}
					ThingCountClass thingCountClass = curJob.placedThings.Find((ThingCountClass x) => x.thing == th);
					if (thingCountClass != null)
					{
						thingCountClass.Count += added;
						return;
					}
					curJob.placedThings.Add(new ThingCountClass(th, added));
				};
				
				Thing local_5;
				if (!actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out local_5, placedAction))
				{
					if (storageMode)
					{
						IntVec3 foundCell;
						if (nextToilOnPlaceFailOrIncomplete != null && ((tryStoreInSameStorageIfSpotCantHoldWholeStack && StoreUtility.TryFindBestBetterStoreCellForIn(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, cell.GetSlotGroup(actor.Map), out foundCell, true)) || StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out foundCell, true)))
						{
							if (actor.CanReserve(foundCell, 1, -1, null, false))
							{
								actor.Reserve(foundCell, actor.CurJob, 1, -1, null, true);
							}
							actor.CurJob.SetTarget(cellInd, foundCell);
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						IntVec3 storeCell;
						if (HaulAIUtility.CanHaulAside(actor, actor.carryTracker.CarriedThing, out storeCell))
						{
							curJob.SetTarget(cellInd, storeCell);
							curJob.count = int.MaxValue;
							curJob.haulOpportunisticDuplicates = false;
							curJob.haulMode = HaulMode.ToCellNonStorage;
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						Log.Warning(string.Format("Incomplete haul for {0}: Could not find anywhere to put {1} near {2}. Destroying. This should be very uncommon!", actor, actor.carryTracker.CarriedThing, actor.Position));
						actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
						return;
					}
					else if (nextToilOnPlaceFailOrIncomplete != null)
					{
						actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
						return;
					}
				}
			};
			return toil;
		}


		private const TargetIndex StuffIndex = TargetIndex.A;

		
		private const TargetIndex TargetCellIndex = TargetIndex.B;
	}
}
