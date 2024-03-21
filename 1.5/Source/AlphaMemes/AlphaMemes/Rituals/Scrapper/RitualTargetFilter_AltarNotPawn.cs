using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace AlphaMemes
{
    public class RitualTargetFilter_AltarNotPawn: RitualTargetFilter_Altar
    {
        public RitualTargetFilter_AltarNotPawn()
        {
        }
        public RitualTargetFilter_AltarNotPawn(RitualTargetFilterDef def) : base(def)
        {
        }

        public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
        {
            Thing thing = initiator.Thing;
            Thing returnThing = null;
            Ideo ideo = Find.FactionManager.OfPlayer.ideos.PrimaryIdeo;//This will only work with primary ideos
            float dist = 99999f;
            foreach (Building candidateBuilding in this.CandidateBuildings(ideo, initiator.Thing.Map))
            {           
                if (candidateBuilding.def.isAltar && thing.Map.reachability.CanReach(thing.Position, candidateBuilding, PathEndMode.Touch,TraverseMode.PassDoors, Danger.Deadly))
                {
                    int horizontalSquared = (thing.Position - candidateBuilding.Position).LengthHorizontalSquared;
                    if ((float)horizontalSquared < dist)
                    {
                        returnThing = candidateBuilding;
                        dist = (float)horizontalSquared;
                    }
                }
            }
            return returnThing;
        }
    }
}
