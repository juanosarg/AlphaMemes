
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class JobDriver_HeartExtraction : JobDriver
    {
        private const TargetIndex VictimIndex = TargetIndex.A;

        private const TargetIndex StandingIndex = TargetIndex.B;

        protected Pawn Victim => (Pawn)job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (!ModLister.CheckIdeology("Sacrifice"))
            {
                yield break;
            }
            this.FailOnDestroyedOrNull(TargetIndex.A);
            Pawn victim = Victim;
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
            yield return Toils_General.Wait(35);
            Toil execute = ToilMaker.MakeToil("MakeNewToils");
            execute.initAction = delegate
            {
                Lord lord = pawn.GetLord();
                if (lord != null)
                {
                    (lord.LordJob as LordJob_Ritual)?.pawnsDeathIgnored.Add(victim);
                }

                BodyPartRecord bodyPartRecord = victim.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).
                                  FirstOrDefault((BodyPartRecord x) => x.def == InternalDefOf.Heart);

                if (bodyPartRecord != null)
                {
                    ThingDef blood = ThingDefOf.Filth_Blood;
                                  

                    for (int i = 0; i < 40; i++)
                    {
                        IntVec3 c;
                        CellFinder.TryFindRandomReachableCellNearPosition(victim.Position, victim.Position, victim.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                        FilthMaker.TryMakeFilth(c, victim.Map, blood);

                    }

                    int num= (int)victim.health?.hediffSet?.GetPartHealth(bodyPartRecord) + 1000;

                    
                    DamageInfo damageInfo = new DamageInfo(DamageDefOf.Cut, (float)num, 999f, -1f, pawn, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
                    damageInfo.SetAllowDamagePropagation(false);
                    victim.TakeDamage(damageInfo);

                }



                ThoughtUtility.GiveThoughtsForPawnExecuted(victim, pawn, PawnExecutionKind.GenericBrutal);
                TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, pawn, victim);
                victim.health.killedByRitual = true;
            };
            execute.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return Toils_Reserve.Release(TargetIndex.A);
            yield return execute;
        }
    }
}