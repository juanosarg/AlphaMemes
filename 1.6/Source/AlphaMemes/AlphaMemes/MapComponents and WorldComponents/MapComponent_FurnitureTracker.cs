using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace AlphaMemes
{
    public class MapComponent_FurnitureTracker : MapComponent
    {



        public int tickCounter = tickInterval;
        public const int tickInterval = 10000;
       

        public MapComponent_FurnitureTracker(Map map) : base(map)
        {

        }

       
        public override void MapComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {


                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Art_Desired) != null|| Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_ArtQuality_Expected) != null))
                {
                   
                    List<Thing> artThings = map.listerThings.ThingsInGroup(ThingRequestGroup.Art);

                   
                    int art = 0;
                    float artTotalBeauty = 0;

                    foreach (Thing building in artThings)
                    {
                        Building_Art buildingArt = building as Building_Art;
                        if (buildingArt != null && buildingArt.Faction?.IsPlayer==true) {

                            CompArt compArt = buildingArt.TryGetComp<CompArt>();
                            if (compArt!=null && compArt.AuthorName!=null)
                            {
                                art++;
                                QualityCategory quality;
                                buildingArt.TryGetQuality(out quality);
                                artTotalBeauty += (int)quality;
                            }
                           
                        }
                    }

                    float artRate;
                    if (art == 0)
                    {
                        artRate = 0;
                    }
                    else artRate = artTotalBeauty / art;
                
                    StaticCollections.SetArtInTheMap(map,art);
                    StaticCollections.SetArtBeautyInTheMap (map,artRate);
                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Reliquaries_Forbidden) != null ))
                {
                    int reliquaryCount = map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Reliquary).EnumerableCount();
                                
                    StaticCollections.SetReliquariesInTheMap(map,reliquaryCount);

                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_DeathrestCaskets_Abhorrent) != null))
                {
                    int casketCount = map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.DeathrestCasket).EnumerableCount();
                    int draincasketCount = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VRE_Draincasket).EnumerableCount();
                    StaticCollections.SetDeathrestCasketsInTheMap(map, casketCount);
                    StaticCollections.SetDraincasketsInTheMap(map, draincasketCount);
                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Megaliths_Desired) != null))
                {
                    int megalithCount = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_Megalith).EnumerableCount();               
                    StaticCollections.SetMegalithsInTheMap(map,megalithCount);
                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Baths_Desired) != null))
                {
                    int bathtubs = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.BathtubStuff).EnumerableCount();
                    int showers = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.ShowerStuff).EnumerableCount() +
                        map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.ShowerSimple).EnumerableCount() +
                        map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.ShowerAdvStuff).EnumerableCount();
                    StaticCollections.SetBathsAndShowersInTheMap(map, bathtubs+showers);

                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_AnimaScreams_Delightful) != null))
                {
                    int trees = map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima).Count();                
                    StaticCollections.SetAnimaTreesInTheMap(map, trees);
                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Ranching_CattleCentered) != null))
                {
              
                    int pens = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.PenMarker).Where(x => x.TryGetComp<CompAnimalPenMarker>()?.PenState.Enclosed==true).EnumerableCount();

                    StaticCollections.SetPenMarkersInTheMap(map, pens);

                }


                tickCounter = 0;
            }



        }

      




    }


}



