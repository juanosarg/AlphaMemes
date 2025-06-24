using System;
using RimWorld;
using System.Reflection;
using Verse;


namespace AlphaMemes
{
    public class StatPart_MeditationFocusStrength : StatPart
    {
        float meditationFactor = 0.75f;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.HasThing && Applies(req.Thing))
            {
                val *= meditationFactor;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && Applies(req.Thing))
            {
                
                return "AM_MeditationFocusStrengthFactor".Translate() + ": x" + meditationFactor.ToStringPercent();
            }
            return null;
        }

        public static bool Applies(Thing th)
        {
            return Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_PsyfocusGain_Dampened) != null;
        }
    }
}
