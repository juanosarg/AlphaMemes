using System;
using RimWorld;
using System.Reflection;
using Verse;
using System.Linq;


namespace AlphaMemes
{
    public class StatPart_CaravanRidingSpeedFactor_HorsesPrecept : StatPart
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
                return "AM_StatsReport_HorseDesired".Translate() + ": " + factor.ToStringPercentEmptyZero();
            }
            return null;
        }

        private bool Applies(StatRequest req)
        {
            Pawn pawn = (req.Thing as Pawn);
            return (pawn?.Faction == Faction.OfPlayerSilentFail&&pawn.kindDef == InternalDefOf.Horse && Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Horses_Desired) != null);

        }
    }
}