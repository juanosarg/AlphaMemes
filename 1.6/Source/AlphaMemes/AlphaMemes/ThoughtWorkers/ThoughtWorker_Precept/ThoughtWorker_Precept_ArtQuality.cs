using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_ArtQuality : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome || !StaticCollections.artBeautyInTheMap.ContainsKey(p.Map))
            {
                return false;
            }

           
            if (StaticCollections.artBeautyInTheMap[p.Map] < 1)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollections.artBeautyInTheMap[p.Map] < 2)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollections.artBeautyInTheMap[p.Map] < 3)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            else if (StaticCollections.artBeautyInTheMap[p.Map] < 4)
            {
                return ThoughtState.ActiveAtStage(3);

            }
            else if (StaticCollections.artBeautyInTheMap[p.Map] < 5)
            {
                return ThoughtState.ActiveAtStage(4);

            }
            else if (StaticCollections.artBeautyInTheMap[p.Map] < 6)
            {
                return ThoughtState.ActiveAtStage(5);

            }
            else
            {
                return ThoughtState.ActiveAtStage(6);

            }












        }


    }
}
