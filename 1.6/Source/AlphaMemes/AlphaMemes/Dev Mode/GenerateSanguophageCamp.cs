using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI.Group;
using Verse;
using RimWorld.QuestGen;
using LudeonTK;

namespace AlphaMemes
{
    public static class GenerateBiotechLabQuest
    {


        [DebugAction("Alpha Memes", "Generate Sanguophage camp quest", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void GenerateLabQuest()
        {

            Slate slate = new Slate();
            Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.AM_OpportunitySite_SanguophageCamp, slate);

            QuestUtility.SendLetterQuestAvailable(quest);
        }




    }
}

