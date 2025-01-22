﻿using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(GenConstruct))]
    [HarmonyPatch("CanConstruct")]
    [HarmonyPatch(new Type[] { typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool),typeof(JobDef) })]
    public static class AlphaMemes_GenConstruct_CanConstruct_Patch
    {
        [HarmonyPostfix]
        static void CantBuildReliquary(Thing t, Pawn p, ref bool __result)
        {

            ThingDef thingDef;
            if ((t.def.IsBlueprint || t.def.IsFrame) && (thingDef = (t.def.entityDefToBuild as ThingDef)) != null && thingDef.building != null )
            {
                if(thingDef == ThingDefOf.Reliquary && !new HistoryEvent(InternalDefOf.AM_BuildingReliquary, p.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
                {
                    __result = false;

                }
                if (thingDef == ThingDefOf.DeathrestCasket && !new HistoryEvent(InternalDefOf.AM_BuildingDeathrestCasket, p.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
                {
                    __result = false;

                }
                if (thingDef == InternalDefOf.VRE_Draincasket && !new HistoryEvent(InternalDefOf.AM_BuildingDrainCasket, p.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
                {
                    __result = false;

                }


            }






        }
    }








}
