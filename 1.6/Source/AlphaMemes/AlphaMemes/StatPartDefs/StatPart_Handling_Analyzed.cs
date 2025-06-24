using System;
using RimWorld;
using System.Reflection;
using Verse;
using System.Linq;


namespace AlphaMemes
{
    public class StatPart_Handling_Analyzed : StatPart
    {
        public float offset;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (Applies(req))
            {
                val += offset;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (Applies(req))
            {
                return "AM_StatsReport_Analyzed".Translate() + ": " + offset.ToStringWithSign();
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