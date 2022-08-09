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
            tryAddBoth = props1.tryAddBoth;
            if (props1.AltTriggerProps != null)
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
        public override void Tick()
        {
            base.Tick();
            otherRitTrigger?.Tick();
            AltTrigger?.Tick();
        }
        public override void Notify_MemberDied(Pawn p)
        {
            RitualObligation obligation = new RitualObligation(ritual, p.Corpse, true)
            {
                sendLetter = !p.IsSlave
            };
            if (UseAlternate(p, obligation))
            {
                if ( otherRitTrigger != null)
                {
                    otherRitTrigger.Notify_MemberDied(p);
                    if (!tryAddBoth) { return; }
                }
                else
                {
                    PreceptDef otherRitualDef = ritual.def.alsoAdds;
                    Precept_Ritual othRitual = p.ideo.Ideo.GetAllPreceptsOfType<Precept_Ritual>().FirstOrDefault(x => x.def == otherRitualDef);
                    if (ShouldTrigger(p) && othRitual != null)
                    {
                        othRitual.AddObligation(new RitualObligation(othRitual, p.Corpse, true)
                        {
                            sendLetter = !p.IsSlave
                        });
                        if (!tryAddBoth) { return; }
                    }
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
            if ((!Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped()) 
            {
                return false;
            }
            return true;
        }
        //Pretty sure this is needed for reform to work right.
        public override void CopyTo(RitualObligationTrigger other)
        {
            base.CopyTo(other);
            var other1 = other as RitualObligationTrigger_AlsoAdds;
            other1.otherRitTrigger = otherRitTrigger;
            other1.AltTrigger = AltTrigger;
            other1.tryAddBoth = tryAddBoth;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref otherRitTrigger, "otherRitTrigger");
            Scribe_Deep.Look(ref AltTrigger, "AltTrigger");
            Scribe_Values.Look(ref tryAddBoth, "tryAddBoth");
        }

        public bool tryAddBoth = false;//Will try to add both obligations. Only do this if you have it setup so only one can send ritual
        public RitualObligationTrigger otherRitTrigger; //If you want to call an alternate trigger for the alsoAdd Ritual instead
        public RitualObligationTrigger AltTrigger; //If you want to call an alternate trigger for the main Ritual instead
    }


}
