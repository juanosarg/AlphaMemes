using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.AI.Group;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(Corpse))]
    [HarmonyPatch("GiveObservedHistoryEvent")]
    //patch to not give the observed corpse thought at a funeral. Thats like part of the point...
    public static class Corpse_GiveObservedHistoryEvent
    {
        [HarmonyPostfix]
        public static void Postfix(Corpse __instance, Pawn observer, ref HistoryEventDef __result)
        {
            if(__result == null) { return; }
            Lord lord = observer.GetLord();
            LordJob_Ritual ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
            if(ritual != null)
            {
                if(ritual is LordJob_Ritual_FuneralFramework)
                {
                    __result = null;
                }
            }
        }

    }


}
