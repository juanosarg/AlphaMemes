using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
namespace AlphaMemes
{
	public class JobGiver_RitualTantrum : ThinkNode_JobGiver
	{

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out var verb))
			{
				return null;
			}
			Thing thing;
			pawn.Map.listerBuildings.allBuildingsColonist.Where(x =>pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.Deadly, 3) && x != duty.focusThird.Thing && !x.IsForbidden(pawn))
				.TryRandomElementByWeight(delegate (Thing x)
				{
					float weight = 0f;
					float lengthHorizontal = (x.Position - pawn.Position).LengthHorizontal;
					if (x.MarketValue < 5 || (x.HitPoints/5 > x.MarketValue))
                    {
						return 1f;
                    }
					weight += Mathf.Max(300f - lengthHorizontal*2, 5f); //From joygiver to find close things
					weight += x.MarketValue;
					return weight;

				}, out thing);
			Job job = JobMaker.MakeJob(InternalDefOf.AM_TrantrumJob, thing, duty.focus);
			job.verbToUse = verb;
			job.playerForced = true;//This is because if a thing is going to explode, they react and run away since not drafted. Except then it breaks the thinknode somehow and they just wander around for actually ever until you player force then to do a different job. Idk why it's annoying. But this is what they signed up for damnit. DESTRUCTION
			return job;

		}


    }	
}
