using HarmonyLib;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;

namespace AlphaMemes
{
   /* [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("DoMemes")]*/
    public static class AlphaMemes_IdeoUIUtility_DoMemes_Patch
    {
        /*[HarmonyPrefix]*/
        public static void MakeBoxSmaller(ref Vector2 ___MemeBoxSize, Ideo ideo)
        {
            if (ideo.memes.Count > 6) { ___MemeBoxSize = new Vector2(80f, 120f); }
        }
        /* [HarmonyPostfix]*/
        public static void MakeBoxBigger(ref Vector2 ___MemeBoxSize, Ideo ideo)
        {
            if (ideo.memes.Count > 6)
            {
                ___MemeBoxSize = new Vector2(122f, 120f);
            }
        }
    }
}