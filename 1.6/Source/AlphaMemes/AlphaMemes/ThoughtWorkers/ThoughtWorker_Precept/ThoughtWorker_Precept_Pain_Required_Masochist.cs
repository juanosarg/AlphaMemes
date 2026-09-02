
using RimWorld;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Pain_Required_Masochist : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            float painTotal = p.health.hediffSet.PainTotal;
            if (painTotal < 0.0001f)
            {
                return ThoughtState.Inactive;
            }
           
            return ThoughtState.ActiveAtStage(0);
        }
    }
}