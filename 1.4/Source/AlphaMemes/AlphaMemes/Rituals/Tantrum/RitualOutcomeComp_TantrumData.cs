using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_TantrumData : RitualOutcomeComp_Data
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref destroyedWealth, "destroyedWealth");
            Scribe_Values.Look(ref colonyWealthBefore, "colonyWealthBefore");
            Scribe_Collections.Look(ref destoryedThings, true, "destoryedThings", LookMode.Reference);
        }

        public List<Thing> destoryedThings = new List<Thing>();
        public float destroyedWealth = 0f;
        public float colonyWealthBefore = 0f;
    }
}
