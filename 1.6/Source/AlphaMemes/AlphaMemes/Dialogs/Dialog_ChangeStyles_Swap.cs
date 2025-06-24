using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlphaMemes
{
    [StaticConstructorOnStartup]
    public class Dialog_ChangeStyles_Swap : Window
    {

        public HashSet<Thing> thingsToChange;
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 8;
        private static readonly Texture2D NoStyle = ContentFinder<Texture2D>.Get("UI/StyleCategories/AM_NoStyle");


        List<StyleCategoryDef> listStyles = new List<StyleCategoryDef>();

        public Dialog_ChangeStyles_Swap(HashSet<Thing> things)
        {
            this.thingsToChange = things;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;           
            listStyles = (from x in DefDatabase<StyleCategoryDef>.AllDefsListForReading select x).ToList();

        }

        public override Vector2 InitialSize => new Vector2(620f, 500f);

       

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 30f;
            outRect.yMax -= 40f;

           
            if (listStyles.Count > 0) {

                Widgets.Label(new Rect(0, 10, 500f, 30f), "AM_ChooseStyle_Swap_First".Translate());

                var viewRect = new Rect(0f, 30f, outRect.width - 16f, (listStyles.Count/8)*64 + 64);

                               
                Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

               
                Rect rectIconFirst = new Rect( 10, 30f, 64, 64);
                
                GUI.DrawTexture(rectIconFirst, NoStyle, ScaleMode.StretchToFill, alphaBlend: true,0f, Color.white, 0f,0f);
                if (Widgets.ButtonInvisible(rectIconFirst))
                {
                    Dialog_ChangeStyles_Swap_Second window = new Dialog_ChangeStyles_Swap_Second(thingsToChange, null);
                    Find.WindowStack.Add(window);

                    Close();
                }
                TooltipHandler.TipRegion(rectIconFirst, "AM_DefaultStyle".Translate());

                for (var i = 0; i < listStyles.Count; i++)
                {
                    
                      
                        Rect rectIcon = new Rect((64 * (i% columnCount)) +10,(64*(i/ columnCount))+100, 64, 64);
                        GUI.DrawTexture(rectIcon, listStyles[i].Icon, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                        if (Widgets.ButtonInvisible(rectIcon))
                        {
                        Dialog_ChangeStyles_Swap_Second window = new Dialog_ChangeStyles_Swap_Second(thingsToChange, listStyles[i]);
                        Find.WindowStack.Add(window);

                        Close();


                    }
                        TooltipHandler.TipRegion(rectIcon, listStyles[i].LabelCap);
                    


                }



                Widgets.EndScrollView();

            }
            else
            {
                Widgets.Label(new Rect(0, 10, 300f, 30f), "AM_NoStyles".Translate());
            }
            
            
        }
    }


    [StaticConstructorOnStartup]
    public class Dialog_ChangeStyles_Swap_Second : Window
    {

        public HashSet<Thing> thingsToChange;
        public StyleCategoryDef styleToBeChanged;
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 8;
        private static readonly Texture2D NoStyle = ContentFinder<Texture2D>.Get("UI/StyleCategories/AM_NoStyle");


        List<StyleCategoryDef> listStyles = new List<StyleCategoryDef>();

        public Dialog_ChangeStyles_Swap_Second(HashSet<Thing> things, StyleCategoryDef style)
        {
            this.thingsToChange = things;
            this.styleToBeChanged = style;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;
            if (AlphaMemes_Settings.makeChangeStyleAbilityUseAllStyles)
            {
                listStyles = (from x in DefDatabase<StyleCategoryDef>.AllDefsListForReading select x).ToList();
            }
            else
            {
                List<ThingStyleCategoryWithPriority> listStylesWithPriority = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo.thingStyleCategories;
                foreach (ThingStyleCategoryWithPriority styleCategory in listStylesWithPriority)
                {
                    listStyles.Add(styleCategory.category);

                }
            }

        }

        public override Vector2 InitialSize => new Vector2(620f, 500f);



        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 30f;
            outRect.yMax -= 40f;


            if (listStyles.Count > 0)
            {

                Widgets.Label(new Rect(0, 10, 500f, 30f), "AM_ChooseStyle_Swap_Second".Translate());

                var viewRect = new Rect(0f, 30f, outRect.width - 16f, (listStyles.Count / 8) * 64 + 64);


                Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);


                Rect rectIconFirst = new Rect(10, 30f, 64, 64);

                GUI.DrawTexture(rectIconFirst, NoStyle, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                if (Widgets.ButtonInvisible(rectIconFirst))
                {
                    foreach (Thing thing in thingsToChange)
                    {
                        if ((thing.StyleDef == null && styleToBeChanged == null) || (thing.StyleDef != null && thing.StyleDef.defName == styleToBeChanged?.GetStyleForThingDef(thing.def)?.defName)) { 

                            thing.StyleDef = null;
                            thing.DirtyMapMesh(thing.Map);
                        }
                       
                    }

                    Close();
                }
                TooltipHandler.TipRegion(rectIconFirst, "AM_DefaultStyle".Translate());

                for (var i = 0; i < listStyles.Count; i++)
                {


                    Rect rectIcon = new Rect((64 * (i % columnCount)) + 10, (64 * (i / columnCount)) + 100, 64, 64);
                    GUI.DrawTexture(rectIcon, listStyles[i].Icon, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                    if (Widgets.ButtonInvisible(rectIcon))
                    {
                        foreach (Thing thing in thingsToChange)
                        {
                            if (thing.StyleDef == null && styleToBeChanged==null || (thing.StyleDef!=null && thing.StyleDef.defName == styleToBeChanged?.GetStyleForThingDef(thing.def)?.defName))
                            {

                                thing.StyleDef = listStyles[i].GetStyleForThingDef(thing.def);
                                thing.DirtyMapMesh(thing.Map);
                            }


                           
                        }

                        Close();


                    }
                    TooltipHandler.TipRegion(rectIcon, listStyles[i].LabelCap);



                }



                Widgets.EndScrollView();

            }
            else
            {
                Widgets.Label(new Rect(0, 10, 300f, 30f), "AM_NoStyles".Translate());
            }


        }
    }

}