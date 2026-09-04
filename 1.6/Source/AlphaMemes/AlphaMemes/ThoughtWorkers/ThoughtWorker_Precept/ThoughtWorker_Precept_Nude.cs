
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Nude : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                Apparel apparel = wornApparel[i];
                if (!apparel.def.apparel.countsAsClothingForNudity)
                {
                    continue;
                }
                for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
                {
                    if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
                    {
                        return false;
                    }
                    if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}