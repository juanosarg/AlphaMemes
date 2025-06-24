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


    [HarmonyPatch(typeof(Precept_Relic))]
    [HarmonyPatch("Notify_ThingLost")]
    public static class AlphaMemes_Precept_Relic_Notify_ThingLost_Patch
    {
        [HarmonyPrefix]
        static bool DontNotifyForIconoclasts()
        {
            if (Current.Game.World.factionManager.OfPlayer.ideos.HasAnyIdeoWithMeme(InternalDefOf.AM_Iconoclast) ==true) {
                return false;
            }else return true;
            




        }
    }








}
