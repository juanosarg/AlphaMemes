using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;


namespace AlphaMemes
{
    public class StageFailTrigger_StuffNotReachable : StageFailTrigger
    {
        public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
        {
            var behavior = ritual.Ritual.behavior as RitualBehaviorWorker_FuneralFramework;
            Pawn pawn = ritual.PawnWithRole(takerId);
            //Few checks to make sure this didnt become true once picked up
            if (ritual.PawnTagSet(pawn, "Arrived"))
            {
                return false;
            }
            if(pawn.carryTracker?.CarriedThing?.def == behavior.stuffToUse) { return false; }

            Thing thingToGet = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(behavior.stuffToUse), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly), 9999f,
            (Thing x) => x.stackCount >= behavior.stuffCount && pawn.CanReserve(x, 10, behavior.stuffCount, null, true), null, 0, -1, false, RegionType.Set_Passable, true);
            if (thingToGet == null) { return true; }
            if (!pawn.CanReach(thingToGet, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, Danger.Deadly))) { return true; }
            return false;
            
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref takerId, "takerId");

        }
        public string takerId;
    }
}
