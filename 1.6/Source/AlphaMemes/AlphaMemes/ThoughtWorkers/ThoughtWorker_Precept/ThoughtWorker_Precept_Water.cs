using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Water : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!StaticCollections.mapAndWateriness.ContainsKey(p.Map))
            {
                return false;
            }
            if (StaticCollections.mapAndWateriness[p.Map])
            {
                return ThoughtState.ActiveAtStage(0);
            }

            else return ThoughtState.ActiveAtStage(1);






        }
    }
}