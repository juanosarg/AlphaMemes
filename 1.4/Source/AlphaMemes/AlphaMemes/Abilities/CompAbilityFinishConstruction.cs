using System;
using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
	public class CompAbilityFinishConstruction : CompAbilityEffect
	{
		

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Frame blueprint = target.Thing as Frame;
			
			if (blueprint != null && blueprint.Faction== Faction.OfPlayer)
			{

				List<ThingDefCountClass> list = blueprint.def.entityDefToBuild.CostListAdjusted(blueprint.Stuff);
				
				int resourcesLeft = 0;
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass need = list[i];
					int num = need.count;
					
					foreach (ThingDefCountClass item in from needed in blueprint.MaterialsNeeded()
														where needed.thingDef == need.thingDef
														select needed)
					{
						resourcesLeft += item.count;
					}
					
				}
				
				if (resourcesLeft==0)
				{
					if(blueprint.def.entityDefToBuild != InternalDefOf.AM_GreatPyramid&& blueprint.def.entityDefToBuild != InternalDefOf.AM_Sphynx)
                    {
						blueprint.CompleteConstruction(this.parent.pawn);
                    }
                    else
                    {
						Messages.Message("AM_AbilityNiceTry".Translate(), MessageTypeDefOf.RejectInput, true);

					}					
                }
                else
                {
					Messages.Message("AM_AbilityNeedsResourcesDelivered".Translate(), MessageTypeDefOf.RejectInput, true);
					this.parent.StartCooldown(30);
				}

			}
			else
			{
				Messages.Message("AM_AbilityNeedsABlueprint".Translate(), MessageTypeDefOf.RejectInput, true);
				this.parent.StartCooldown(30);
			}



			base.Apply(target, dest);

		}
	}
}
