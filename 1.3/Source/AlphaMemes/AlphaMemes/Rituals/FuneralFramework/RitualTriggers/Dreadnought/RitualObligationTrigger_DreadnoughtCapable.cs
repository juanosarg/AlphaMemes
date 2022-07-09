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
                Messages.Message("Funeral_DreadnoughtBrainGoneMessage".Translate(p.NameShortColored.Named("PAWN"),ritual.Label.Named("RITUAL")),MessageTypeDefOf.NeutralEvent);//Trying to convey why obligation was not created
                return;
            }
            RitualObligation obligation = new RitualObligation(ritual, p.Corpse, false)//Not expiring automatically. I'll handle the removal of obligation
            {
                sendLetter = !p.IsSlave
            };
            //New idea obligation only sends letter if you an do it right away.
            ResearchProjectDef research = DefDatabase<ResearchProjectDef>.GetNamed("VFEP_SpacerWarcaskets", false);
            if (!research?.IsFinished ?? true)
            {
                obligation.sendLetter = false;
            }

            ritual.AddObligation(obligation);

         


        }
        
        public override void Tick()
        {
            base.Tick();
            //I dont like this but hopefuly impact is negligable. I'd like to have my own ritual oblgiation for this. But there's nothing virtual in that and since they essentially all tick anyway I figured this is less impact then harmony
            foreach(RitualObligation obligation in ritual.activeObligations?.ToList())
            {
                Corpse corpse = obligation.targetA.Thing as Corpse;
                if (corpse != null)
                {
                    if (corpse.ParentHolder is Building_CryptosleepCasket) { continue; }//if crypto burial no issue
                    if(corpse.Age >= (30 * 60000))//30 days is max they can get before that brain is too freezerburnt
                    {
                        ritual.activeObligations.Remove(obligation);
                    }
                }
            }

        }




    }


}
