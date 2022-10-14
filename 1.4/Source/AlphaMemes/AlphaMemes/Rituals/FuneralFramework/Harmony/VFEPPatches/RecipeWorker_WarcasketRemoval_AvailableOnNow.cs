using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using System;


namespace AlphaMemes
{


    //To prevent the pawn resurrected via sacro ritual from being removed from casket so it cant be used as just a cheapish mech serum
    public static class RecipeWorker_WarcasketRemoval_AvailableOnNow_Patch
    {
       /* [HarmonyPostfix]*/
        public static void Postfix(ref bool __result,Thing thing, BodyPartRecord part = null)
        {
            if (__result == false) { return; }
            if (thing is Pawn pawn && pawn.health.hediffSet.HasHediff(FuneralFrameWork_StaticStartup.AM_WarCasketLifeSupport))
            {
                __result = false;
            }
            

        }

    }


}
