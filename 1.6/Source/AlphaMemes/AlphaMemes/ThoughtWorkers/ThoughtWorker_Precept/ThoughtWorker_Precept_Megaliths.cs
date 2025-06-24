using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Megaliths : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.Map?.IsPlayerHome != true || !StaticCollections.megalithsInTheMap.ContainsKey(p.Map))
            {
                return false;
            }


            if (StaticCollections.megalithsInTheMap[p.Map] * 3 < p.Map.mapPawns.FreeColonistsSpawnedCount)
            {
                return ThoughtState.ActiveAtStage(0); 

            }
            return false;












        }


    }
}
