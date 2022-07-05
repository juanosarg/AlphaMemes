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
    public class RitualOutcomeComp_Crafter : RitualOutcomeComp_FuneralFramework
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return null;
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return ritual.RoleFilled(roleId);
        }

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Pawn pawn = ritual.assignments.AssignedPawns(roleId).First();
            if (pawn != null)
            {
                return pawn.skills.GetSkill(skill).Level;
            }
            return base.Count(ritual, data);
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            int count = assignments.AssignedPawns(roleId).First().skills.GetSkill(skill).Level;
            float quality = this.curve.Evaluate((float)count);
            return new ExpectedOutcomeDesc
            {
                label = "AM_RitualCrafterOutcome".Translate(label.Named("LABEL"),skill.LabelCap.Named("SKILL")),
                count = Mathf.Min((float)count, base.MaxValue) + " / " + base.MaxValue,
                effect = this.ExpectedOffsetDesc(true, quality),
                quality = quality,
                positive = (count > 0),
                priority = 1f

            };
        }
        public override Pawn BestPawnForRole(List<Pawn> pawns, RitualRoleAssignments assignments, out string roleId)
        {
            roleId = this.roleId;
            Pawn bestPawn = pawns.First(x => !x.Dead);//Was random but eventually that'd cause an awkward situation
            foreach(Pawn pawn in pawns)
            {
                bool flag = false;
                string reason = null;
                if(pawn.skills?.GetSkill(skill).Level > bestPawn.skills.GetSkill(skill).Level && !pawn.Dead)
                {
                    Precept_Role pRole = pawn.Ideo?.GetRole(pawn);//seperated this all out because somewhere in the chain it wasnt happy about null and I wanted to see where. Dont feel like recombining
                    if (pRole != null)
                    {
                        foreach(RitualRole role in assignments.Ritual.behavior.def.roles)
                        {
                            flag = role.AppliesToRole(pRole, out reason);
                            if (flag) { break; }
                        }                        
                    }
                    if(!flag)
                    {
                        bestPawn = pawn;
                    } 
                    
                }               

            }
            return bestPawn;
        }
        public string roleId;
        public SkillDef skill;

    }
}
