using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_DeathrestCaskets : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.Map?.IsPlayerHome != true || !StaticCollections.deathrestCasketsInTheMap.ContainsKey(p.Map))
            {
                return false;
            }
            if (StaticCollections.deathrestCasketsInTheMap[p.Map] > 0)
            {
                return ThoughtState.ActiveAtStage(0);

            }
            else return false;






        }
    }
}