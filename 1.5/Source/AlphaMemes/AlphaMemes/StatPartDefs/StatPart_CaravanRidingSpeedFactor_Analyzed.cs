using System;
using RimWorld;
using System.Reflection;
using Verse;
using System.Linq;


namespace AlphaMemes
{
    public class StatPart_CaravanRidingSpeedFactor_Analyzed : StatPart
    {
        public const float factor = 1.25f;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (Applies(req))
            {
                val *= factor;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (Applies(req))
            {
                return "AM_StatsReport_Analyzed".Translate() + ": " + factor.ToStringPercentEmptyZero();
            }
            return null;
        }

        private bool Applies(StatRequest req)
        {
            PawnKindDef pawn = (req.Thing as Pawn)?.kindDef;
            if (pawn != null)
            {
                return StaticCollections.analyzedAnimals.Contains(pawn);
            }
            return false;

        }
    }
}