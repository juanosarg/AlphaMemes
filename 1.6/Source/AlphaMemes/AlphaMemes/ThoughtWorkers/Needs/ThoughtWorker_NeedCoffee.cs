using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class ThoughtWorker_NeedCoffee : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.needs?.TryGetNeed<Need_Coffee>() == null)
            {
                return ThoughtState.Inactive;
            }

            if (InternalDefOf.AM_CoffeeDrinking_Required == null || p.Ideo?.HasPrecept(InternalDefOf.AM_CoffeeDrinking_Required) != true)
            {
                return ThoughtState.Inactive;
            }
            Need_Coffee need = p.needs.TryGetNeed<Need_Coffee>();
            switch (need.CurCategory)
            {
                case CoffeeNeedCategory.Craving:
                    return ThoughtState.ActiveAtStage(0);
                case CoffeeNeedCategory.Desiring:
                    return ThoughtState.ActiveAtStage(1);
                case CoffeeNeedCategory.Wanting:
                    return ThoughtState.ActiveAtStage(2);
                case CoffeeNeedCategory.RecentlyDrank:
                    return ThoughtState.Inactive;
                case CoffeeNeedCategory.Full:
                    return ThoughtState.ActiveAtStage(3);
                case CoffeeNeedCategory.CompletelyFull:
                    return ThoughtState.ActiveAtStage(4);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
