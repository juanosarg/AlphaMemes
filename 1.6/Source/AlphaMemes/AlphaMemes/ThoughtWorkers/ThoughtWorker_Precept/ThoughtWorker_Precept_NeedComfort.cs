
using RimWorld;
using System;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_NeedComfort : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p.needs.comfort == null)
            {
                return ThoughtState.Inactive;
            }
            switch (p.needs.comfort.CurCategory)
            {
                case ComfortCategory.Uncomfortable:
                    return ThoughtState.ActiveAtStage(0);
                case ComfortCategory.Normal:
                    return ThoughtState.Inactive;
                case ComfortCategory.Comfortable:
                    return ThoughtState.ActiveAtStage(1);
                case ComfortCategory.VeryComfortable:
                    return ThoughtState.ActiveAtStage(2);
                case ComfortCategory.ExtremelyComfortable:
                    return ThoughtState.ActiveAtStage(3);
                case ComfortCategory.LuxuriantlyComfortable:
                    return ThoughtState.ActiveAtStage(4);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}