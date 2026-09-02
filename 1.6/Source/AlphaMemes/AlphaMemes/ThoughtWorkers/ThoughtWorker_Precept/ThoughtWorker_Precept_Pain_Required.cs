
using RimWorld;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Pain_Required : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            float painTotal = p.health.hediffSet.PainTotal;
            if (painTotal < 0.0001f)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            if (painTotal < 0.15f)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            if (painTotal < 0.4f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            if (painTotal < 0.8f)
            {
                return ThoughtState.ActiveAtStage(3);
            }
            return ThoughtState.ActiveAtStage(4);
        }
    }
}