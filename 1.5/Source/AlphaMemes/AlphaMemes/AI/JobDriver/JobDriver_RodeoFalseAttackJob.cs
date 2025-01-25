using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace AlphaMemes
{
    public class JobDriver_RodeoFalseAttackJob : JobDriver
    {
        private const int AutotargetRadius = 4;

        private int numMeleeAttacksMade;

       

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref numMeleeAttacksMade, "numMeleeAttacksMade", 0);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            IAttackTarget attackTarget = job.targetA.Thing as IAttackTarget;
            if (attackTarget != null)
            {
                pawn.Map.attackTargetReservationManager.Reserve(pawn, job, attackTarget);
            }
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddFinishAction(delegate
            {
                if (pawn.IsPlayerControlled && pawn.Drafted && !base.job.playerInterruptedForced && (base.TargetThingA?.def.autoTargetNearbyIdenticalThings ?? false))
                {
                    foreach (IntVec3 item in GenRadial.RadialCellsAround(base.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
                    {
                        if (item.InBounds(base.Map))
                        {
                            foreach (Thing thing2 in item.GetThingList(base.Map))
                            {
                                if (thing2.def == base.TargetThingA.def && pawn.CanReach(thing2, PathEndMode.Touch, Danger.Deadly) && pawn.jobs.jobQueue.Count == 0)
                                {
                                    Job job = base.job.Clone();
                                    job.targetA = thing2;
                                    pawn.jobs.jobQueue.EnqueueFirst(job);
                                    return;
                                }
                            }
                        }
                    }
                }
            });
            yield return Toils_General.DoAtomic(delegate
            {

                Pawn pawn;
                if ((pawn = job.targetA.Thing as Pawn) != null)
                {
                    MoteMaker.MakeSpeechBubble(pawn, StaticCollections.moteIcon);
                    InternalDefOf.AM_Rodeo.PlayOneShot(pawn);
                    bool num = pawn.Downed && base.pawn.mindState.duty != null && base.pawn.mindState.duty.attackDownedIfStarving && base.pawn.Starving();
                    CompActivity comp;
                    bool flag = ModsConfig.AnomalyActive && pawn.TryGetComp<CompActivity>(out comp) && comp.IsDormant;
                    if (num || flag)
                    {
                        job.killIncappedTarget = true;
                    }
                }
            });
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate
            {
                MoteMaker.MakeSpeechBubble(pawn, StaticCollections.moteIcon);
                if (Rand.Chance(0.5f))
                {
                    InternalDefOf.AM_Rodeo_Yeehaw.PlayOneShot(pawn);

                }
                Thing thing = job.GetTarget(TargetIndex.A).Thing;
                Pawn p;
                if (job.reactingToMeleeThreat && (p = thing as Pawn) != null && !p.Awake())
                {
                    EndJobWith(JobCondition.InterruptForced);
                }
                else if (pawn.CurJob != null && pawn.jobs.curDriver == this)
                {
                    Lord lord = pawn.GetLord();
                    LordJob_Ritual_Rodeo lordJob_Ritual_Duel;
                    if (lord?.LordJob != null && (lordJob_Ritual_Duel = lord.LordJob as LordJob_Ritual_Rodeo) != null)
                    {
                        lordJob_Ritual_Duel.Notify_MeleeAttack(pawn, thing);
                    }
                    numMeleeAttacksMade++;
                    if (numMeleeAttacksMade >= job.maxNumMeleeAttacks)
                    {
                        EndJobWith(JobCondition.Succeeded);
                    }
                }
            }).FailOnDespawnedOrNull(TargetIndex.A);
        }

        public override void Notify_PatherFailed()
        {
            if (job.attackDoorIfTargetLost)
            {
                Thing thing;
                using (PawnPath pawnPath = base.Map.pathFinder.FindPath(pawn.Position, base.TargetA.Cell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors)))
                {
                    if (!pawnPath.Found)
                    {
                        return;
                    }
                    thing = pawnPath.FirstBlockingBuilding(out var _, pawn);
                }
                if (thing != null && thing.Position.InHorDistOf(pawn.Position, 6f))
                {
                    job.targetA = thing;
                    job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
                    job.expiryInterval = Rand.Range(2000, 4000);
                    return;
                }
            }
            base.Notify_PatherFailed();
        }

        public override bool IsContinuation(Job j)
        {
            return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
        }
    }
}