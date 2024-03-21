using RimWorld;

namespace AlphaMemes
{
    //simple extension method to get an outcome comp just like you would a thingcomp
    public static class OutcomeExtensionMethod
    {
        public static T GetComp<T>(this RitualOutcomeEffectWorker worker) where T : RitualOutcomeComp
        {
            if (worker.def.comps != null)
            {
                int index = 0;
                int count = worker.def.comps.Count;
                while (index < count)
                {
                    T comp = worker.def.comps[index] as T;
                    if (comp != null)
                    {
                        return comp;
                    }
                    index++;
                }
            }
            return default(T);
        }
    }
}
