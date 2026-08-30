using HarmonyLib;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection.Emit;
using Verse;

namespace AlphaMemes
{
    /* [HarmonyPatch(typeof(IdeoUIUtility))]
     [HarmonyPatch("DoMemes")]*/
    [StaticConstructorOnStartup]
    public static class AlphaMemes_IdeoUIUtility_DoMemes_Patch
    {
        private static readonly Color TutorArrowColor = new Color(0.937f, 0.847f, 0f);
        private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/TutorArrowRight");
        private static List<MemeDef> tmpMemesToShow = new List<MemeDef>();

        /*[HarmonyPrefix]*/
        public static void MakeBoxSmaller(ref Vector2 ___MemeBoxSize, Ideo ideo)
        {
            if (ideo.memes.Count > 6) { ___MemeBoxSize = new Vector2(80f, 120f); }
        }
        /* [HarmonyPostfix]*/
        public static void MakeBoxBigger(ref Vector2 ___MemeBoxSize, Ideo ideo)
        {
            if (ideo.memes.Count > 6)
            {
                ___MemeBoxSize = new Vector2(122f, 120f);
            }
        }
        /*[HarmonyPrefix]*/
        public static bool MoreThanEightMemes(ref float curY, float width, Ideo ideo, IdeoEditMode editMode, ref MemeDef ___tmpMouseOverMeme)
        {
            if (ideo.memes.Count > 8) {
                float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
                float curY2 = curY;
                Widgets.Label(num, ref curY2, width, "Memes".Translate());

                tmpMemesToShow.Clear();

                for (int i = 0; i < ideo.memes.Count; i++)
                {
                    if (ideo.memes[i].category != MemeCategory.Structure)
                        tmpMemesToShow.Add(ideo.memes[i]);
                }

                ___tmpMouseOverMeme = null;

                if (editMode == IdeoEditMode.GameStart && tmpMemesToShow.Any())
                {
                    DrawKnowledgeTip(ConceptDefOf.EditingMemes, curY + Text.LineHeight, num);
                }

                const int columns = 8;
                const float spacing = 8f;

                int rows = Mathf.CeilToInt(tmpMemesToShow.Count / (float)columns);

                float boxWidth = 80f;
                float boxHeight = 120f;

                float totalWidth =columns * boxWidth +(columns - 1) * spacing;

                float startX = (width - totalWidth) / 2f;

                for (int i = 0; i < tmpMemesToShow.Count; i++)
                {
                    int row = i / columns;
                    int column = i % columns;

                    float x = startX + column * (boxWidth + spacing);
                    float y = curY + row * (boxHeight + spacing);

                    Rect rect = new Rect(x,y,boxWidth,boxHeight);

                    if (editMode == IdeoEditMode.GameStart)
                    {
                        UIHighlighter.HighlightOpportunity(rect, "MemeBox");
                    }

                    IdeoUIUtility.DoMeme(rect, tmpMemesToShow[i], ideo, editMode);
                }

                curY += rows * boxHeight;
                curY += (rows - 1) * spacing;
                curY += 17f;

                return false;
            }
            return true;
        }

        private static void DrawKnowledgeTip(ConceptDef conceptDef, float curY, float labelAlignOffset)
        {
            if (Find.World == null && TutorSystem.AdaptiveTrainingEnabled && !PlayerKnowledgeDatabase.IsComplete(conceptDef))
            {
                Rect rect = new Rect(new Rect(0f, curY + Text.LineHeight, labelAlignOffset / 2f + 26f, Text.LineHeight * 2f));
                GUI.color = TutorArrowColor;
                GUI.DrawTexture(new Rect(rect.xMax - 1f, rect.y, rect.height, rect.height), ArrowTex);
                GUI.color = Color.white;
                Widgets.DrawWindowBackgroundTutor(rect);
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = Color.white;
                Widgets.Label(rect.ContractedBy(2f), IdeoUIUtility.ClickToEdit);
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }
    }
}