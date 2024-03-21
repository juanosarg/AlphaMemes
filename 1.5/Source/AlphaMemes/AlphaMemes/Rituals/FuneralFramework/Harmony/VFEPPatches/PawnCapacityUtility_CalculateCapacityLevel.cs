using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Verse.PawnCapacityUtility;
using System;


namespace AlphaMemes
{

    //Patch to tempoarily override the VFE Apparel extension pawnCapacityMinLevels so that resurrection sickness can actually do things.
    public static class PawnCapacityUtility_CalculateCapacityLevel_Patch
    {
       /* [HarmonyPostfix]*/
        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null, bool forTradePrice = false)
        {            

            if (diffSet.GetFirstHediffOfDef(FuneralFrameWork_StaticStartup.AM_WarCasketLifeSupport) != null)
            {
                Hediff sick = null;
                Hediff psyco = null;
                if ((sick = diffSet.GetFirstHediffOfDef(HediffDefOf.ResurrectionSickness)) != null || (psyco = diffSet.GetFirstHediffOfDef(HediffDefOf.ResurrectionPsychosis)) != null)
                {
                    //Have to redo some of the work of vanilla method to return what I want.
                    float cap = capacity.Worker.CalculateCapacityLevel(diffSet, impactors); 
                    float factor = 1f;
                    bool flag = false;
                    List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();
                    if(sick != null) { capMods.AddRange(sick.CapMods); } //Rather then looping everything like vanilla there's only 2 hediff I care about.
                    if (psyco != null) { capMods.AddRange(psyco.CapMods); }
                    foreach (PawnCapacityModifier mod in capMods)
                    {
                        if(mod.capacity == capacity)
                        {
                            flag = true;
                            cap += mod.offset;
                            factor *= mod.postFactor;
                        }
                    }
                    if (flag) //So we only affect the result of things related to resurrection
                    {
                        cap = Mathf.Max(cap * factor, capacity.minValue);
                        __result = GenMath.RoundedHundredth(cap);
                    }

                }
                
            }

        }

    }


}
