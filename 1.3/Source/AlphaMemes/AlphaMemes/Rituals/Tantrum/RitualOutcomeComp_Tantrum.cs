using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_Tantrum : RitualOutcomeComp_Quality
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_TantrumData();
        }
        public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
        {
            base.Tick(ritual, data, progressAmount);
            var tantrumData = data as RitualOutcomeComp_TantrumData;
            foreach (Thing thing in ritual.usedThings)
            {
                if (thing.Destroyed && !tantrumData.destoryedThings.Contains(thing))
                {
                    tantrumData.destoryedThings.Add(thing);
                }
            }
        }
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            var tantrumData = data as RitualOutcomeComp_TantrumData;
            float wealth = tantrumData.colonyWealthBefore - ritual.Map.wealthWatcher.WealthBuildings;
            float percentDestroy = wealth / tantrumData.colonyWealthBefore;            
            return curve.Evaluate(percentDestroy);
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            var tantrumData = data as RitualOutcomeComp_TantrumData;
            tantrumData.colonyWealthBefore = ritualTarget.Map.wealthWatcher.WealthBuildings;
            return new ExpectedOutcomeDesc()
            {
                label = "AM_TantrumOutcomeComp".Translate(),
                uncertainOutcome = true,
                present = false,
                effect = "?",
                quality = 1f,
                positive = true,
            };
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }

        




    }
}
