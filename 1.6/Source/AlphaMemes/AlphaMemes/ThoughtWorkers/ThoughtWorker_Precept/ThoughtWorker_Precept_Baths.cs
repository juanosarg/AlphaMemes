using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Baths : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (!p.Map.IsPlayerHome || !StaticCollections.bathsAndShowersInTheMap.ContainsKey(p.Map))
            {
                return false;
            }

            int colonistTotal = p.Map.mapPawns.FreeColonistsSpawned.Count;


            if (StaticCollections.bathsAndShowersInTheMap[p.Map] == 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else if (StaticCollections.bathsAndShowersInTheMap[p.Map] < colonistTotal)
            {
                return ThoughtState.ActiveAtStage(1);

            }
            else if (StaticCollections.bathsAndShowersInTheMap[p.Map] < colonistTotal * 1.25)
            {
                return ThoughtState.ActiveAtStage(2);

            }
            
            else
            {
                return ThoughtState.ActiveAtStage(3);

            }












        }


    }
}
