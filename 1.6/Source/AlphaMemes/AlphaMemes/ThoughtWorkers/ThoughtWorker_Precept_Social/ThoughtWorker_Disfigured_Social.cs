
using RimWorld;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Disfigured_Social : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn pawn, Pawn other)
        {
            if (!other.RaceProps.Humanlike || other.Dead)
            {
                return false;
            }
            if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
            {
                return false;
            }
            if (!ThoughtWorker_Precept_Disfigured.IsDisfigured(other))
            {
                return false;
            }
            if (PawnUtility.IsBiologicallyBlind(pawn))
            {
                return false;
            }
            if (!pawn.story.CaresAboutOthersAppearance)
            {
                return false;
            }
            if (pawn.Ideo != null && pawn.Ideo.IdeoApprovesOfBlindness() && ThoughtWorker_Precept_Disfigured.IsDisfigured(other) && (PawnUtility.IsBiologicallyBlind(other) || ThoughtWorker_Precept_HalfBlind.IsHalfBlind(other)))
            {
                return false;
            }
            return true;
        }
    }
}