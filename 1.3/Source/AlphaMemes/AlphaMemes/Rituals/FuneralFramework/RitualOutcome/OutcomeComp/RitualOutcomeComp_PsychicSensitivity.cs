using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_PsychicSensitivity : RitualOutcomeComp_FuneralFramework
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return null;
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Pawn pawn = ritual.assignments.AssignedPawns(roleId).First();
            if (pawn != null)
            {
                return pawn.GetStatValue(StatDefOf.PsychicSensitivity,true);
            }
            return base.Count(ritual, data);
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            Pawn pawn = assignments.AssignedPawns(roleId).First();
            
            float count = pawn.GetStatValue(StatDefOf.PsychicSensitivity,true);
            if (pawn.Dead)
            {
                count = count - .5f;//Stupid hack because dead pawns are always blind meaning they always get 1.5 which is dumb
            }
            float quality = this.curve.Evaluate((float)count);
            return new ExpectedOutcomeDesc
            {
                label = "AM_PsychicSensitivty".Translate(),
                count = Mathf.Min((float)count, base.MaxValue) + " / " + base.MaxValue,//I should make this display as % 
                effect = this.ExpectedOffsetDesc(true, quality),
                quality = quality,
                positive = (count > 0),
                priority = 1f

            };
        }

        public string roleId;


    }
}
