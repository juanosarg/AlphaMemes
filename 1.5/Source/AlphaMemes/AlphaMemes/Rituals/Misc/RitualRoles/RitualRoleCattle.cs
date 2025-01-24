﻿using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class RitualRoleCattle : RitualRole
    {
        public override bool Animal
        {
            get
            {
                return true;
            }
        }

        public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
        {
            reason = null;
            if (!p.RaceProps.Animal)
            {

                if (!skipReason)
                {
                    reason = "MessageRitualRoleMustBeAnimal".Translate(base.LabelCap);
                }

                return false;
            }
            if (!StaticCollections.cattleAnimals.Contains(p.kindDef))
            {
                if (!skipReason)
                {
                    reason = "AM_MessageRitualRoleNeedsCattle".Translate();
                }
                return false;
            }

            
            if (!p.Faction.IsPlayerSafe())
            {
                if (!skipReason)
                {
                    reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
                }
                return false;
            }
            reason = null;
            return true;
        }

        public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
        {
            reason = null;
            return false;
        }


    }
}
