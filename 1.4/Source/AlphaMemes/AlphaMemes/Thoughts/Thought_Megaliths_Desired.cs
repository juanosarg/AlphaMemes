using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_Megaliths_Desired : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.Map?.IsPlayerHome != true)
            {
                return false;
            }


            if (StaticCollectionsClass.megalithsInTheMap*3 < PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count)
            {
                return ThoughtState.ActiveAtStage(0); 

            }
            return false;












        }


    }
}
