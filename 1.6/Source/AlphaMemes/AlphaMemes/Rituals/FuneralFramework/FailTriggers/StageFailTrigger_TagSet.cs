using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;


namespace AlphaMemes
{
    //Simple way to add fail conditions to jobgivers
    public class StageFailTrigger_TagSet : StageFailTrigger
    {
        public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
        {
            Pawn pawn = ritual.PawnWithRole(roleID);            
            if (ritual.PawnTagSet(pawn, tag))
            {
                return true;
            }
            return false;
            
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref roleID, "roleID");
            Scribe_Values.Look(ref tag, "tag");
        }
        public string roleID;
        public string tag;
    }
}
