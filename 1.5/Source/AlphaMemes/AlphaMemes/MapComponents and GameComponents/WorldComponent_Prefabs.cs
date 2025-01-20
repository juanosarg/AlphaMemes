using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using AlphaPrefabs;
using UnityEngine.UIElements;
using Verse.Noise;
using System.Linq;
using RimWorld.Planet;

namespace AlphaMemes
{
    public class WorldComponent_Prefabs : WorldComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 3600000; //1 year



        public static WorldComponent_Prefabs Instance;

        public WorldComponent_Prefabs(World world) : base(world) => Instance = this;



        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterPrefabs", 0, true);

        }

        public override void WorldComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModLister.HasActiveModWithName("Alpha Prefabs"))
                        {
                            if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_PrefabAcquisition_Easy) != null)
                            {
                                List<ThingDef> possiblePrefabs = new List<ThingDef>() { AlphaPrefabs.InternalDefOf.AP_Prefab_LowValue, AlphaPrefabs.InternalDefOf.AP_Prefab,
                                AlphaPrefabs.InternalDefOf.AP_Prefab_MediumHighValue,AlphaPrefabs.InternalDefOf.AP_Prefab_HighValue};
                                ThingDef chosenPrefab = possiblePrefabs.RandomElement();
                                Thing newPrefab = ThingMaker.MakeThing(chosenPrefab);
                                Map map = Find.CurrentMap;
                                LookTargets targets = null;
                                if (!map.IsPlayerHome)
                                {
                                    map = Find.RandomPlayerHomeMap;
                                }
                                IntVec3 position;
                                Thing caravanSpot = null;
                                List<Thing> caravanSpots = map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
                                if(caravanSpots.Count > 0)
                                {
                                    caravanSpot = caravanSpots.RandomElement();
                                }
                               
                                if (caravanSpot == null)
                                {
                                    IntVec3 cell = IntVec3.Invalid;
                                    RCellFinder.TryFindRandomSpotJustOutsideColony(Find.CameraDriver.MapPosition, map, out cell);
                                    position = cell;
                                    targets = new LookTargets(newPrefab);
                                }
                                else
                                {
                                    position = caravanSpot.Position;
                                    targets = new LookTargets(caravanSpot);
                                }
                                DropPodUtility.DropThingsNear(position, map, new List<Thing>() { newPrefab }, 110, false, false, false, false);
                                Find.LetterStack.ReceiveLetter("AM_LetterLabelPrefabDelivered".Translate(), "AM_LetterPrefabDelivered".Translate(), LetterDefOf.PositiveEvent, targets);

                            }
                        }
                    }))();
                }
                catch (TypeLoadException) { }



                

                

                tickCounter = 0;
            }



        }


    }


}
