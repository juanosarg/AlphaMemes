using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;



namespace AlphaMemes
{



    /*This pass-through postfix adds or removes gizmos from the pawn's gizmo list (which is actually IEnumerable)
     * 
     */
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("GetGizmos")]

    static class AlphaMemes_Pawn_GetGizmos_Patch
    {

       

        [HarmonyPostfix]

        public static IEnumerable<Gizmo> AddGizmo(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (var gizmo in __result) yield return gizmo;
            var pawn = __instance;

            if (pawn.RaceProps.Animal && !pawn.RaceProps.Dryad && Current.Game.World.factionManager.OfPlayer.ideos.HasAnyIdeoWithMeme(InternalDefOf.AM_Madness))
            {


                


            }




        }

        






    }


}
