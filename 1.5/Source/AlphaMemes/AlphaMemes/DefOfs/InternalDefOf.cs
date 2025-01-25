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

        public static DamageDef AM_AcidSpit;
        public static DamageDef AM_HolyBurn;

        public static SoundDef AM_Rodeo;
        public static SoundDef AM_Rodeo_Yeehaw;
        public static SoundDef RawVegetable_Eat;

        public static EffecterDef EatVegetarian;

        [MayRequire("VanillaExpanded.VMemesE")]
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
        public static PreceptDef AM_NutrientPasteEating_Forbidden;
        public static PreceptDef AM_SkyBurial;
        public static PreceptDef AM_Hydroagriculture_Revered;
        public static PreceptDef AM_GauranlenConnection_Forbidden;
        public static PreceptDef AM_RoughLiving_Disliked;
        public static PreceptDef AM_Ranching_CattleCentered;
        public static PreceptDef AM_Horses_Desired;
        public static PreceptDef AM_Cattle_Improved;
        public static PreceptDef AM_TableQuality_Desired;
        [MayRequire("Sarg.AlphaPrefabs")]
        public static PreceptDef AM_PrefabBuying_Preferred;
        [MayRequire("Sarg.AlphaPrefabs")]
        public static PreceptDef AM_PrefabAcquisition_Easy;
        [MayRequireRoyalty]
        public static PreceptDef AM_Abilities_DeathKnell;
        [MayRequireRoyalty]
        public static PreceptDef AM_PsyfocusGain_Dampened;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static PreceptDef AM_Creep_DontCare;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static PreceptDef AM_Creep_Sublime;
        [MayRequireBiotech]
        public static PreceptDef AM_MaternalMortality_Nullified;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static PreceptDef AM_HuntFocus_Strigoi;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static PreceptDef AM_HuntFocus_Ekkimian;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static PreceptDef AM_HuntFocus_Bruxa;
        [MayRequireBiotech]
        public static PreceptDef AM_HuntFocus_Sanguophage;
        [MayRequireBiotech]
        public static PreceptDef AM_DeathrestCaskets_Abhorrent;
        [MayRequireBiotech]
        public static PreceptDef AM_SanguophageCamps_RaidingDesired;
        [MayRequire("Dubwise.DubsBadHygiene")]
        public static PreceptDef AM_Baths_Desired;

        public static HistoryEventDef AM_SomeoneDied;
        public static HistoryEventDef AM_HarvestedNonColonistOrgan;
        public static HistoryEventDef AM_DespisedAnimalDied;
        public static HistoryEventDef AM_AnimalDied;
        public static HistoryEventDef AM_TameAnimal;
        public static HistoryEventDef AM_TrainAnimal;
        public static HistoryEventDef AM_AnimalReleased;
        public static HistoryEventDef AM_AnimalAnalyzedAndReleased;
        public static HistoryEventDef AM_UsedMelee;
        public static HistoryEventDef AM_UsedRanged;
        public static HistoryEventDef AM_BuildingReliquary;
        public static HistoryEventDef AM_PruneGauranlenTree;
        public static HistoryEventDef AM_CutGauranlenTree;
        public static HistoryEventDef AM_SleptInWoodenBed;
        public static HistoryEventDef AM_AteInWoodenTable;
        public static HistoryEventDef AM_AteWithoutATable;
        public static HistoryEventDef AM_ColonyHorseDied;
        public static HistoryEventDef AM_AteInLowQualityTable;
        public static HistoryEventDef AM_DestroyedARelic;
        [MayRequire("Sarg.AlphaPrefabs")]
        public static HistoryEventDef AM_BoughtPrefabOnMerchant;
        [MayRequireBiotech]
        public static HistoryEventDef AM_BuildingDeathrestCasket;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static HistoryEventDef AM_BuildingDrainCasket;
        [MayRequireBiotech]
        public static HistoryEventDef AM_SanguophageCampRaided;
        [MayRequireBiotech]
        public static HistoryEventDef AM_ChildBorn;
        [MayRequireRoyalty]
        public static HistoryEventDef AM_AnimaScream;
        [MayRequireAnomaly]
        public static HistoryEventDef AM_CutHarbingerTree;
        [MayRequire("VanillaExpanded.VCookE")]
        public static HistoryEventDef AM_AteSimpleMeal;
        [MayRequire("VanillaExpanded.VCookE")]
        public static HistoryEventDef AM_AteRawFood;
        [MayRequire("VanillaExpanded.VCookE")]
        public static HistoryEventDef AM_AteLavishMeal;

        public static ThingDef AM_TrapBlunt;
        public static ThingDef AM_AnimalDatabase;
        public static ThingDef AM_BlocksPristineLimestone;
        public static ThingDef AM_RelicSmashingAltar;
        public static ThingDef AM_GrogDispenser;
        public static ThingDef AM_Megalith;
        public static ThingDef AM_Sphynx;
        public static ThingDef AM_GreatPyramid;
        public static ThingDef AM_CorpseRumBasic;
        public static ThingDef AM_CorpseRumFine;
        public static ThingDef AM_Plant_CorruptedPodGauranlen;
        public static ThingDef AM_CorruptedGaumakerCocoon;
        public static ThingDef AM_MummyMale;
        public static ThingDef AM_MummyFemale;
        public static ThingDef AM_Filth_RedSlime;
        public static ThingDef PenMarker;
        [MayRequire("sarg.alphaanimals")]
        public static ThingDef AA_AlienTree;
        [MayRequire("sarg.alphaanimals")]
        public static ThingDef AA_DisgustingMealNutrientPaste;
        [MayRequire("sarg.alphaanimals")]
        public static ThingDef AA_AlienGrass;
        [MayRequire("sarg.alphaanimals")]
        public static ThingDef AA_RedLeaves;
        [MayRequire("sarg.alphaanimals")]
        public static ThingDef AA_RedPlantsTall;
        [MayRequire("sarg.alphabiomes")]
        public static ThingDef AB_AlienTree;
        [MayRequire("sarg.alphabiomes")]
        public static ThingDef AB_AlienGrass;
        [MayRequire("sarg.alphabiomes")]
        public static ThingDef AB_RedLeaves;
        [MayRequire("sarg.alphabiomes")]
        public static ThingDef AB_RedPlantsTall;
        public static ThingDef TableSculpting;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static ThingDef VRE_Draincasket;
        [MayRequire("Dubwise.DubsBadHygiene")]
        public static ThingDef BathtubStuff;
        [MayRequire("Dubwise.DubsBadHygiene")]
        public static ThingDef ShowerStuff;
        [MayRequire("Dubwise.DubsBadHygiene")]
        public static ThingDef ShowerSimple;
        [MayRequire("Dubwise.DubsBadHygiene")]
        public static ThingDef ShowerAdvStuff;

        public static FleckDef PsycastPsychicEffect;

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
        public static MemeDef AM_BiologicalCorruptors;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_Structure_Serketist;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_Structure_ChthonianCult;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_Structure_Pantheism;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_Structure_Omnism;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_Structure_Shintaoism;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_GodEmperor;
        [MayRequire("VanillaExpanded.VMemesE")]
        public static MemeDef VME_ViolentConversion;
        [MayRequireRoyalty]
        public static MemeDef AM_PsychicVampirism;

        //public static ThingStyleDef AM_MealNutrientPaste;

        public static HediffDef AM_CatharsisHediff;
        public static HediffDef AM_UtilityDryadHediff;
        public static HediffDef AM_CombatDryadHediff;
        public static HediffDef AM_GenericDryadHediff;
        public static HediffDef AM_IconoclastHediff;
        public static HediffDef AM_Kamikaze;
        [MayRequire("VanillaExpanded.VCookE")]
        public static HediffDef AM_FeastingFrenzy;

        public static SoundDef AM_RitualSustainer_MaddeningChant;
        public static SoundDef Hive_Spawn;
        public static SoundDef AM_GooPop;

        public static ThoughtDef AM_SleptInBarracksMonastic;
        public static ThoughtDef AM_SleptInPrivateRoomMonastic;
        public static ThoughtDef AM_SleptInBarracksPreferred;
        public static ThoughtDef AM_SleptInPrivateRoomPreferred;
        public static ThoughtDef AM_CorpseRumThought;
        public static ThoughtDef AteLavishMeal;
        [MayRequireRoyalty]
        public static ThoughtDef AM_AnimaScreamLesser;
        [MayRequireRoyalty]
        public static ThoughtDef AM_DeathKnellThought;
        [MayRequireRoyalty]
        public static ThoughtDef AnimaScream;

        public static PawnKindDef Dryad_Woodmaker;
        public static PawnKindDef Dryad_Berrymaker;
        public static PawnKindDef Dryad_Medicinemaker;
        public static PawnKindDef Dryad_Carrier;
        public static PawnKindDef Dryad_Clawer;
        public static PawnKindDef Dryad_Barkskin;
        public static PawnKindDef AM_UnshackledDryad;
        public static PawnKindDef AA_Eyeling;
        public static PawnKindDef AM_Dryad_Ocular;
        public static PawnKindDef AM_Dryad_Corruptor;
        public static PawnKindDef AM_Dryad_Spitter;
        public static PawnKindDef AM_Dryad_Unstable;
        public static PawnKindDef AM_Dryad_Tumorous;
        public static PawnKindDef Bison;
        public static PawnKindDef Horse;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_Dryad_Stonedigger;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_Dryad_Gaubricmaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_Dryad_Nectarmaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_Dryad_Spitter;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Carrier;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Woodmaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Medicinemaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Berrymaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Stonedigger;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Nectarmaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Gaubricmaker;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Clawer;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Barkskin;
        [MayRequire("VanillaExpanded.Ideo.Dryads")]
        public static PawnKindDef VDE_AwakenedDryad_Spitter;


        public static DesignatorDropdownGroupDef AM_Floor_JewishTiles;
        public static DesignatorDropdownGroupDef AM_Floor_JewishFineTiles;
        public static DesignatorDropdownGroupDef AM_Floor_KemeticTiles;
        public static DesignatorDropdownGroupDef AM_Floor_KemeticFineTiles;
        public static DesignatorDropdownGroupDef AM_Floor_SteampunkTiles;
        public static DesignatorDropdownGroupDef AM_Floor_NeolithicTiles;

        [MayRequireRoyalty]
        public static JobDef AM_AnimaBurialLink;
        public static JobDef AM_DeliverCorpseToCell;
        public static JobDef AM_DeliverStuffToCell;
        public static JobDef AM_LoadCorpseToThing;
        public static JobDef AM_DeliverThingsToCell;
        public static JobDef AM_TrantrumJob;
        public static JobDef AM_MergeIntoCorruptedPod;
        public static JobDef AM_InstallRelic;
        public static JobDef AM_RodeoFalseAttackJob;
        [MayRequire("VanillaExpanded.VCookE")]
        public static JobDef AM_EatAtBanquet;

        public static AbilityDef AM_ChangeStyleRadius;

        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static TerrainDef VFEI2_Creep;

        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static XenotypeDef VRE_Strigoi;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static XenotypeDef VRE_Ekkimian;
        [MayRequire("vanillaracesexpanded.sanguophage")]
        public static XenotypeDef VRE_Bruxa;

        [MayRequireBiotech]
        public static QuestScriptDef AM_OpportunitySite_SanguophageCamp;

    }
}
