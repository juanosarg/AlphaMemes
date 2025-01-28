
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AlphaMemes
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {

        //This static class stores lists of animals and pawns for different things.

        public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("UI/Rituals/AM_Lasso");

        public static Dictionary<Map, int> artInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, float> artBeautyInTheMap = new Dictionary<Map, float>();

        public static Dictionary<Map, int> reliquariesInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, int> deathrestCasketsInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, int> draincasketsInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, int> megalithsInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, int> penMarkersInMap = new Dictionary<Map, int>();

        public static Dictionary<Map, int> bathsAndShowersInTheMap = new Dictionary<Map, int>();

        public static Dictionary<Map, bool> mapAndWateriness = new Dictionary<Map, bool>();

        public static Dictionary<Map, int> animaTreesInMap = new Dictionary<Map, int>();

        public static List<PawnKindDef> analyzedAnimals = new List<PawnKindDef>();

        public static List<PawnKindDef> cattleAnimals = new List<PawnKindDef>();

        public static HashSet<ThingDef> normalTeas = new HashSet<ThingDef>();

        public static HashSet<ThingDef> specialtyTeas = new HashSet<ThingDef>();

        public static float databaseCompletion = 0;

        public static List<PawnKindDef> utilityDryads = new List<PawnKindDef>() { PawnKindDefOf.Dryad_Basic, InternalDefOf.Dryad_Woodmaker,
            InternalDefOf.Dryad_Berrymaker, InternalDefOf.Dryad_Medicinemaker, PawnKindDefOf.Dryad_Gaumaker, InternalDefOf.Dryad_Carrier,InternalDefOf.AM_UnshackledDryad,InternalDefOf.AM_Dryad_Ocular,
            InternalDefOf.AM_Dryad_Corruptor,InternalDefOf.AM_Dryad_Tumorous,InternalDefOf.VDE_Dryad_Stonedigger,InternalDefOf.VDE_Dryad_Gaubricmaker,
            InternalDefOf.VDE_Dryad_Nectarmaker,InternalDefOf.VDE_AwakenedDryad_Carrier,InternalDefOf.VDE_AwakenedDryad_Woodmaker,InternalDefOf.VDE_AwakenedDryad_Medicinemaker
            ,InternalDefOf.VDE_AwakenedDryad_Berrymaker,InternalDefOf.VDE_AwakenedDryad_Stonedigger,InternalDefOf.VDE_AwakenedDryad_Nectarmaker,InternalDefOf.VDE_AwakenedDryad_Gaubricmaker
    };

        public static List<PawnKindDef> combatDryads = new List<PawnKindDef>() { InternalDefOf.Dryad_Clawer, InternalDefOf.Dryad_Barkskin, InternalDefOf.AM_Dryad_Spitter,
            InternalDefOf.AM_Dryad_Unstable,InternalDefOf.VDE_Dryad_Spitter,InternalDefOf.VDE_AwakenedDryad_Clawer,InternalDefOf.VDE_AwakenedDryad_Barkskin,InternalDefOf.VDE_AwakenedDryad_Spitter};

        public static List<MemeDef> listReligiousMemes = new List<MemeDef>() { InternalDefOf.AM_Structure_Jainism,InternalDefOf.AM_Structure_Jewish, InternalDefOf.AM_Structure_Kemetism, InternalDefOf.AM_Structure_Sikhism,
        InternalDefOf.Structure_Animist,InternalDefOf.Structure_Archist,InternalDefOf.Structure_OriginBuddhist,InternalDefOf.Structure_OriginChristian,InternalDefOf.Structure_OriginHindu,InternalDefOf.Structure_OriginIslamic,
        InternalDefOf.Structure_TheistAbstract,InternalDefOf.Structure_TheistEmbodied,InternalDefOf.AM_Structure_Neolithic, InternalDefOf.VME_Structure_Serketist
            ,InternalDefOf.VME_Structure_ChthonianCult,InternalDefOf.VME_Structure_Pantheism,InternalDefOf.VME_Structure_Omnism,InternalDefOf.VME_Structure_Shintaoism,InternalDefOf.VME_GodEmperor};

        public static List<MemeDef> listProselytizerMemes = new List<MemeDef>() { InternalDefOf.Proselytizer, InternalDefOf.VME_ViolentConversion };

        public static List<DesignatorDropdownGroupDef> designatorsToBeRemoved = new List<DesignatorDropdownGroupDef>() { InternalDefOf.AM_Floor_JewishTiles, InternalDefOf.AM_Floor_JewishFineTiles, InternalDefOf.AM_Floor_KemeticTiles, InternalDefOf.AM_Floor_KemeticFineTiles, InternalDefOf.AM_Floor_SteampunkTiles,
        InternalDefOf.AM_Floor_NeolithicTiles};


        static StaticCollections()
        {

            List<PawnKindDef> allMilkCattle = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x=>x.race.HasComp<CompMilkable>()).ToList();
            cattleAnimals.AddRange(allMilkCattle);
            cattleAnimals.Add(PawnKindDefOf.Muffalo);
            cattleAnimals.Add(InternalDefOf.Bison);

            HashSet<TeaDefs> allTeaLists = DefDatabase<TeaDefs>.AllDefsListForReading.ToHashSet();
            foreach (TeaDefs individualList in allTeaLists)
            {
                foreach(string normalTea in individualList.normalTeas)
                {
                    ThingDef normalTeaDef = DefDatabase<ThingDef>.GetNamedSilentFail(normalTea);
                    if (normalTeaDef != null)
                    {
                        normalTeas.Add(normalTeaDef);
                    }
                }
                foreach (string specialtyTea in individualList.specialtyTeas)
                {
                    ThingDef specialtyTeaDef = DefDatabase<ThingDef>.GetNamedSilentFail(specialtyTea);
                    if (specialtyTeaDef != null)
                    {
                        specialtyTeas.Add(specialtyTeaDef);
                    }
                }      
            }
        }

        public static void SetArtInTheMap(Map map, int art)
        {
            artInTheMap[map] = art;
        }
        public static void SetArtBeautyInTheMap(Map map, float artbeauty)
        {
            artBeautyInTheMap[map] = artbeauty;
        }
        public static void SetReliquariesInTheMap(Map map, int reliquaries)
        {
            reliquariesInTheMap[map] = reliquaries;
        }
        public static void SetDeathrestCasketsInTheMap(Map map, int caskets)
        {
            deathrestCasketsInTheMap[map] = caskets;
        }
        public static void SetDraincasketsInTheMap(Map map, int caskets)
        {
            draincasketsInTheMap[map] = caskets;
        }
        public static void SetMegalithsInTheMap(Map map, int megaliths)
        {
            megalithsInTheMap[map] = megaliths;
        }
        public static void SetBathsAndShowersInTheMap(Map map, int bathsAndShowers)
        {
            bathsAndShowersInTheMap[map] = bathsAndShowers;
        }
        public static void SetPenMarkersInTheMap(Map map, int pens)
        {
            penMarkersInMap[map] = pens;
        }
        public static void SetAnimaTreesInTheMap(Map map, int trees)
        {
            animaTreesInMap[map] = trees;
        }
        public static void SetMapWateriness(Map map, bool iswatery)
        {
            mapAndWateriness[map] = iswatery;
        }

        public static void AddUtilityDryad(PawnKindDef pawn)
        {
            if (pawn != null && !utilityDryads.Contains(pawn))
            {
                utilityDryads.Add(pawn);
            }

        }


        public static void AddCombatDryad(PawnKindDef pawn)
        {
            if (pawn != null && !combatDryads.Contains(pawn))
            {
                combatDryads.Add(pawn);
            }

        }

    }
}
