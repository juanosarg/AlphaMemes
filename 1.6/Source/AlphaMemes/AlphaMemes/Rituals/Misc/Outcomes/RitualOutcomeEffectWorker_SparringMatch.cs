
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace AlphaMemes
{
    public class RitualOutcomeEffectWorker_SparringMatch : RitualOutcomeEffectWorker_FromQuality
    {
        public const float RecreationGainGood = 0.15f;

        public const float RecreationGainBest = 0.4f;

        public const float MeleeXPGainParticipantsGood = 1500f;

        public const float MeleeXPGainSpectatorsGood = 500f;

        public const float MeleeXPGainParticipantsBest = 3000f;

        public const float MeleeXPGainSpectatorsBest = 1000f;

        public RitualOutcomeEffectWorker_SparringMatch()
        {
        }

        public RitualOutcomeEffectWorker_SparringMatch(RitualOutcomeEffectDef def)
            : base(def)
        {
        }

        protected override bool OutcomePossible(RitualOutcomePossibility chance, LordJob_Ritual ritual)
        {
            if (!chance.BestPositiveOutcome(ritual))
            {
                return true;
            }
            return ((LordJob_Ritual_Duel)ritual).duelists.Any((Pawn d) => d.Dead);
        }

        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            extraOutcomeDesc = null;
            if (outcome.Positive)
            {
                float amount = outcome.BestPositiveOutcome(jobRitual) ? RecreationGainBest : RecreationGainGood;
                float xp = outcome.BestPositiveOutcome(jobRitual) ? MeleeXPGainParticipantsBest : MeleeXPGainParticipantsGood;
                float xp2 = outcome.BestPositiveOutcome(jobRitual) ? MeleeXPGainSpectatorsBest : MeleeXPGainSpectatorsGood;
                LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)jobRitual;
                foreach (Pawn key in totalPresence.Keys)
                {
                    if (lordJob_Ritual_Duel.duelists.Contains(key))
                    {
                        key.skills.Learn(SkillDefOf.Melee, xp);
                    }
                    else
                    {
                        key.skills.Learn(SkillDefOf.Melee, xp2);
                        if (key.needs.joy != null)
                        {
                            key.needs.joy.GainJoy(amount, JoyKindDefOf.Social);
                        }
                    }
                }
            }
        }
    }
}