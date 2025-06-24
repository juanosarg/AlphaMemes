using System;
using RimWorld;
using System.Reflection;
using Verse;


namespace AlphaMemes
{
	public class StatPart_CreepBeauty : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			float offset;
			if (req.BuildableDef == InternalDefOf.VFEI2_Creep && Applies(req.BuildableDef, out offset))
			{
				val += offset;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
            float offset;
            if (req.BuildableDef == InternalDefOf.VFEI2_Creep && Applies(req.BuildableDef, out offset))
            {			
				return "AM_CreepPreceptModifier".Translate() + ": " + offset.ToStringWithSign();
            }
			return null;
		}

		public static bool Applies(BuildableDef th, out float offset)
		{
			
			bool dontcare = Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Creep_DontCare) != null;
            bool sublime = Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Creep_Sublime) != null;
			if (dontcare)
			{
				offset = 1;
			}else if (sublime)
			{
                offset = 4;
            }
            else offset = 0;
            return (dontcare|| sublime);
		}
	}
}
