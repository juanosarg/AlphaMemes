using System;
using RimWorld;
using System.Reflection;
using Verse;
using System.Linq;


namespace AlphaMemes
{
    public class StatPart_FilthRate_Analyzed : StatPart
    {
        public const float factor = 0.75f;

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
            Pawn pawn = req.Thing as Pawn;

            if (pawn?.Faction == Faction.OfPlayerSilentFail)
            {
                return StaticCollections.analyzedAnimals.Contains(pawn.kindDef);
            }
            return false;

        }
    }
}