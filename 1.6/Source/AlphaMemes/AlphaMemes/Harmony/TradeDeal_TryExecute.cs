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
    [HarmonyPatch(typeof(TradeDeal))]
    [HarmonyPatch("TryExecute")]
    public static class AlphaMemes_TradeDeal_TryExecute_Patch
    {
        [HarmonyPostfix]
        static void NotifySuccessfulTrade(bool __result, List<Tradeable> ___tradeables)
        {

            if (___tradeables.Any((Tradeable x) => x.ActionToDo == TradeAction.PlayerBuys && x.ThingDef != null && x.ThingDef.defName.Contains("AP_Prefab")))
            {
                HistoryEvent historyEvent = new HistoryEvent(InternalDefOf.AM_BoughtPrefabOnMerchant);             
                Find.HistoryEventsManager.RecordEvent(historyEvent);
            }
        }
    }
}
