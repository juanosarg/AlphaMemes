using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class Thought_Armour_Expected : ThoughtWorker_Precept
	{
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			bool flag = false;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff?.stuffProps?.categories?.Contains(StuffCategoryDefOf.Metallic) == true || wornApparel[i].def?.thingCategories?.Contains(ThingCategoryDefOf.ApparelArmor)==true)
				{
					flag = true;
				}
			}

			if (flag)
			{

				return ThoughtState.ActiveAtStage(1);
			}
			else return ThoughtState.ActiveAtStage(0);






		}
	}
}