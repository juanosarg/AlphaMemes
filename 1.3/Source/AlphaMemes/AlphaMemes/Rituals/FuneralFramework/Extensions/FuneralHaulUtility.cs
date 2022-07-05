using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using Verse.AI;
using System.Text;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public static class FuneralHaulUtility

		//started with dobill but borrowed a lot from VPFE welding utility after finding it. cause was going insane. credit to them saved me many hours
    {
        public static bool TryFindBestBillIngredients(List<IngredientCount> ingredients, Pawn pawn, Thing billGiver, List<ThingCount> chosen)
        {
            return TryFindBestIngredientsHelper((Thing t) => ingredients.Any((IngredientCount ingNeed) => ingNeed.filter.Allows(t)),				
				(List<Thing> foundThings) => tryFindBestIngredientsInSet_NoMixHelper(foundThings, ingredients, chosen, billGiver.Position,false)
				, ingredients, pawn, billGiver, chosen, 999f);
        }


		private static bool TryFindBestIngredientsHelper(Predicate<Thing> thingValidator, Predicate<List<Thing>> foundAllIngredientsAndChoose, List<IngredientCount> ingredients, Pawn pawn, Thing billGiver, List<ThingCount> chosen, float searchRadius)
		{
			chosen.Clear();
			newRelevantThings.Clear();
			if (ingredients.Count == 0)
			{
				return true;
			}
			IntVec3 billGiverRootCell = billGiver.Position;
			Region rootReg = billGiverRootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (rootReg == null)
			{
				return false;
			}
			relevantThings.Clear();
			processedThings.Clear();
			bool foundAll = false;
			float radiusSq = searchRadius * searchRadius;
			Predicate<Thing> baseValidator = (Thing t) => t.Spawned && !t.IsForbidden(pawn) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < radiusSq && thingValidator(t) && pawn.CanReserve(t, 1, -1, null, false);

			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			RegionEntryPredicate entryCondition = null;
			if (Math.Abs(999f - searchRadius) >= 1f)
			{
				entryCondition = delegate (Region from, Region r)
				{
					if (!r.Allows(traverseParams, false))
					{
						return false;
					}
					CellRect extentsClose = r.extentsClose;
					int num = Math.Abs(billGiver.Position.x - Math.Max(extentsClose.minX, Math.Min(billGiver.Position.x, extentsClose.maxX)));
					if ((float)num > searchRadius)
					{
						return false;
					}
					int num2 = Math.Abs(billGiver.Position.z - Math.Max(extentsClose.minZ, Math.Min(billGiver.Position.z, extentsClose.maxZ)));
					return (float)num2 <= searchRadius && (float)(num * num + num2 * num2) <= radiusSq;
				};
			}
			else
			{
				entryCondition = ((Region from, Region r) => r.Allows(traverseParams, false));
			}
			int adjacentRegionsAvailable = rootReg.Neighbors.Count((Region region) => entryCondition(rootReg, region));
			int regionsProcessed = 0;
			processedThings.AddRange(relevantThings);
			RegionProcessor regionProcessor = delegate (Region r)
			{
				List<Thing> thingList = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int index = 0; index < thingList.Count; index++)
				{
					Thing thing = thingList[index];
					if (!processedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing))
					{
						newRelevantThings.Add(thing);
						processedThings.Add(thing);
					}
				}				
				regionsProcessed++;				
				if (newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
				{
					relevantThings.AddRange(newRelevantThings);
					newRelevantThings.Clear();
					if (foundAllIngredientsAndChoose(relevantThings))
					{
						foundAll = true;
						return true;
					}
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
			relevantThings.Clear();
			newRelevantThings.Clear();
			processedThings.Clear();
			return foundAll;
		}

		
		//I straight stole this from VFEP hope they dont mind but saved me much harmony hair pulling
		public delegate bool TryFindBestIngredientsInSet_NoMixHelper(List<Thing> availableThings, List<IngredientCount> ingredients, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted, Bill bill = null);
		public static readonly TryFindBestIngredientsInSet_NoMixHelper tryFindBestIngredientsInSet_NoMixHelper =
	AccessTools.MethodDelegate<TryFindBestIngredientsInSet_NoMixHelper>(AccessTools.Method(typeof(WorkGiver_DoBill), "TryFindBestIngredientsInSet_NoMixHelper"));

		private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);

		private static List<Thing> relevantThings = new List<Thing>();


		private static HashSet<Thing> processedThings = new HashSet<Thing>();


		private static List<Thing> newRelevantThings = new List<Thing>();
	}
}
