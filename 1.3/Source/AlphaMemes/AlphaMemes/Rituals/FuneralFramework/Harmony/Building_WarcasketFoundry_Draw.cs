using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


namespace AlphaMemes
{

/*    [HarmonyPatch(typeof(Building_WarcasketFoundry))]
    [HarmonyPatch("Draw")]*/
    //To position corpse
    public static class Building_WarcasketFoundry_Draw_Patch
    {
        /*[HarmonyPostfix]*/
        public static void Postfix(Pawn ___occupant, Building __instance)
        {
            if (___occupant == null) { return; }
            if (___occupant.Dead)
            {
                if(Find.IdeoManager.GetActiveRituals(___occupant.Corpse.Map).Any(x => x.Ritual.def.defName == "AM_DreadnoughtFuneral"))//Some extra protection
                {
                    Vector3 vector = ___occupant.Corpse.Position.ToVector3Shifted();
                    vector += new Vector3(0, 0, 0.67f);
                    vector.y = ___occupant.def.Altitude;
                    ___occupant.Drawer.renderer.wiggler.SetToCustomRotation(0f);
                    ___occupant.Drawer.renderer.RenderPawnAt(vector,Rot4.South);//Trying render at directly cause DrawAt doesnt give me rot4 override
                    //___occupant.Corpse.DrawAt(vector);
                }
                
                
            }
            

        }       
    }


}
