using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace AlphaMemes
{
    public class RitualStage_AnimaBurial: RitualStage
    {
        public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
        {
            return ritual.selectedTarget;
        }
    }
}
