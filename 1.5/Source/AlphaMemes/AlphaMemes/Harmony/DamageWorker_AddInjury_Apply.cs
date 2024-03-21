using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System;
using Verse.AI;



namespace AlphaMemes
{


    [HarmonyPatch(typeof(DamageWorker_AddInjury))]
    [HarmonyPatch("Apply")]
    public static class AlphaMemes_DamageWorker_AddInjury_Apply_Patch
    {
        [HarmonyPostfix]
        static void SendHistoryIfMelee(DamageInfo dinfo, Thing thing)
        {
            Pawn instigatorPawn = dinfo.Instigator as Pawn;

            if (instigatorPawn != null)
            {
                if (instigatorPawn.Ideo?.HasPrecept(InternalDefOf.AM_CombatProwess_Increased) == true)
                {

                    Pawn victimPawn = thing as Pawn;

                    if (victimPawn != null && dinfo.Weapon?.IsRangedWeapon == false)
                    {

                        Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_UsedMelee, dinfo.Instigator.Named(HistoryEventArgsNames.Doer)), true);

                    }


                }


                

                if (instigatorPawn.Ideo?.HasPrecept(InternalDefOf.AM_CombatProwess_Melee) == true)
                {

                    Pawn victimPawn = thing as Pawn;

                    if (victimPawn != null)
                    {
                        if(dinfo.Weapon?.IsRangedWeapon == true && dinfo.Weapon?.Verbs?.FirstOrFallback()?.range >2)
                        {
                            if(dinfo.Weapon?.building == null || dinfo.Weapon?.building.buildingTags.Contains("Artillery_BaseDestroyer")!=true)
                            {
                                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_UsedRanged, dinfo.Instigator.Named(HistoryEventArgsNames.Doer)), true);
                            }
                            

                        } else if (dinfo.Weapon?.IsMeleeWeapon == true || dinfo.Weapon?.IsNaturalOrgan == true)
                        {
                            System.Random random = new System.Random();
                            if (random.NextDouble() < 0.2)
                            {

                                DamageInfo dinfo2 = dinfo;
                                dinfo2.Def = DamageDefOf.Stun;
                                dinfo2.SetAmount(10f);
                                victimPawn.TakeDamage(dinfo2);

                               

                            }
                        }


                    }


                }


            }







        }
    }








}
