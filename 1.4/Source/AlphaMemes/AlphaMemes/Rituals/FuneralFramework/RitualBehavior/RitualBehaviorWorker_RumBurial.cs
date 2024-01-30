using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;
using Verse.AI;

using Verse.AI.Group;
using RimWorld;


namespace AlphaMemes
{
    public class RitualBehaviorWorker_RumBurial : RitualBehaviorWorker_FuneralFramework 
    {

        public RitualBehaviorWorker_RumBurial()
        {
        }
        public RitualBehaviorWorker_RumBurial(RitualBehaviorDef def) : base(def)
        {
        }
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {

            return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
        }
        public override void Cleanup(LordJob_Ritual ritual)
        {
            //If the funeral failed throw the body out so it can be completed properly, as can't be manually removed
            if (ritual.Ritual.activeObligations.Contains(ritual.obligation))
            {
                var rumVat = ritual.selectedTarget.Thing as Building_RumVat;
                if (rumVat != null)
                {
                    rumVat.EjectContents();
                }                
            }
            base.Cleanup(ritual);
        }

    }
}
