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
    public class FloatMenuOptionProvider_RelicSmashingAltar : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;

        protected override bool Undrafted => true;

        protected override bool Multiselect => false;

        protected override bool RequiresManipulation => true;

        protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
        {
            if (context.FirstSelectedPawn.Ideo?.HasMeme(InternalDefOf.AM_Iconoclast) == true)
            {

                if (!CompRelicSmashingContainer.IsRelic(clickedThing))
                {
                    return null;
                }
                IEnumerable<Thing> searchSet = from x in clickedThing.Map.listerThings.ThingsOfDef(InternalDefOf.AM_RelicSmashingAltar)
                                               where x.TryGetComp<CompRelicSmashingContainer>().ContainedThing == null
                                               select x;
                Thing thing5 = GenClosest.ClosestThing_Global_Reachable(clickedThing.Position, clickedThing.Map, searchSet, PathEndMode.Touch, TraverseParms.For(context.FirstSelectedPawn), 9999f, (Thing t) => context.FirstSelectedPawn.CanReserve(t));
                if (thing5 == null)
                {
                    return new FloatMenuOption("AM_InstallInReliquary".Translate() + " (" + "AM_NoEmptyReliquary".Translate() + ")", null);
                   
                }
                Job job = JobMaker.MakeJob(InternalDefOf.AM_InstallRelic, clickedThing, thing5, thing5.InteractionCell);
                job.count = 1;
                return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("AM_InstallInReliquary".Translate(), delegate
                {
                    context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }), context.FirstSelectedPawn, new LocalTargetInfo(clickedThing));

            }


            return null;


        }
    }
    /*
    [HarmonyPatch(typeof(FloatMenuMakerMap))]
    [HarmonyPatch("AddHumanlikeOrders")]
    public static class AlphaMemes_FloatMenuMakerMap_AddHumanlikeOrders_Patch
    {
        [HarmonyPostfix]
        static void AddDragRelicToAltar(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> opts)
        {

            if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn.Ideo?.HasMeme(InternalDefOf.AM_Iconoclast) == true)
            {
                IntVec3 c = IntVec3.FromVector3(clickPos);
                List<Thing> thingList = c.GetThingList(pawn.Map);
                foreach (Thing item in thingList)
                {
                   
                }

            }








        }
    }


    */





}
