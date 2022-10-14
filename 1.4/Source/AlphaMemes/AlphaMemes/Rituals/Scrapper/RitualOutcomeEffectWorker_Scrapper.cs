using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;


namespace AlphaMemes
{
	public class RitualOutcomeEffectWorker_Scrapper : RitualOutcomeEffectWorker_FromQuality
	{



		public RitualOutcomeEffectWorker_Scrapper()
		{
		}

		public RitualOutcomeEffectWorker_Scrapper(RitualOutcomeEffectDef def) : base(def)
		{
		}



        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
			
			var behavior = jobRitual.Ritual.behavior as RitualBehaviorWorker_Scrapper;
			var scrap = behavior.toScrap;
            string comps = "";
            string advcomps = "";
            int numComps = (int)ComponentCurve.Evaluate(outcome.positivityIndex);
            int numAdv = numComps >= 6 ? numComps / 6 : 0;
            if (numComps > 0)
            {
                Thing components = ThingMaker.MakeThing(ThingDefOf.ComponentIndustrial);
                components.stackCount = numComps;
                components.StyleDef = DefDatabase<ThingStyleDef>.GetNamed("AM_Scavenger_Components");
                GenSpawn.Spawn(components, jobRitual.selectedTarget.Cell, jobRitual.Map);
                comps = "AM_ScrapperComponentCount".Translate(numComps.ToString());
                if (scrap.def.techLevel == TechLevel.Ultra || Rand.Bool)
                {                    
                    if (numAdv > 0)
                    {
                        var advanced = ThingMaker.MakeThing(ThingDefOf.ComponentSpacer);
                        advanced.stackCount = numAdv;
                        GenSpawn.Spawn(advanced, jobRitual.selectedTarget.Cell, jobRitual.Map);
                        advcomps = "AM_ScrapperAdvComponentCount".Translate(numAdv.ToString());
                    }
                }
            }
            if (outcome.Positive)
            {
                float xp = outcome.BestPositiveOutcome(jobRitual) ? 2500f : 1000f;
                foreach (Pawn pawn in totalPresence.Keys)
                {                    
                    if (jobRitual.RoleFor(pawn) != null)
                    {
                        pawn.skills.GetSkill(SkillDefOf.Crafting).Learn(xp * 2);
                    }
                    pawn.skills.GetSkill(SkillDefOf.Intellectual).Learn(xp);

                }
            }

            extraOutcomeDesc = "AM_ScrapperRitualOutcome".Translate(scrap.Label,comps, advcomps);
            scrap.Destroy();
		}

        private static readonly SimpleCurve ComponentCurve = new SimpleCurve
        {
            {
                new CurvePoint(-2f, 0f),
                true
            },
            {
                new CurvePoint(-1f, 3f),
                true
            },
            {
                new CurvePoint(1f, 6f),
                true
            },
            {
                new CurvePoint(2, 12f),
                true
            },

        };
    }
}
