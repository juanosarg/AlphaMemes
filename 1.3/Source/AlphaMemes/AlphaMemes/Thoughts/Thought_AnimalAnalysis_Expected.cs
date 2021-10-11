using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_AnimalAnalysis_Expected : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

           

            float progressTotal = StaticCollectionsClass.databaseCompletion;


            if (progressTotal == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            /*else if (StaticCollectionsClass.artInTheMap < colonistTotal)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollectionsClass.artInTheMap < colonistTotal * 1.5)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            else if (StaticCollectionsClass.artInTheMap < colonistTotal * 2)
            {
                return ThoughtState.ActiveAtStage(3);

            }
            else if (StaticCollectionsClass.artInTheMap < colonistTotal * 3)
            {
                return ThoughtState.ActiveAtStage(4);

            }*/
            else
            {
                return ThoughtState.ActiveAtStage(1);

            }












        }


    }
}
