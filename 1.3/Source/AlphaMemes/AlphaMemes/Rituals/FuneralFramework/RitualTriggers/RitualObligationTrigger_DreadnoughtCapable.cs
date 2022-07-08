using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    
    public class RitualObligationTrigger_DreadnoughtCapable : RitualObligationTrigger_MemberDied
    {
        public override void Notify_MemberDied(Pawn p)
        {
            //Dont create obligation if no research or brain was destroyed
            if (p.health.hediffSet.GetBrain() == null)
            {
                return;
            }
            ResearchProjectDef research = DefDatabase<ResearchProjectDef>.GetNamed("VFEP_SpacerWarcaskets", false);
            if (!research?.IsFinished ?? true)
            {
                return;
            }
            Precept_Ritual ritual = p.ideo.Ideo.GetAllPreceptsOfType<Precept_Ritual>().First(x => x.def.defName == "AM_DreadnoughtFuneral");
            if(ritual.activeObligations != null)
            {
                foreach (RitualObligation obligation in ritual.activeObligations)
                {
                    if (obligation.targetA.Thing as Pawn == p)
                    {
                        return;//bit messy but adding so that if I have to rekill pawn after resurrect it doesnt create new obligation
                    }
                }
            }
            base.Notify_MemberDied(p);
        }



    }


}
