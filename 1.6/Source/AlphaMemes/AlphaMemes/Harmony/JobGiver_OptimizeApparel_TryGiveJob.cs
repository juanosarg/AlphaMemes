using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using Verse.AI;

namespace AlphaMemes
{
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "TryGiveJob")]
    internal class AlphaMemes_JobGiver_OptimizeApparel_TryGiveJob_Postfix
    {

        [HarmonyPostfix]
        public static void RemoveClothing(Pawn pawn, ref Job __result)
        {
            if (pawn.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Forbidden) == true)
            {
                List<Apparel> wornApparel = pawn.apparel.WornApparel;
                for (int num = wornApparel.Count - 1; num >= 0; num--)
                {

                    Job job2 = JobMaker.MakeJob(JobDefOf.RemoveApparel, wornApparel[num]);
                    job2.haulDroppedApparel = true;
                    __result= job2;

                }

            }
        }
    }
}