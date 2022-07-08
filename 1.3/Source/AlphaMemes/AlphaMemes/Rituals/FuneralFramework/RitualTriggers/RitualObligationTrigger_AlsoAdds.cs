using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    
    //A way to create an obligation for your also adds ritual without creating duplicate obligations.
    //Kind of pointless now that multiple funerals are okay. But i'll leave it. In case you want like 1 funeral that has like tiers based on the pawn
    public class RitualObligationTrigger_AlsoAdds : RitualObligationTrigger
    {
        public override void Init(RitualObligationTriggerProperties props)
        {
            base.Init(props);
            var props1 = props as RitualObligationTrigger_AlsoAddsProps;
            if(props1.AltTriggerProps != null)
            {
                AltTrigger = props1.AltTriggerProps.GetInstance(ritual);
            }
            if (props1.otherRitTriggerProps != null)
            {
                PreceptDef otherRitualDef = ritual.def.alsoAdds;                
                Precept_Ritual othRitual = ritual.ideo.GetAllPreceptsOfType<Precept_Ritual>().FirstOrDefault(x => x.def == otherRitualDef);
                otherRitTrigger = props1.otherRitTriggerProps.GetInstance(othRitual);
            }
        }
        public override void Notify_MemberDied(Pawn p)
        {
            RitualObligation obligation = new RitualObligation(ritual, p.Corpse, true)
            {
                sendLetter = !p.IsSlave
            };
            if (UseAlternate(p, obligation))
            {
                if (otherRitTrigger != null)
                {
                    otherRitTrigger.Notify_MemberDied(p);
                    return;
                }
                PreceptDef otherRitualDef = ritual.def.alsoAdds;
                Precept_Ritual othRitual = p.ideo.Ideo.GetAllPreceptsOfType<Precept_Ritual>().FirstOrDefault(x => x.def == otherRitualDef);
                if(ShouldTrigger(p) && othRitual != null)
                {
                    othRitual.AddObligation(new RitualObligation(othRitual, p.Corpse, true)
                    {
                        sendLetter = !p.IsSlave
                    });
                    return;
                }
            }
            if(AltTrigger != null)
            {
                AltTrigger.Notify_MemberDied(p);
                return;
            }
            if (ShouldTrigger(p))
            {
                ritual.AddObligation(obligation);
            }              

        }
        //If no valid targets is the default. But be careful with this as there's plenty of filters that can become valid later.
        public virtual bool UseAlternate(Pawn p, RitualObligation obligation)
        {
            if (ritual.obligationTargetFilter.GetTargets(obligation, Find.AnyPlayerHomeMap).EnumerableNullOrEmpty())
            {
                return true;
            }
            return false;
        }
        //Copied from base MemberDied
        public virtual bool ShouldTrigger(Pawn p)
        {
            if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped()) 
            {
                return false;
            }
            return true;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref otherRitTrigger, "otherRitTrigger");
            Scribe_Deep.Look(ref AltTrigger, "AltTrigger");
        }


        public RitualObligationTrigger otherRitTrigger; //If you want to call an alternate trigger for the alsoAdd Ritual instead
        public RitualObligationTrigger AltTrigger; //If you want to call an alternate trigger for the main Ritual instead
    }


}
