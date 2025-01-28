using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_AnimaTrees : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome || !StaticCollections.animaTreesInMap.ContainsKey(p.Map))
            {
                return false;
            }

            if (StaticCollections.animaTreesInMap[p.Map] == 0)
            {
                return false;

            }
            else if (StaticCollections.animaTreesInMap[p.Map] ==1)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollections.animaTreesInMap[p.Map] >1)
            {
                return ThoughtState.ActiveAtStage(1);

            }

            return false;

        }


    }
}
