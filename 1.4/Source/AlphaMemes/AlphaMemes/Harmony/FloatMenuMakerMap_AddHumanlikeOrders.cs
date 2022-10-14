using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Verse.AI;
using System.Reflection;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(FloatMenuMakerMap))]
    [HarmonyPatch("AddHumanlikeOrders")]
    public static class AlphaMemes_FloatMenuMakerMap_AddHumanlikeOrders_Patch
    {
        [HarmonyPostfix]
        static void AddDragRelicToAltar(Vector3 clickPos, Pawn pawn,ref List<FloatMenuOption> opts)
        {

			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn.Ideo?.HasMeme(InternalDefOf.AM_Iconoclast)==true)
			{
				IntVec3 c = IntVec3.FromVector3(clickPos);
				List<Thing> thingList = c.GetThingList(pawn.Map);
				foreach (Thing item in thingList)
				{
					if (!CompRelicSmashingContainer.IsRelic(item))
					{
						continue;
					}
					IEnumerable<Thing> searchSet = from x in item.Map.listerThings.ThingsOfDef(InternalDefOf.AM_RelicSmashingAltar)
												   where x.TryGetComp<CompRelicSmashingContainer>().ContainedThing == null
												   select x;
					Thing thing5 = GenClosest.ClosestThing_Global_Reachable(item.Position, item.Map, searchSet, PathEndMode.Touch, TraverseParms.For(pawn), 9999f, (Thing t) => pawn.CanReserve(t));
					if (thing5 == null)
					{
						opts.Add(new FloatMenuOption("AM_InstallInReliquary".Translate() + " (" + "AM_NoEmptyReliquary".Translate() + ")", null));
						continue;
					}
					Job job = JobMaker.MakeJob(InternalDefOf.AM_InstallRelic, item, thing5, thing5.InteractionCell);
					job.count = 1;
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("AM_InstallInReliquary".Translate(), delegate
					{
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					}), pawn, new LocalTargetInfo(item)));
				}

			}

			






		}
    }








}
