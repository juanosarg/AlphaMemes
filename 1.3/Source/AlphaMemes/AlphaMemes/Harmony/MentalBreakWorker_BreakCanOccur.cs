using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(MentalBreakWorker))]
    [HarmonyPatch("BreakCanOccur")]
    public static class AlphaMemes_MentalBreakWorker_BreakCanOccur_Patch
    {
        [HarmonyPostfix]
        static void DisableMostMentalBreaksIfPacifist(ref bool __result, Pawn pawn, MentalBreakWorker __instance)
        {

            if (pawn.Ideo != null && pawn.Ideo.HasPrecept(InternalDefOf.AM_Violence_Abhorrent_Strict))
            {

                if (__instance.def.defName != "Wander_Psychotic")
                {

                    __result = false;
                }

            }





        }
    }








}
