using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using System.Text;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class JoyGiver_VisitCorpseContainer : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Thing result;
			if (!pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Where(delegate (Thing x) //repurposed from JoyGiver_VisitGrave
			{
				var comp = x.TryGetComp<Comp_CorpseContainer>();
				return comp !=null && x.Faction == Faction.OfPlayer && comp.Active 
				&& (allowedOutside || x.Position.Roofed(x.Map)) && pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.None, 1, -1, null, false) && x.IsPoliticallyProper(pawn);
			}).TryRandomElementByWeight(delegate (Thing x)
			{
				float lengthHorizontal = (x.Position - pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}, out result))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, result);
		}
    }
}
