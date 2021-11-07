using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class Thought_Reliquaries_Forbidden : ThoughtWorker_Precept
	{
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (StaticCollectionsClass.reliquariesInTheMap > 0)
			{
				return ThoughtState.ActiveAtStage(0);

			}
			else return false;






		}
	}
}