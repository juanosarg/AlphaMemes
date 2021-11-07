
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace AlphaMemes
{

    public static class StaticCollectionsClass
    {

        //This static class stores lists of animals and pawns for different things.



        public static int artInTheMap = 0;

        public static float databaseCompletion = 0;

        public static float artBeautyInTheMap = 0;

        public static float reliquariesInTheMap = 0;

        public static Dictionary<Pawn, int> colonist_and_random_mood = new Dictionary<Pawn, int>();

        public static List<PawnKindDef> utilityDryads = new List<PawnKindDef>() { PawnKindDefOf.Dryad_Basic, InternalDefOf.Dryad_Woodmaker, 
            InternalDefOf.Dryad_Berrymaker, InternalDefOf.Dryad_Medicinemaker, PawnKindDefOf.Dryad_Gaumaker, InternalDefOf.Dryad_Carrier,InternalDefOf.AM_UnshackledDryad};
        public static List<PawnKindDef> combatDryads = new List<PawnKindDef>() { InternalDefOf.Dryad_Clawer, InternalDefOf.Dryad_Barkskin };

        public static void AddColonistRandomMood(Pawn pawn, int mood)
        {
            if (!colonist_and_random_mood.ContainsKey(pawn))
            {
                colonist_and_random_mood.Add(pawn, mood);
            }
            else { colonist_and_random_mood[pawn] = mood; }
        }

        public static void AddUtilityDryad(PawnKindDef pawn)
        {
            if (pawn !=null &&!utilityDryads.Contains(pawn))
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
