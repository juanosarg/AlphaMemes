using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_PenMarkers : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome || !StaticCollections.penMarkersInMap.ContainsKey(p.Map))
            {
                return false;
            }

            if (StaticCollections.penMarkersInMap[p.Map] == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollections.penMarkersInMap[p.Map] ==1l)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollections.penMarkersInMap[p.Map] >1)
            {
                return ThoughtState.ActiveAtStage(2);

            }

            return false;

        }


    }
}
