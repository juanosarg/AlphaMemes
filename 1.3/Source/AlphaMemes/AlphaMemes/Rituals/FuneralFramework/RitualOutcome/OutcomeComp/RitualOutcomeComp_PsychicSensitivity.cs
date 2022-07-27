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
                float count = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
                if (pawn.Dead)
                {
                    count = count - .5f;
                }
                return count;
            }
            return base.Count(ritual, data);
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            Pawn pawn = assignments.AssignedPawns(roleId).FirstOrDefault();           
            if(pawn == null) { return null; }
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
        public override Pawn BestPawnForRole(List<Pawn> pawns, RitualRoleAssignments assignments, out string roleId)
        {
            roleId = this.roleId;
            if(roleId == "AM_RitualRoleCorpse")
            {
                return null;
            }
            Pawn bestPawn = pawns.First(x => !x.Dead);//Was random but eventually that'd cause an awkward situation
            foreach (Pawn pawn in pawns)
            {
                bool flag = false;
                string reason = null;
                if (pawn.GetPsylinkLevel() > bestPawn.GetPsylinkLevel() && !pawn.Dead)
                {
                    Precept_Role pRole = pawn.Ideo?.GetRole(pawn);//seperated this all out because somewhere in the chain it wasnt happy about null and I wanted to see where. Dont feel like recombining
                    if (pRole != null)
                    {
                        foreach (RitualRole role in assignments.Ritual.behavior.def.roles)
                        {
                            flag = role.AppliesToRole(pRole, out reason);
                            if (flag) { break; }
                        }
                    }
                    if (!flag)
                    {
                        bestPawn = pawn;
                    }

                }

            }
            return bestPawn;
        }
        public string roleId;


    }
}
