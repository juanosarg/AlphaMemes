using System;
using Verse;
using RimWorld;
using System.Linq;
using Verse.Sound;

namespace AlphaMemes
{
	public class CompAbilityAnalyzeCreature : CompAbilityEffect
	{
		public new CompProperties_AbilityAnalyzeCreature Props
		{
			get
			{
				return (CompProperties_AbilityAnalyzeCreature)this.props;
			}
		}

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Thing as Pawn;
			Building_AnimalDatabase building = null;
			
			if (pawn != null && pawn.RaceProps.Animal && !pawn.RaceProps.Dryad && !pawn.Dead)
			{
				if (this.parent.pawn.Ideo?.HasMeme(InternalDefOf.AM_BiologicalReconstructors) != true)
				{
					Messages.Message("AM_AbilityNeedsMeme".Translate(), MessageTypeDefOf.RejectInput, true);
					this.parent.StartCooldown(30);
					return;
				}
				bool existingDatabase = false;
				foreach (Map existingMap in Current.Game.Maps)
				{
					if (existingMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_AnimalDatabase).Count() > 0)
					{
						building = (Building_AnimalDatabase)existingMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_AnimalDatabase).First();
						existingDatabase = true;

					}
				}
				
				if (existingDatabase && building!=null)
                {
                    if (building.analyzedAnimalList.Contains(pawn.kindDef))
                    {
						Messages.Message("AM_AnimalAlreadyScanned".Translate(), MessageTypeDefOf.RejectInput, true);
						this.parent.StartCooldown(30);
					}
                    else {
						Messages.Message("AM_AnimalScanned".Translate(pawn.LabelCap), MessageTypeDefOf.SilentInput, true);
						building.analyzedAnimalList.Add(pawn.kindDef);
					}

					

                }
                else
                {
					Messages.Message("AM_NoDataBaseFound".Translate(), MessageTypeDefOf.RejectInput, true);
					this.parent.StartCooldown(30);

				}


			}
			else
			{
				Messages.Message("AM_AbilityNeedsAValidAnimal".Translate(), MessageTypeDefOf.RejectInput, true);
				this.parent.StartCooldown(30);
			}



			base.Apply(target, dest);

		}
	}
}
