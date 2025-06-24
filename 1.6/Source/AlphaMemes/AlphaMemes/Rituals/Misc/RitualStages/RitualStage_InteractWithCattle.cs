using System;
using System.Linq;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class RitualStage_InteractWithCattle : RitualStage
    {
        public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
        {
            return ritual.assignments.Participants.FirstOrDefault((Pawn p) => StaticCollections.cattleAnimals.Contains(p.kindDef));
        }
    }
}
