using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;


namespace AlphaMemes
{
	public class RitualOutcomeEffectWorker_Tantrum : RitualOutcomeEffectWorker_FromQuality
	{



		public RitualOutcomeEffectWorker_Tantrum()
		{
		}

		public RitualOutcomeEffectWorker_Tantrum(RitualOutcomeEffectDef def) : base(def)
		{
		}

		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return true;
			}
		}
        protected override bool OutcomePossible(OutcomeChance chance, LordJob_Ritual ritual)
        {
			if (chance.positivityIndex == -2)
            {
				var data = compDatas.First(x => x is RitualOutcomeComp_TantrumData) as RitualOutcomeComp_TantrumData;
				float wealth = data.colonyWealthBefore - ritual.Map.wealthWatcher.WealthBuildings;
				if (wealth / data.colonyWealthBefore > 0.05f)
				{
					return false;
				}
			}
			return base.OutcomePossible(chance, ritual);
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
			extraOutcomeDesc = null;
			var data = compDatas.First(x => x is RitualOutcomeComp_TantrumData) as RitualOutcomeComp_TantrumData;
			float wealth = data.colonyWealthBefore - jobRitual.Map.wealthWatcher.WealthBuildings;
			extraOutcomeDesc = "AM_TantrumDestoryed".Translate(string.Join(", ",data.destoryedThings.Select(x=>x.LabelCap)), wealth.ToStringMoney());
		}
       
        protected override void ApplyDevelopmentPoints(Precept_Ritual ritual, OutcomeChance outcome, out string extraOutcomeDesc)
        {
			if (((ritual != null) ? ritual.ideo : null) != null && ritual.ideo.Fluid)
			{
				int outcomeIndex = this.def.outcomeChances.IndexOf(outcome);
				int developmentPoints;
				if (outcomeIndex >= 0 && ritual.ideo.development.TryGainDevelopmentPointsForRitualOutcome(ritual, outcomeIndex, out developmentPoints))
				{
					if (developmentPoints > 0)
					{
						ritual.ideo.development.TryAddDevelopmentPoints(developmentPoints);
						developmentPoints += developmentPoints;
						extraOutcomeDesc = "RitualOutcomeExtraDesc_DevelopmentPointsAwarded".Translate(ritual.ideo.development.Points - developmentPoints, ritual.ideo.development.Points, developmentPoints.ToStringWithSign());
						//Doubling up for this cause fresh start XD						
						return;
					}
					extraOutcomeDesc = "RitualOutcomeExtraDesc_NoDevelopmentPointsAwarded".Translate();
					return;
				}
			}
			extraOutcomeDesc = null;

		}


    }
}
