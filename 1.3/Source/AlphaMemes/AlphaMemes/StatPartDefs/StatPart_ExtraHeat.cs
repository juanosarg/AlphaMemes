using System;
using RimWorld;
using System.Reflection;
using Verse;


namespace AlphaMemes
{
	public class StatPart_ExtraHeat : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && Applies(req.Thing))
			{
				val *= 1.25f;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && Applies(req.Thing))
			{
				float extraArmorFactor = 1.25f;
				return "AM_ExtraHeatArmorFactor".Translate() + ": x" + extraArmorFactor.ToStringPercent();
			}
			return null;
		}

		public static bool Applies(Thing th)
		{
			Apparel apparel = th as Apparel;

			return apparel != null && apparel.Wearer?.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Heat) == true;
		}
	}
}
