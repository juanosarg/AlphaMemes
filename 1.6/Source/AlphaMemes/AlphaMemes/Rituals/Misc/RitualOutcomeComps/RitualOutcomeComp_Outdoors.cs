
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AlphaMemes
{
    public class RitualOutcomeComp_Outdoors : RitualOutcomeComp_QualitySingleOffset
    {
        public override bool DataRequired => false;

        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        [MustTranslate]
        public string labelNotMet;

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Room room = ritual.Spot.GetRoom(ritual.Map);
            if (room != null)
            {
                return (room.PsychologicallyOutdoors) ? 1 : 0;
            }
            return 0f;
        }
        public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
        {
            return ((Count(ritual, data) > 0f) ? label : labelNotMet).CapitalizeFirst().Formatted() + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(qualityOffset.ToStringPercent()) + ".";
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            float quality = 0f;
            bool flag = false;
            if (ritualTarget.Map != null)
            {
                Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
                flag = room != null && room.PsychologicallyOutdoors;
                quality = (flag ? qualityOffset : 0f);
            }
            return new QualityFactor
            {
                label = flag ? label.CapitalizeFirst() : labelNotMet.CapitalizeFirst(),
                qualityChange = ExpectedOffsetDesc(positive: true, quality),
                quality = quality,
                present = flag,
                positive = true,
                priority = 0f
            };
        }
    }
}