using RimWorld;
using UnityEngine;
using Verse;
using System;


namespace AlphaMemes
{


    public class AlphaMemes_Settings : ModSettings

    {



        public static bool makeChangeStyleAbilityUseAllStyles = false;
       



        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref makeChangeStyleAbilityUseAllStyles, "makeChangeStyleAbilityUseAllStyles", false, true);
           



        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);

            ls.CheckboxLabeled("AM_MakeChangeStyleAbilityUseAllStyles".Translate(), ref makeChangeStyleAbilityUseAllStyles, "AM_MakeChangeStyleAbilityUseAllStyles_Tooltip".Translate());

            ls.End();
        }



    }










}
