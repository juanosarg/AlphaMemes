using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlphaMemes
{
    public class Dialog_AnimalDatabase : Window
    {

        public Building_AnimalDatabase building;
        private Vector2 scrollPosition = new Vector2(0, 0);
        private string searchText = "";
        private bool checkedBox = false;

        public Dialog_AnimalDatabase(Building_AnimalDatabase building)
        {
            this.building = building;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;
        }

        public override Vector2 InitialSize => new Vector2(620f, 500f);

       

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;
            searchText = Widgets.TextField(outRect.TopPartPixels(35f), searchText);
            outRect.yMin += 40f;

            
            var rectCheckBox = new Rect(0f,70f,250f,30f);
            Widgets.CheckboxLabeled(rectCheckBox, "AM_OnlyTaggedAnimals".Translate(), ref checkedBox);
            outRect.yMin += 60f;
            List<PawnKindDef> animalList = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
                                            where x.RaceProps.Animal && !x.RaceProps.Dryad
                                            select x).OrderBy(x => x.label).ToList();

            var shownOptions = animalList.Where(animal => animal.LabelCap.ToString().ToLower().Contains(searchText.ToLower())&&(!checkedBox||(checkedBox&&building.analyzedAnimalList.Contains(animal)))).ToList();

            var viewRect = new Rect(0f, 0f, outRect.width - 16f, shownOptions.Sum(opt => 50 + 17f));
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {
                var y = 0f;
                for (var i = 0; i < shownOptions.Count; i++)
                {
                    var opt = shownOptions[i];
                    var height = 50 + 10f;
                    var rect2 = new Rect(0f, y, viewRect.width - 7f, height);
                    if (shownOptions[i] != null)
                    {
                        Rect rectIcon = new Rect(0, rect2.y + 1f, 50f, 50f);
                        GUI.color = shownOptions[i].lifeStages.Last().bodyGraphicData.color;
                        GUI.DrawTexture(rectIcon, ContentFinder<Texture2D>.Get(shownOptions[i].lifeStages.Last().bodyGraphicData.Graphic.path+"_east"));
                        Rect rectText = new Rect(rectIcon.xMax + 10f, rect2.y + 1f, 250,50);
                        GUI.color = Color.white;
                        Widgets.Label(rectText, shownOptions[i].LabelCap);
                        Rect rectTextAcquired = new Rect(rectIcon.xMax + 270, rect2.y + 1f, 150, 50);
                        if (building.analyzedAnimalList.Contains(shownOptions[i]))
                        {
                            GUI.color = Color.green;
                            Widgets.Label(rectTextAcquired, "AM_Analyzed".Translate());
                        }
                        else {
                            GUI.color = Color.red;
                            Widgets.Label(rectTextAcquired, "AM_NotAnalyzed".Translate());
                        }
                        
                        rect2.xMax -= Widgets.InfoCardButtonSize + 7f;
                        Widgets.InfoCardButton(rect2.xMax + 7f, rect2.y + 1f, shownOptions[i].race);
                    }


                    GUI.color = Color.white;
                    y += height + 7f;
                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }
    }
}