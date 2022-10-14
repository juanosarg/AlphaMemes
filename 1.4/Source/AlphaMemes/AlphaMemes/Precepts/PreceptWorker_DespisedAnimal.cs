// RimWorld.PreceptWorker_Animal
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;




namespace AlphaMemes {

	public class PreceptWorker_DespisedAnimal : PreceptWorker
	{
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(ThingCategoryDefOf.Animals) && !x.race.Dryad))
				{
					PreceptThingChance preceptThingChance = default(PreceptThingChance);
					preceptThingChance.def = item;
					preceptThingChance.chance = 1f;
					yield return preceptThingChance;
				}
			}
		}

		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			foreach (Precept item in ideo.PreceptsListForReading)
			{
				Precept_DespisedAnimal precept_Animal;
				if ((precept_Animal = item as Precept_DespisedAnimal) != null && precept_Animal.ThingDef == def)
				{
					return false;
				}
			}
			return true;
		}
	}

}


