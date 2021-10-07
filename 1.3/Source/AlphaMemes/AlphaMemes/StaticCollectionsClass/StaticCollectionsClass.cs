
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

        public static float artBeautyInTheMap = 0;

        public static Dictionary<Pawn, int> colonist_and_random_mood = new Dictionary<Pawn, int>();


        public static void AddColonistRandomMood(Pawn pawn, int mood)
        {
            if (!colonist_and_random_mood.ContainsKey(pawn))
            {
                colonist_and_random_mood.Add(pawn, mood);
            }
            else { colonist_and_random_mood[pawn] = mood; }
        }

    }
}
