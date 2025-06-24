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

/*    [HarmonyPatch(typeof(Building_WarcasketFoundry))]
    [HarmonyPatch("OccupantAliveAndPresent")]
    [HarmonyPatch(MethodType.Getter)]*/
    //To get the mesh to work on dreadnought funeral
    public static class Building_WarcasketFoundry_OccupantAliveAndPresent_Patch
    {
       /* [HarmonyPostfix]*/
        public static void Postfix(ref bool __result, Pawn ___occupant)
        {
            if (___occupant == null) { return; }
            if (___occupant.Dead)
            {
                if(Find.IdeoManager.GetActiveRituals(___occupant.Corpse.Map).Any(x => x.Ritual.def.defName == "AM_DreadnoughtFuneral"))//Some extra protection
                {
                    __result = true;
                }
                
                
            }
            

        }

    }


}
