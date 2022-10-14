using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using System.Threading.Tasks;

namespace AlphaMemes
{    //This is not a real pawn positon. If you need the delviering pawn to have a custom position after delivering use pawnPosition field
	 //roleBehaviors accepts a list of custom positions but dont use it with this
	public class RitualPosition_CorpsePosition : RitualPosition
    {
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{

			Thing thing = ritual.selectedTarget.Thing;
			IntVec3 cell = thing.InteractionCell;
			if(pawnPosition != null)
            {
				return pawnPosition.GetCell(spot, p, ritual);
            }
			return new PawnStagePosition(spot, null, Rot4.Invalid, false);
		}

		public virtual IntVec3 CorpseCell(Pawn p, LordJob_Ritual ritual, Thing overrideThing = null)
		{
			Thing thing = overrideThing == null ? ritual.selectedTarget.Thing : overrideThing;

			IntVec3 cell = thing.Position;
			if (corpseOffset.TryGetValue(thing.def, out var offset))
            {
				cell += offset.RotatedBy(thing.Rotation);//we'll see if that works but its what they do for interactionCellOffset
			}			
			if (!p.CanReach(cell, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(p, Danger.Deadly))) //Using Position fallback as fallback
			{
				Log.Warning("Alpha Memes Funeral framework Can't reach cell position. RitualPosition_CorpsePosition:CorpseCell using fall back");
				cell = base.GetFallbackSpot(thing.OccupiedRect(), ritual.Spot, p, ritual,new Func<IntVec3,bool>(x=>x.Standable(ritual.Map)));
				return cell;
			}
			return cell;
		}

        public override void ExposeData()
        {
            base.ExposeData();
			Scribe_Collections.Look(ref corpseOffset, "corpeOffset",LookMode.Value,LookMode.Value,ref tmpThingDefs,ref tmpVec3);
			Scribe_Deep.Look(ref pawnPosition, "pawnPosition");
        }
		public Dictionary<ThingDef,IntVec3> corpseOffset = new Dictionary<ThingDef,IntVec3>();
		private List<ThingDef> tmpThingDefs = new List<ThingDef>();
		private List<IntVec3> tmpVec3 = new List<IntVec3>();
		//public IntVec3 corpeOffset = IntVec3.Zero;
		public RitualPosition pawnPosition = null;
	}


}
