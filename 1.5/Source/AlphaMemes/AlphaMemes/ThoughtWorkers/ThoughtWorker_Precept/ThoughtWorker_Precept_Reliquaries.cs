using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class ThoughtWorker_Precept_Reliquaries : ThoughtWorker_Precept
	{
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{

            if (p.Map?.IsPlayerHome != true || !StaticCollections.reliquariesInTheMap.ContainsKey(p.Map))
            {
                return false;
            }
            if (StaticCollections.reliquariesInTheMap[p.Map] > 0)
			{
				return ThoughtState.ActiveAtStage(0);

			}
			else return false;






		}
	}
}