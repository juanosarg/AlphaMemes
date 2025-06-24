using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class ThoughtWorker_Precept_Proselytism : ThoughtWorker_Precept_Social
	{

		

		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (otherPawn.ideo == null)
            {
				return false;
            }

			foreach(MemeDef meme in StaticCollections.listProselytizerMemes)
            {
				if (otherPawn.Ideo?.HasMeme(meme) == true)
                {
					return ThoughtState.ActiveAtStage(0);
				}

            }

			return false;


		}


	}
}
