using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_NeedTea : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.needs?.TryGetNeed<Need_Tea>() == null)
            {
                return ThoughtState.Inactive;
            }

            if (InternalDefOf.AM_TeaDrinking_Required == null || p.Ideo?.HasPrecept(InternalDefOf.AM_TeaDrinking_Required) != true)
            {
                return ThoughtState.Inactive;
            }
            Need_Tea need = p.needs.TryGetNeed<Need_Tea>();
            switch (need.CurCategory)
            {
                case TeaNeedCategory.Craving:
                    return ThoughtState.ActiveAtStage(0);
                case TeaNeedCategory.Desiring:
                    return ThoughtState.ActiveAtStage(1);
                case TeaNeedCategory.Wanting:
                    return ThoughtState.ActiveAtStage(2);
                case TeaNeedCategory.RecentlyDrank:
                    return ThoughtState.Inactive;
                case TeaNeedCategory.Full:
                    return ThoughtState.ActiveAtStage(3);
                case TeaNeedCategory.CompletelyFull:
                    return ThoughtState.ActiveAtStage(4);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
