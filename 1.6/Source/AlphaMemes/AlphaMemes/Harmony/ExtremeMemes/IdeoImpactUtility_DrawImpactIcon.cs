using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(IdeoImpactUtility))]
    [HarmonyPatch("DrawImpactIcon")]
    public static class AlphaMemes_IdeoImpactUtility_DrawImpactIcon_Patch
    {
        private static readonly Color IconTint = Color.Lerp(ColoredText.ImpactColor, new Color(0.3f, 0.3f, 0.3f, 1f), 0.9f);
        private static readonly CachedTexture Impact4Icon = new CachedTexture("UI/Icons/AM_MemeImpact4");


        [HarmonyPrefix]
        public static bool DrawImpactFour(Rect rect, int impact)
        {
            if (impact == 4)
            {
                Color color = GUI.color;
                GUI.color = IconTint;
                GUI.DrawTexture(rect, Impact4Icon.Texture);
                GUI.color = color;
                return false;
            }
            return true;
        }
    }
}
