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

				List<ThingDefCountClass> materialsNeeded = blueprint.TotalMaterialCost();

				bool resourcesDelivered = true;
				for (int i = 0; i < materialsNeeded.Count; i++)
				{
					ThingDefCountClass material = materialsNeeded[i];
					int numMaterial = material.count;
					int materialsDelivered = blueprint.resourceContainer.TotalStackCountOfDef(material.thingDef);
					if(numMaterial!= materialsDelivered)
					{
						resourcesDelivered = false;

                    }


					
				}
				
				if (resourcesDelivered)
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
