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

            if(p.Faction!= Faction.OfPlayer)
            {
                return false;
            }

            if (progressTotal == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (progressTotal <= 0.01)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (progressTotal <= 0.05)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            else if (progressTotal <= 0.1)
            {
                return ThoughtState.ActiveAtStage(3);

            }
            else if (progressTotal <= 0.25)
            {
                return ThoughtState.ActiveAtStage(4);

            }
            else if (progressTotal <= 0.4)
            {
                return ThoughtState.ActiveAtStage(5);

            }
            else if (progressTotal <= 0.5)
            {
                return ThoughtState.ActiveAtStage(6);

            }
            else if (progressTotal <= 0.6)
            {
                return ThoughtState.ActiveAtStage(7);

            }
            else if (progressTotal <= 0.75)
            {
                return ThoughtState.ActiveAtStage(8);

            }
            else
            {
                return ThoughtState.ActiveAtStage(9);

            }












        }


    }
}
