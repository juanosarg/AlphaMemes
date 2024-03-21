using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_Art_Desired : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome)
            {
                return false;
            }

            int colonistTotal = p.Map.mapPawns.FreeColonistsSpawned.Count;


            if (StaticCollectionsClass.artInTheMap == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollectionsClass.artInTheMap < colonistTotal)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollectionsClass.artInTheMap < colonistTotal*1.5)
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

            }
            else
            {
                return ThoughtState.ActiveAtStage(5);

            }












        }


    }
}
