using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_CorruptGauranlen : RitualOutcomeComp_RitualTargetDefs
    {

        public override bool Applies(LordJob_Ritual ritual)
        {
            if (ritual.Ritual.ideo.HasMeme(corrupter) && base.Applies(ritual))
            {
                return true;
            }
            return false;
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            var desc = base.GetExpectedOutcomeDesc(ritual, ritualTarget, obligation, assignments, data);
            var flag = ritual.ideo.HasMeme(corrupter);
            desc.positive = flag;
            desc.present = flag;
            return desc;
        }
        private static readonly MemeDef corrupter = DefDatabase<MemeDef>.GetNamedSilentFail("AA_BiologicalCorruptors");


    }
}
