using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_BloodfeederDied_Desire : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (WorldComponent_KillsTracker.Instance.ticksWithoutSanguoKill<45*60000)
            {
                return false;
            }
           
            else return ThoughtState.ActiveAtStage(0);






        }
    }
}