using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;

namespace AlphaMemes
{

    /*[HarmonyPatch(typeof(Dialog_ChooseMemes))]
    [HarmonyPatch("MemeCountRangeAbsolute", MethodType.Getter)]*/
    public static class AlphaMemes_Dialog_ChooseMemes_MemeCountRangeAbsolute_Patch
    {
       /* [HarmonyPostfix]*/
        public static void SetMaxToOptions(ref IntRange __result)
        {
            __result = new IntRange(1, (int)AlphaMemes_Settings.memeAmount);
        }
    }
}
