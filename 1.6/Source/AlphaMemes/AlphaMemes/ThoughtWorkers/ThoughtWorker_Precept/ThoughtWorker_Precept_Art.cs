using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Art : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome || !StaticCollections.artInTheMap.ContainsKey(p.Map))
            {
                return false;
            }

            int colonistTotal = p.Map.mapPawns.FreeColonistsSpawned.Count;


            if (StaticCollections.artInTheMap[p.Map] == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollections.artInTheMap[p.Map] < colonistTotal)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollections.artInTheMap[p.Map] < colonistTotal*1.5)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            else if (StaticCollections.artInTheMap[p.Map] < colonistTotal * 2)
            {
                return ThoughtState.ActiveAtStage(3);

            }
            else if (StaticCollections.artInTheMap[p.Map] < colonistTotal * 3)
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
