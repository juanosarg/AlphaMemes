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


	[HarmonyPatch(typeof(ThoughtUtility))]
	[HarmonyPatch("RemovePositiveBedroomThoughts")]
	public static class AlphaMemes_ThoughtUtility_RemovePositiveBedroomThoughts_Patch
	{
		[HarmonyPostfix]
		static void RemoveBarrackThoughts(Pawn pawn)
		{

			if (pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(InternalDefOf.AM_SleptInBarracksMonastic, (Thought_Memory thought) => thought.MoodOffset() > 0f);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(InternalDefOf.AM_SleptInPrivateRoomMonastic, (Thought_Memory thought) => thought.MoodOffset() > 0f);

				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(InternalDefOf.AM_SleptInBarracksPreferred, (Thought_Memory thought) => thought.MoodOffset() > 0f);
				pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(InternalDefOf.AM_SleptInPrivateRoomPreferred, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			}





		}
	}








}
