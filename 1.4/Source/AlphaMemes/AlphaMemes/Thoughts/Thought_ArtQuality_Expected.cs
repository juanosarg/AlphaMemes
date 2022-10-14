using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_ArtQuality_Expected : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome)
            {
                return false;
            }

           
            if (StaticCollectionsClass.artBeautyInTheMap < 1)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollectionsClass.artBeautyInTheMap < 2)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollectionsClass.artBeautyInTheMap < 3)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            else if (StaticCollectionsClass.artBeautyInTheMap < 4)
            {
                return ThoughtState.ActiveAtStage(3);

            }
            else if (StaticCollectionsClass.artBeautyInTheMap < 5)
            {
                return ThoughtState.ActiveAtStage(4);

            }
            else if (StaticCollectionsClass.artBeautyInTheMap < 6)
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
