using System;
using RimWorld;
using System.Reflection;
using Verse;
using System.Linq;


namespace AlphaMemes
{
    public class StatPart_Handling_HorsesPrecept : StatPart
    {
        public float offset = -2;

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
                return "AM_StatsReport_HorseDesired".Translate() + ": " + offset.ToStringWithSign();
            }
            return null;
        }

        private bool Applies(StatRequest req)
        {
            Pawn pawn = (req.Thing as Pawn);
            return (pawn.kindDef == InternalDefOf.Horse && Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Horses_Desired) != null);

        }
    }
}