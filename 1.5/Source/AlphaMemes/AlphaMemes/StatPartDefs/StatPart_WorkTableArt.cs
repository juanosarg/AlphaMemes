using System;
using RimWorld;
using System.Reflection;
using Verse;


namespace AlphaMemes
{
	public class StatPart_WorkTableArt : StatPart
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
				float artWorkTableWorkSpeedFactor = 1.25f;
				return "AM_ArtTableExtraSpeed".Translate() + ": x" + artWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		public static bool Applies(Thing th)
		{

			bool isArtTable = (th.def.defName== "TableSculpting");
			return isArtTable && Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_ArtProductionSpeed_Increased) != null;
		}
	}
}
