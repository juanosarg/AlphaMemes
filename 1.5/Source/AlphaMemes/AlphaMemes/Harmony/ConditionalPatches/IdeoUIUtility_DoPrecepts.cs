using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;

namespace AlphaMemes
{
    /*[HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("DoPrecepts")]*/
    public static class AlphaMemes_IdeoUIUtility_DoPrecepts_Amount_Patch
    {
        /*[HarmonyPostfix]*/
        public static void EnableMorePrecepts()
        {
            List<CurvePoint> newCurve = new List<CurvePoint>();
            newCurve.Add(new CurvePoint(0.5f, 2));
            newCurve.Add(new CurvePoint(1f, (int)AlphaMemes_Settings.relicsAmount));
            DefDatabase<PreceptDef>.GetNamedSilentFail("IdeoRelic").preceptInstanceCountCurve.Points.Clear();
            DefDatabase<PreceptDef>.GetNamedSilentFail("IdeoRelic").preceptInstanceCountCurve.Points.AddRange(newCurve);

            List<CurvePoint> newCurve2 = new List<CurvePoint>();
            newCurve2.Add(new CurvePoint(0.5f, 2));
            newCurve2.Add(new CurvePoint(1f, (int)AlphaMemes_Settings.buildingsAmount));
            DefDatabase<PreceptDef>.GetNamedSilentFail("IdeoBuilding").preceptInstanceCountCurve.Points.Clear();
            DefDatabase<PreceptDef>.GetNamedSilentFail("IdeoBuilding").preceptInstanceCountCurve.Points.AddRange(newCurve2);
        }
    }
}