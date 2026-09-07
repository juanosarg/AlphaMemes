using System;
using Verse;
using RimWorld;


namespace AlphaMemes
{
    public class PreceptComp_IncreaseCoffeeNeed : PreceptComp
    {
        public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
        {

            if (ev.def != this.eventDef)
            {
                return;
            }
            Pawn pawn = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
            if (pawn.needs != null)
            {
                Need_Coffee need = pawn.needs.TryGetNeed<Need_Coffee>();
                need.CoffeeTaken(needGain);
                need.CurLevel += needGain;
            }
        }
        public HistoryEventDef eventDef;
        public float needGain;
        public bool onlyForNonSlaves;
    }


}
