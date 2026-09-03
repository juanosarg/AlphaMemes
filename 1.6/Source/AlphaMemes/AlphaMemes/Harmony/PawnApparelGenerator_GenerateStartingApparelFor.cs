
using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;

namespace AlphaMemes
{
 
    public class AlphaMemes_PawnApparelGenerator_GenerateStartingApparelFor_Patch
    {
       
        public static void RemoveApparel(Pawn pawn)
        {
            if (pawn?.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Armour_Forbidden) == true)
            {
                pawn.apparel.DestroyAll();
                pawn.outfits?.forcedHandler?.Reset();

            }
           

        }
    }
}