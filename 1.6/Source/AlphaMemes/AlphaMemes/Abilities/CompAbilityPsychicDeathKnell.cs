﻿using System;
using Verse;
using RimWorld;
using System.Linq;
using Verse.Sound;

namespace AlphaMemes
{
	public class CompAbilityPsychicDeathKnell : CompAbilityEffect
	{
		public new CompProperties_AbilityPsychicDeathKnell Props
		{
			get
			{
				return (CompProperties_AbilityPsychicDeathKnell)this.props;
			}
		}

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Thing as Pawn;
			Pawn caster = this.parent.pawn;

			if (pawn != null && caster!= null && pawn.Downed)
			{
                if (this.parent.pawn.Ideo?.HasMeme(InternalDefOf.AM_PsychicVampirism) != true)
                {
					Messages.Message("AM_AbilityNeedsMeme".Translate(), MessageTypeDefOf.RejectInput, true);
					this.parent.StartCooldown(30);
					return;
				}
				caster.needs.mood.thoughts.memories.TryGainMemory(InternalDefOf.AM_DeathKnellThought,null,caster.ideo.Ideo.GetPrecept(InternalDefOf.AM_Abilities_DeathKnell));
				if (caster.health != null)
				{
					if (Utils.GetInjuriesTendable(caster.health.hediffSet.hediffs) != null && Utils.GetInjuriesTendable(caster.health.hediffSet.hediffs).Count<Hediff_Injury>() > 0)
					{
						foreach (Hediff_Injury injury in Utils.GetInjuriesTendable(caster.health.hediffSet.hediffs))
						{
							injury.Severity = injury.Severity - 0.5f;
							break;
						}
					}
				}
				if (caster.HasPsylink)
				{
					caster.psychicEntropy.OffsetPsyfocusDirectly(1);
					caster.psychicEntropy.TryAddEntropy(caster.psychicEntropy.MaxPotentialEntropy*0.3f,null,true,true);
				}
				
				for (int i = 0; i < 20; i++)
				{
					IntVec3 c;
					CellFinder.TryFindRandomReachableCellNearPosition(pawn.Position, pawn.Position, pawn.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
					FilthMaker.TryMakeFilth(c, pawn.Map, ThingDefOf.Filth_AmnioticFluid);

				}
				InternalDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
				pawn.Kill(null);
			}
			else
			{
				Messages.Message("AM_AbilityNeedsADownedPawn".Translate(), MessageTypeDefOf.RejectInput, true);
				this.parent.StartCooldown(30);
			}



			base.Apply(target, dest);

		}
	}
}
