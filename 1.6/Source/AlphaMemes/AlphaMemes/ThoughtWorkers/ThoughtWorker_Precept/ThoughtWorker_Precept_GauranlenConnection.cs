using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_GauranlenConnection : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.connections?.ConnectedThings?.Any()==true)
            {
                return ThoughtState.ActiveAtStage(0);
                
            }

            return false;


        }


    }
}
