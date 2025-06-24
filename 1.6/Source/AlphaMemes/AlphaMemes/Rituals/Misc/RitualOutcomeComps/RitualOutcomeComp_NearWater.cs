
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AlphaMemes
{
    public class RitualOutcomeComp_NearWater : RitualOutcomeComp_QualitySingleOffset
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
            int waterNumber = WaterTiles(ritual.Spot,ritual.Map, 10);

            if (waterNumber>20)
            {
                return 1;
            }

            return 0f;
        }

        public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
        {
            return ((Count(ritual, data) > 0f) ? label : labelNotMet).CapitalizeFirst().Formatted() + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(qualityOffset.ToStringPercent()) + ".";
        }

        public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return (Count(ritual, data) > 0f) ? qualityOffset : 0;
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            float quality = 0f;
            bool flag = false;
            if (ritualTarget.Map != null)
            {

                flag = WaterTiles(ritualTarget.Cell,ritualTarget.Map, 10)>20;
               
                quality = (flag ? qualityOffset : 0f);
            }
            return new QualityFactor
            {
                label = flag ? label.CapitalizeFirst(): labelNotMet.CapitalizeFirst(),
                qualityChange = ExpectedOffsetDesc(positive: true, quality),
                quality = quality,
                present = flag,
                positive = flag,
                priority = 0f
            };
        }

        public int WaterTiles(IntVec3 ritualPos, Map map,float radius)
        {
            int num = GenRadial.NumCellsInRadius(radius);
            int waterNumber = 0;
            for (int i = 0; i < num; i++)
            {
                IntVec3 c = ritualPos + GenRadial.RadialPattern[i];
                if (c.InBounds(map))
                {
                    TerrainDef terrain = c.GetTerrain(map);

                    if (terrain != null && terrain.IsWater)
                    {
                        waterNumber++;

                    }

                }

            }

            return waterNumber;

        }
    }
}