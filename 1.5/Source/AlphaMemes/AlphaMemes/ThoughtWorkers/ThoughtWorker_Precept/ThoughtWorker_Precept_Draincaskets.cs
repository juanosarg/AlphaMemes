using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Draincaskets : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.Map?.IsPlayerHome != true || !StaticCollections.draincasketsInTheMap.ContainsKey(p.Map))
            {
                return false;
            }
            if (StaticCollections.draincasketsInTheMap[p.Map] > 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else return false;






        }
    }
}