using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //simple override to make the position always facing interaction spot
    public class RitualPosition_InterctingOnCell : RitualPosition_OnInteractionCell
    {
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = ritual.selectedTarget.Thing;
			IntVec3 cell = thing.InteractionCell;
			Rot4 rotation = thing.Rotation;
			return new PawnStagePosition(cell, thing, rotation, this.highlight);
		}
	}
}
