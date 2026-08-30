using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Security.Cryptography;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(Mineable))]
    [HarmonyPatch("TrySpawnYield")]
    [HarmonyPatch(new Type[] { typeof(Map), typeof(bool), typeof(Pawn) })]
    public static class AlphaMemes_Mineable_TrySpawnYield_Patch
    {
        public static List<ThingDef> randomMinerals = new List<ThingDef>() { ThingDefOf.Steel,ThingDefOf.Gold,
        ThingDefOf.Plasteel,ThingDefOf.Uranium,ThingDefOf.Silver,ThingDefOf.Jade};

        [HarmonyPostfix]
        public static void ExtraYields(Map map, bool moteOnWaste, Pawn pawn, Mineable __instance)
        {
            if (pawn != null)
            {
                if (pawn.Ideo?.HasPrecept(InternalDefOf.AB_MiningYield_VeryHigh) == true && Rand.Chance(0.25f))
                {
                    Thing thing = ThingMaker.MakeThing(randomMinerals.RandomElement());
                    thing.stackCount = new IntRange(5, 10).RandomInRange;
                    GenPlace.TryPlaceThing(thing, __instance.Position, map, ThingPlaceMode.Near);

                }            
            }
        }
    }
}

