using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;
using Verse.AI;
using System.Reflection;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("DoPrecepts")]
    public static class AlphaMemes_IdeoUIUtility_DoPrecepts_Patch
    {

        [HarmonyPostfix]
        public static void AddDespisedAnimalPrecepts(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
        {
            if (ideo.HasMeme(InternalDefOf.AM_BiologicalDefilers))
            {
                Func<PreceptDef, bool> filter = (PreceptDef p) => p.preceptClass == typeof(Precept_DespisedAnimal);
                string stringDespised = "AM_DespisedAnimals".Translate();
                string stringAddDespised = "AM_AddDespisedAnimals".Translate();
                ReflectionCache.DoPreceptsInt(stringDespised, stringAddDespised, false, ideo, editMode, ref curY, width, filter, true);
            }

          

        }
    }

}
