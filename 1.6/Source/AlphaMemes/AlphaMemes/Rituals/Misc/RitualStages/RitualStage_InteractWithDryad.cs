using System;
using System.Linq;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class RitualStage_InteractWithDryad : RitualStage
    {
        public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
        {
            return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.kindDef == InternalDefOf.AM_Dryad_Ocular);
        }
    }
}
