using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Universal No corpse trigger
    public class RitualObligationTrigger_NoCorpse : RitualObligationTrigger
    {
        public override void Notify_MemberCorpseDestroyed(Pawn p)
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }
            if ((!Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || p.HomeFaction != Faction.OfPlayer || !p.IsFreeNonSlaveColonist || p.IsKidnapped())
            {
                return;
            }
            if (ModsConfig.AnomalyActive && (p.IsCreepJoiner || p.IsMutant || p.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse)))
            {
                return;
            }
            List<Precept_Ritual> rituals = ritual.ideo.GetAllPreceptsOfType<Precept_Ritual>().ToList();
            if (rituals.Any(x => x.def.GetModExtension<FuneralPreceptExtension>()?.addNoCorpseFuneral == true))
            {
                this.ritual.AddObligation(new RitualObligation(this.ritual, p, true));
            }
        }

        public override void Notify_RitualExecuted(LordJob_Ritual ritualJob)
        {
            base.Notify_RitualExecuted(ritualJob);

            // Only run the cleanup code for AM's no funeral corpse, and no other rituals that (may) use this obligation trigger.
            if (ritual?.def.defName != "AM_FuneralNoCorpse")
            {
                return;
            }

            // Make sure the lord job and ritual aren't null and the obligation has a target (corpse).
            if (ritualJob?.Ritual?.ideo == null || ritualJob.obligation?.FirstValidTarget.HasThing != true)
            {
                return;
            }

            // Only run the cleanup if the ritual that just ended is a funeral.
            if (ritualJob.Ritual.def.defName != "AM_FuneralNoCorpse" && !ritualJob.Ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                return;
            }

            var corpse = ritualJob.obligation.targetA;

            foreach (var precept in ritualJob.Ritual.ideo.GetAllPreceptsOfType<Precept_Ritual>().Where(x => x.activeObligations != null))
            {
                // Only remove obligations from funeral rituals.
                if (precept.def.HasModExtension<FuneralPreceptExtension>() || precept.def.defName == "AM_FuneralNoCorpse")
                {
                    // Using a reverse for loop since we're removing stuff from the list as we go
                    for (var i = precept.activeObligations.Count - 1; i >= 0; i--)
                    {
                        var ritualObligation = precept.activeObligations[i];
                        if (ritualObligation.FirstValidTarget == corpse)
                        {
                            precept.RemoveObligation(ritualObligation);
                        }
                    }
                }
            }
        }
    }        



}
