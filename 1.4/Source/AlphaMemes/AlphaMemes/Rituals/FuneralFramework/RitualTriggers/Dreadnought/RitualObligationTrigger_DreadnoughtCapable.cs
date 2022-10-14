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
            if ((!Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped())
            {
                return;
            }
            if (p.health.hediffSet.GetBrain() == null)
            {
                Messages.Message("Funeral_DreadnoughtBrainGoneMessage".Translate(p.NameShortColored.Named("PAWN"),ritual.Label.Named("RITUAL")),MessageTypeDefOf.NeutralEvent);//Trying to convey why obligation was not created
                return;
            }
            RitualObligation obligation = new RitualObligation(ritual, p, false)//Not expiring automatically. I'll handle the removal of obligation
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
            //Really dont need to check very often, but this was only ticking thing I had access to since I cant have my own obligation class for it.
            if(Find.TickManager.TicksGame - lastCheck > 300)
            {
                lastCheck = Find.TickManager.TicksGame;
                if (ritual.activeObligations.NullOrEmpty())
                {
                    return;
                }
                foreach (RitualObligation obligation in ritual.activeObligations?.ToList())
                {
                    Corpse corpse = obligation.targetA.Thing as Corpse;
                    if (corpse != null)
                    {
                        if (corpse.ParentHolder is Building_CryptosleepCasket) { continue; }//if crypto burial no issue
                        if (corpse.Age >= (30 * 60000))//30 days is max they can get before that brain is too freezerburnt
                        {
                            ritual.activeObligations.Remove(obligation);
                        }
                    }
                }
            }


        }
        public override void ExposeData()
        {
            Scribe_References.Look(ref ritual, "ritual", false);
            base.ExposeData();
        }
        public int lastCheck;

    }


}
