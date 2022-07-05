using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace AlphaMemes
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static JobDef AM_InstallRelic;

		public static PreceptDef AM_Violence_Abhorrent_Strict;
		public static PreceptDef AM_ArtProductionSpeed_Increased;
		public static PreceptDef AM_Art_Desired;
		public static PreceptDef AM_ArtQuality_Expected;
		public static PreceptDef AM_AnimalDespised;
		public static PreceptDef AM_Bonding_Abhorrent;
		public static PreceptDef AM_Dryads_Enhanced;
		public static PreceptDef AM_Armour_Blunt;
		public static PreceptDef AM_Armour_Sharp;
		public static PreceptDef AM_Armour_Heat;
		public static PreceptDef AM_Mood_Volatile;
		public static PreceptDef AM_CombatProwess_Increased;
		public static PreceptDef AM_CombatProwess_Melee;
		public static PreceptDef AM_Reliquaries_Forbidden;
		public static PreceptDef AM_Barracks_Preferred;
		public static PreceptDef AM_Megaliths_Desired;
		public static PreceptDef AM_Barracks_PreferredTrue;

		public static HistoryEventDef AM_SomeoneDied;
		public static HistoryEventDef AM_HarvestedNonColonistOrgan;
		public static HistoryEventDef AM_DespisedAnimalDied;
		public static HistoryEventDef AM_AnimalDied;
		public static HistoryEventDef AM_TameAnimal;
		public static HistoryEventDef AM_TrainAnimal;
		public static HistoryEventDef AM_AnimalReleased;
		public static HistoryEventDef AM_UsedMelee;
		public static HistoryEventDef AM_UsedRanged;
		public static HistoryEventDef AM_BuildingReliquary;

		public static ThingDef AM_TrapBlunt;
		public static ThingDef AM_AnimalDatabase;
		public static ThingDef AM_BlocksPristineLimestone;
		public static ThingDef AM_RelicSmashingAltar;
		public static ThingDef AM_GrogDispenser;
		public static ThingDef AM_Megalith;

		public static MemeDef AM_Madness;
		public static MemeDef AM_BiologicalDefilers;
		public static MemeDef AM_BiologicalReconstructors;
		public static MemeDef AM_Structure_Jewish;
		public static MemeDef AM_Structure_Jainism;
		public static MemeDef AM_Structure_Sikhism;
		public static MemeDef AM_Structure_Kemetism;
		public static MemeDef Structure_Animist;
		public static MemeDef Structure_TheistEmbodied;
		public static MemeDef Structure_TheistAbstract;
		public static MemeDef Structure_Archist;
		public static MemeDef Structure_OriginChristian;
		public static MemeDef Structure_OriginIslamic;
		public static MemeDef Structure_OriginHindu;
		public static MemeDef Structure_OriginBuddhist;
		public static MemeDef AM_Iconoclast;
		public static MemeDef Proselytizer;
		public static MemeDef AM_Structure_Neolithic;

		public static ThingStyleDef AM_MealNutrientPaste;

		public static HediffDef AM_CatharsisHediff;
		public static HediffDef AM_UtilityDryadHediff;
		public static HediffDef AM_CombatDryadHediff;
		public static HediffDef AM_GenericDryadHediff;
		public static HediffDef AM_IconoclastHediff;

		public static SoundDef AM_RitualSustainer_MaddeningChant;

		public static ThoughtDef AM_SleptInBarracksMonastic;
		public static ThoughtDef AM_SleptInPrivateRoomMonastic;
		public static ThoughtDef AM_SleptInBarracksPreferred;
		public static ThoughtDef AM_SleptInPrivateRoomPreferred;

		public static PawnKindDef Dryad_Woodmaker;
		public static PawnKindDef Dryad_Berrymaker;
		public static PawnKindDef Dryad_Medicinemaker;
		public static PawnKindDef Dryad_Carrier;		
		public static PawnKindDef Dryad_Clawer;
		public static PawnKindDef Dryad_Barkskin;
		public static PawnKindDef AM_UnshackledDryad;

		public static DesignatorDropdownGroupDef AM_Floor_JewishTiles;
		public static DesignatorDropdownGroupDef AM_Floor_JewishFineTiles;
		public static DesignatorDropdownGroupDef AM_Floor_KemeticTiles;
		public static DesignatorDropdownGroupDef AM_Floor_KemeticFineTiles;
		public static DesignatorDropdownGroupDef AM_Floor_SteampunkTiles;
		public static DesignatorDropdownGroupDef AM_Floor_NeolithicTiles;

		[MayRequireRoyalty]
		public static ThoughtDef AM_AnimaScreamLesser;
		[MayRequireRoyalty]
		public static JobDef AM_AnimaBurialLink;
		[MayRequireRoyalty]
		public static ThoughtDef AM_DeathKnellThought;

		[MayRequireRoyalty]
		public static PreceptDef AM_Abilities_DeathKnell;
		//Funeral stuff
		public static JobDef AM_DeliverCorpseToCell;
		public static JobDef AM_DeliverStuffToCell;
		public static JobDef AM_LoadCorpseToThing;
		public static JobDef AM_DeliverThingsToCell;
		public static ThingDef AM_MummyMale;
		public static ThingDef AM_MummyFemale;
	}
}
