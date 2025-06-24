
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
namespace AlphaMemes
{
    public class PreceptComp_SanguophageDied : PreceptComp
    {

        public HistoryEventDef eventDef;

        

        public override void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
        {

            if (ev.def != eventDef)
            {
                return;
            }
            WorldComponent_KillsTracker.Instance.ticksWithoutSanguoKill = 0;
        }

      
    }
}