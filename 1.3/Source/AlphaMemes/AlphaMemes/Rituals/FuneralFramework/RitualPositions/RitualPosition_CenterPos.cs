using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Override to make it the interaction cell to start. This was needee cause base beside thing freaks out on 2x3 things \o/
    public class RitualPosition_CenterPos: RitualPosition_OnInteractionCell
	{
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = ritual.selectedTarget.Thing;
			IntVec3 cell = thing.OccupiedRect().CenterCell;
			cell += offset;
			if (this.thingOffset.TryGetValue(thing.def,out var thingOffset))
            {
				cell += thingOffset;
            }
			Rot4 rotation = thing.Rotation;
			return new PawnStagePosition(cell, thing, rotation, this.highlight);
		}
        public override void ExposeData()
        {
            base.ExposeData();
			Scribe_Collections.Look(ref thingOffset, "thingOffset", LookMode.Value, LookMode.Value, ref tmpThingDefs, ref tmpVec3);
		}
        public Dictionary<ThingDef, IntVec3> thingOffset = new Dictionary<ThingDef, IntVec3>();
		private List<ThingDef> tmpThingDefs = new List<ThingDef>();
		private List<IntVec3> tmpVec3 = new List<IntVec3>();
	}
}
