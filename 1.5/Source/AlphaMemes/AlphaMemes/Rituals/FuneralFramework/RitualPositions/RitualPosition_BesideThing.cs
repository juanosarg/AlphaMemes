using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Override to make it the interaction cell to start. This was needee cause base beside thing freaks out on 2x3 things \o/
    public class RitualPosition_BesideInteract: RitualPosition_BesideThing
    {
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = ritual.selectedTarget.Thing;
			IntVec3 cell = thing.InteractionCell;
			return base.GetCell(cell, p, ritual);
		}
	}
}
