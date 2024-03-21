using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class Thought_RelicDestruction_Desired : ThoughtWorker_Precept
	{



		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			
			switch (StaticCollectionsClass.relicsDestroyedThisGame)
            {
				case 0:
					return false;
					
				case 1:
					return ThoughtState.ActiveAtStage(0);
				case 2:
					return ThoughtState.ActiveAtStage(1);
				case 3:
					return ThoughtState.ActiveAtStage(2);
				default:
					return ThoughtState.ActiveAtStage(2);
			}

			


		}


	}
}
