using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(Toils_LayDown))]
    [HarmonyPatch("ApplyBedThoughts")]
    public static class AlphaMemes_Toils_LayDown_ApplyBedThoughts_Patch
    {
        [HarmonyPostfix]
        static void ApplyBarrackThoughts(Pawn actor)
        {

            if (actor.ideo?.Ideo?.HasPrecept(InternalDefOf.AM_Barracks_Preferred) == true)
            {
                Building_Bed building_Bed = actor.CurrentBed();
				if (building_Bed != null && building_Bed == actor.ownership.OwnedBed && !building_Bed.ForPrisoners)
				{
					
					if (building_Bed.GetRoom().Role == RoomRoleDefOf.Barracks)
					{
						ThoughtDef thoughtDef = InternalDefOf.AM_SleptInBarracksMonastic;
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(building_Bed.GetRoom().GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex));
						}
					}
					
						
					
				}
			}





        }
    }








}
