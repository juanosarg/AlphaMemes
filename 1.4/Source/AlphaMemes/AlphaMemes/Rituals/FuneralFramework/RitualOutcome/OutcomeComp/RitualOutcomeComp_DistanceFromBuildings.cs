using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    //This is just repurposed FocusStrengthOffset_ArtificialBuildings
    public class RitualOutcomeComp_DistanceFromBuildings : RitualOutcomeComp_FuneralFramework
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return null;
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Thing thing = ritual.selectedTarget.Thing;
            //Sky burial thing is considered natural
            return ritual.Map.listerArtificialBuildingsForMeditation.GetForCell(thing.Position, radius).Count;

        }
        protected override string ExpectedOffsetDesc(bool positive, float quality = 0)
        {
            return "QualityOutOf".Translate(quality.ToStringWithSign("0.#%"), this.curve.Points[0].y.ToStringWithSign("0.#%"));
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            List<Thing> things = ritualTarget.Map.listerArtificialBuildingsForMeditation.GetForCell(ritualTarget.Thing.Position, radius);
            float count = things.Count;
            string tooltip = null;
            if (things.Count > 0)
            {
                IEnumerable<string> source = (from c in things
                                              select GenLabel.ThingLabel(c, 1, false)).Distinct<string>();
                TaggedString taggedString = "AM_UnnaturalThingsAffecting".Translate(radius.ToString()) + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
                if (source.Count<string>() > 3)
                {
                    taggedString += " " + "Etc".Translate();
                }
                tooltip = taggedString.Resolve();
            }
            float quality = this.curve.Evaluate((float)count);
            return new ExpectedOutcomeDesc
            {
                label = "AM_ArtificialBuildings".Translate(),
                count = Mathf.Min((float)count, base.MaxValue) + " / " + base.MaxValue,
                effect = this.ExpectedOffsetDesc(quality>0, quality),
                quality = quality,
                positive = (quality > 0),
                priority = 1f,
                tip = tooltip

            };
        }
        public float radius;

    }
}
