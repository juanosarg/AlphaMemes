using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace AlphaMemes
{
    public class MapComponent_DryadTracker : MapComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 10000;
     



        public MapComponent_DryadTracker(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            if (DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Ocular") != null)
            {
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Ocular"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Corruptor"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Spitter"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Unstable"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("AA_Dryad_Tumorous"));

            }

            if (DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_Dryad_Stonedigger") != null)
            {
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_Dryad_Stonedigger"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_Dryad_Gaubricmaker"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_Dryad_Nectarmaker"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_Dryad_Spitter"));

                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Carrier"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Woodmaker"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Medicinemaker"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Berrymaker"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Stonedigger"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Nectarmaker"));
                StaticCollectionsClass.AddUtilityDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Gaubricmaker"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Clawer"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Barkskin"));
                StaticCollectionsClass.AddCombatDryad(DefDatabase<PawnKindDef>.GetNamedSilentFail("VDE_AwakenedDryad_Spitter"));

            }



            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
           
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterDryads", 0, true);

        }
        public override void MapComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {

                if(Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Dryads_Enhanced) != null)
                {
                    List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
                    List<Pawn> dryadList = new List<Pawn>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].RaceProps.Dryad || list[i].kindDef == InternalDefOf.AM_UnshackledDryad || list[i].def.defName.Contains("VDE_AwakenedDryad"))
                        {
                            dryadList.Add(list[i]);
                        }
                    }


                    foreach (Pawn dryad in dryadList)
                    {
                        if (StaticCollectionsClass.utilityDryads.Contains(dryad.kindDef))
                        {
                            if (dryad.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.AM_UtilityDryadHediff) == null)
                            {
                                dryad.health.AddHediff(InternalDefOf.AM_UtilityDryadHediff);
                            }
                        }else if (StaticCollectionsClass.combatDryads.Contains(dryad.kindDef))
                        {
                            if (dryad.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.AM_CombatDryadHediff) == null)
                            {
                                dryad.health.AddHediff(InternalDefOf.AM_CombatDryadHediff);
                            }
                        }
                        else
                        {
                            if (dryad.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.AM_GenericDryadHediff) == null)
                            {
                                dryad.health.AddHediff(InternalDefOf.AM_GenericDryadHediff);
                            }

                        }



                    }
                }





                tickCounter = 0;
            }



        }

       




    }


}



