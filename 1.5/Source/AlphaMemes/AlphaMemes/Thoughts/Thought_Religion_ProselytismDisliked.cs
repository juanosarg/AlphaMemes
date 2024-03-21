using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class Thought_Religion_ProselytismDisliked : ThoughtWorker_Precept_Social
	{

		

		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (otherPawn.ideo == null)
            {
				return false;
            }

			foreach(MemeDef meme in StaticCollectionsClass.listProselytizerMemes)
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
