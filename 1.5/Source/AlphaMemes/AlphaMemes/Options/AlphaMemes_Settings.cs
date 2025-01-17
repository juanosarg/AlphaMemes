using RimWorld;
using UnityEngine;
using Verse;
using System;


namespace AlphaMemes
{


    public class AlphaMemes_Settings : ModSettings

    {



        public static bool makeChangeStyleAbilityUseAllStyles = false;

        public static float memeAmount = baseGameMemeAmount;
        public const float baseGameMemeAmount = 4;
        public const float maxMemeAmount = 8;


        public static float ritualsAmount = baseGameRitualsAmount;
        public const float baseGameRitualsAmount = 6;
        public const float maxRitualsAmount = 15;

        public static float stylesAmount = maxStylesAmount;
        public const float baseGameStylesAmount = 3;
        public const float maxStylesAmount = 5;

        public static float relicsAmount = baseGameRelicsAmount;
        public const float baseGameRelicsAmount = 3;
        public const float maxRelicsAmount = 15;


        public static float buildingsAmount = baseGameBuildingsAmount;
        public const float baseGameBuildingsAmount = 2;
        public const float maxBuildingsAmount = 10;



        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref makeChangeStyleAbilityUseAllStyles, "makeChangeStyleAbilityUseAllStyles", false, true);
            Scribe_Values.Look(ref memeAmount, "memeAmount", baseGameMemeAmount, true);
            Scribe_Values.Look(ref ritualsAmount, "ritualsAmount", baseGameRitualsAmount, true);
            Scribe_Values.Look(ref stylesAmount, "stylesAmount", maxStylesAmount, true);
            Scribe_Values.Look(ref relicsAmount, "relicsAmount", baseGameRelicsAmount, true);
            Scribe_Values.Look(ref buildingsAmount, "buildingsAmount", baseGameBuildingsAmount, true);




        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);

            ls.CheckboxLabeled("AM_MakeChangeStyleAbilityUseAllStyles".Translate(), ref makeChangeStyleAbilityUseAllStyles, "AM_MakeChangeStyleAbilityUseAllStyles_Tooltip".Translate());

            if(!ModLister.HasActiveModWithName("Vanilla Ideology Expanded - Memes and Structures"))
            {
                var memesLabel = ls.LabelPlusButton("VME_MemeAmount".Translate() + ": " + memeAmount, "VME_MemeAmountTooltip".Translate());
                memeAmount = (float)Math.Round(ls.Slider(memeAmount, baseGameMemeAmount, maxMemeAmount), 0);

                if (ls.Settings_Button("VME_Reset".Translate() + " " + "VME_Meme".Translate(), new Rect(0f, memesLabel.position.y + 35, 250f, 29f)))
                {
                    memeAmount = baseGameMemeAmount;
                }

                var ritualsLabel = ls.LabelPlusButton("VME_RitualAmount".Translate() + ": " + ritualsAmount, "VME_RitualAmountTooltip".Translate());
                ritualsAmount = (float)Math.Round(ls.Slider(ritualsAmount, baseGameRitualsAmount, maxRitualsAmount), 0);

                if (ls.Settings_Button("VME_Reset".Translate() + " " + "VME_Ritual".Translate(), new Rect(0f, ritualsLabel.position.y + 35, 250f, 29f)))
                {
                    ritualsAmount = baseGameRitualsAmount;
                }

                var stylesLabel = ls.LabelPlusButton("VME_StylesAmount".Translate() + ": " + stylesAmount, "VME_StylesAmountTooltip".Translate());
                stylesAmount = (float)Math.Round(ls.Slider(stylesAmount, baseGameStylesAmount, maxStylesAmount), 0);

                if (ls.Settings_Button("VME_Reset".Translate() + " " + "VME_Styles".Translate(), new Rect(0f, stylesLabel.position.y + 35, 250f, 29f)))
                {
                    stylesAmount = baseGameStylesAmount;
                }

                var relicsLabel = ls.LabelPlusButton("VME_RelicsAmount".Translate() + ": " + relicsAmount, "VME_RelicsAmountTooltip".Translate());
                relicsAmount = (float)Math.Round(ls.Slider(relicsAmount, baseGameRelicsAmount, maxRelicsAmount), 0);

                if (ls.Settings_Button("VME_Reset".Translate() + " " + "VME_Relics".Translate(), new Rect(0f, relicsLabel.position.y + 35, 250f, 29f)))
                {
                    relicsAmount = baseGameRelicsAmount;
                }
                var buildingsLabel = ls.LabelPlusButton("VME_BuildingsAmount".Translate() + ": " + buildingsAmount, "VME_BuildingsAmountTooltip".Translate());
                buildingsAmount = (float)Math.Round(ls.Slider(buildingsAmount, baseGameBuildingsAmount, maxBuildingsAmount), 0);

                if (ls.Settings_Button("VME_Reset".Translate() + " " + "VME_Buildings".Translate(), new Rect(0f, buildingsLabel.position.y + 35, 250f, 29f)))
                {
                    buildingsAmount = baseGameBuildingsAmount;
                }



            }




            ls.End();
        }



    }










}
