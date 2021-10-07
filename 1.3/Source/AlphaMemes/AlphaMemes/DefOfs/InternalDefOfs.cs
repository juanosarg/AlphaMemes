using System;
using RimWorld;
using Verse;

namespace AlphaMemes
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static PreceptDef AM_Violence_Abhorrent_Strict;

		public static PreceptDef AM_ArtProductionSpeed_Increased;

		public static HistoryEventDef AM_SomeoneDied;

		public static ThingDef AM_TrapBlunt;

		public static MemeDef AM_Madness;

		public static HediffDef AM_CatharsisHediff;


	}
}
