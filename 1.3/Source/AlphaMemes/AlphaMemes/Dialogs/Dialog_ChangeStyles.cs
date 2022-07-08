using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlphaMemes
{
    public class Dialog_ChangeStyles : Window
    {

        public Thing thingToChange;
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 4; 

        List<StyleCategoryDef> listStyles = new List<StyleCategoryDef>();

        public Dialog_ChangeStyles(Thing thing)
        {
            this.thingToChange = thing;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;
            if (AlphaMemes_Settings.makeChangeStyleAbilityUseAllStyles)
            {
                listStyles = (from x in DefDatabase<StyleCategoryDef>.AllDefsListForReading where x.GetStyleForThingDef(thingToChange.def)!=null select x).ToList();
            }
            else
            {
                List<ThingStyleCategoryWithPriority> listStylesWithPriority = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo.thingStyleCategories;
                foreach(ThingStyleCategoryWithPriority style in listStylesWithPriority)
                {
                    if(style.category.GetStyleForThingDef(thingToChange.def) != null) { listStyles.Add(style.category); }
                    
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

           
            if (listStyles.Count > 0) {

                Widgets.Label(new Rect(0, 10, 300f, 30f), "AM_ChooseStyle".Translate());

                var viewRect = new Rect(0f, 30f, outRect.width - 16f, (listStyles.Count/4)*128f + 256f);

                Color color = thingToChange.Graphic.Color;
                if (thingToChange.Stuff != null)
                {
                    color = thingToChange.def.GetColorForStuff(thingToChange.Stuff);
                }
                
                Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

               
                Rect rectIconFirst = new Rect( 10, 20f, 128f, 128f);
                
                GUI.DrawTexture(rectIconFirst, thingToChange.DefaultGraphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true,0f, color, 0f,0f);
                if (Widgets.ButtonInvisible(rectIconFirst))
                {
                    thingToChange.StyleDef = null;
                    thingToChange.DirtyMapMesh(thingToChange.Map);
                    Close();
                }
                TooltipHandler.TipRegion(rectIconFirst, "AM_DefaultStyle".Translate());

                for (var i = 0; i < listStyles.Count; i++)
                {
                    ThingStyleDef style = listStyles[i].GetStyleForThingDef(thingToChange.def);
                    if (style != null)
                    {
                      
                        Rect rectIcon = new Rect((128 * (i% columnCount)) +10,(128*(i/ columnCount))+128f, 128f, 128f);
                        GUI.DrawTexture(rectIcon, style.Graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, color, 0f, 0f);
                        if (Widgets.ButtonInvisible(rectIcon))
                        {
                            thingToChange.StyleDef = style;
                            thingToChange.DirtyMapMesh(thingToChange.Map);
                            Close();
                        }
                        TooltipHandler.TipRegion(rectIcon, listStyles[i].LabelCap);
                    }


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