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

		public static PreceptDef AM_AnimalDespised;

		public static PreceptDef AM_Bonding_Abhorrent;

		public static HistoryEventDef AM_SomeoneDied;
		public static HistoryEventDef AM_HarvestedNonColonistOrgan;
		public static HistoryEventDef AM_DespisedAnimalDied;
		public static HistoryEventDef AM_AnimalDied;
		public static HistoryEventDef AM_TameAnimal;
		public static HistoryEventDef AM_TrainAnimal;

		public static ThingDef AM_TrapBlunt;

		public static MemeDef AM_Madness;

		public static MemeDef AM_BiologicalDefilers;

		public static HediffDef AM_CatharsisHediff;

		public static SoundDef AM_RitualSustainer_MaddeningChant;


	}
}
