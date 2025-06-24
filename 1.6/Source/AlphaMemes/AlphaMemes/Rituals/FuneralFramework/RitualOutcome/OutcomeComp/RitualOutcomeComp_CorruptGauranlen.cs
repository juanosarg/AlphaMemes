﻿using System;
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
            if (ritual.Ritual.ideo.HasMeme(InternalDefOf.AM_BiologicalCorruptors) && base.Applies(ritual))
            {
                return true;
            }
            return false;
        }
        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            var desc = base.GetQualityFactor(ritual, ritualTarget, obligation, assignments, data);
            var flag = ritual.ideo.HasMeme(InternalDefOf.AM_BiologicalCorruptors);
            desc.positive = flag;
            desc.present = flag;
            return desc;
        }
      

    }
}
