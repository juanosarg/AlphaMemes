


using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;


namespace AlphaMemes
{


	public class Thought_Mood_Volatile : ThoughtWorker_Precept
	{

		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (StaticCollectionsClass.colonist_and_random_mood.ContainsKey(p))
			{

				switch (StaticCollectionsClass.colonist_and_random_mood[p])
				{

					case 0:
						return ThoughtState.ActiveAtStage(0);
					case 1:
						return ThoughtState.ActiveAtStage(1);
					case 2:
						return ThoughtState.ActiveAtStage(2);
					case 3:
						return ThoughtState.ActiveAtStage(3);
					case 4:
						return ThoughtState.ActiveAtStage(4);
					case 5:
						return ThoughtState.ActiveAtStage(5);
					case 6:
						return ThoughtState.ActiveAtStage(6);
					case 7:
						return ThoughtState.ActiveAtStage(7);
					case 8:
						return ThoughtState.ActiveAtStage(8);
					default:
						return ThoughtState.ActiveAtStage(4);



				}





			}
			else return false;







		}

	}
}

