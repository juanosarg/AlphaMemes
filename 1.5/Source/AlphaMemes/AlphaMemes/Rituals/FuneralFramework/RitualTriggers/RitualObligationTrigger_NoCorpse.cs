using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Universal No corpse trigger
    public class RitualObligationTrigger_NoCorpse : RitualObligationTrigger
    {
        public override void Notify_MemberCorpseDestroyed(Pawn p)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }
            if ((!Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || p.HomeFaction != Faction.OfPlayer || !p.IsFreeNonSlaveColonist || p.IsKidnapped())
            {
                return;
            }
            List<Precept_Ritual> rituals = ritual.ideo.GetAllPreceptsOfType<Precept_Ritual>().ToList();
            if (rituals.Where(x => x.def.HasModExtension<FuneralPreceptExtension>()).Any(x => x.def.GetModExtension<FuneralPreceptExtension>().addNoCorpseFuneral))
            {
                this.ritual.AddObligation(new RitualObligation(this.ritual, p, true));
            }
        }




    }        



}
