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
    public class RitualBehaviorWorker_SkyBurial : RitualBehaviorWorker_FuneralFramework 
    {

        public RitualBehaviorWorker_SkyBurial()
        {
        }
        public RitualBehaviorWorker_SkyBurial(RitualBehaviorDef def) : base(def)
        {
        }

        public override void Cleanup(LordJob_Ritual ritual)
        {
            //If the funeral failed throw the body out so it can be completed properly, as can't be manually removed
            if (ritual.Ritual.activeObligations.Contains(ritual.obligation))
            {
                var skyBurialSpot = ritual.selectedTarget.Thing as Building_SkyBurialSpot;
                if (skyBurialSpot != null)
                {
                    skyBurialSpot.EjectContents();
                }                
            }
            base.Cleanup(ritual);
        }

    }
}
