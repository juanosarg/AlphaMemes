using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace AlphaMemes
{
    public class MapComponent_FurnitureTracker : MapComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 10000;
        public int artInTheMap_backup = 0;
        public int reliquariesInTheMap_backup = 0;
        public float artBeautyInTheMap_backup = 0;
        public int megalithsInTheMap_backup = 0;




        public MapComponent_FurnitureTracker(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            if (map.IsPlayerHome)
            {
                StaticCollectionsClass.artInTheMap = artInTheMap_backup;
                StaticCollectionsClass.artBeautyInTheMap = artBeautyInTheMap_backup;
                StaticCollectionsClass.reliquariesInTheMap = reliquariesInTheMap_backup;
                StaticCollectionsClass.megalithsInTheMap = megalithsInTheMap_backup;

            }
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.artInTheMap_backup, "artInTheMap_backup", 0, true);
            Scribe_Values.Look<float>(ref this.artBeautyInTheMap_backup, "artBeautyInTheMap_backup", 0, true);
            Scribe_Values.Look<int>(ref this.reliquariesInTheMap_backup, "reliquariesInTheMap_backup", 0, true);
            Scribe_Values.Look<int>(ref this.megalithsInTheMap_backup, "megalithsInTheMap_backup", 0, true);


            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterFurniture", 0, true);

        }
        public override void MapComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {


                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Art_Desired) != null|| Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_ArtQuality_Expected) != null))
                {
                    artInTheMap_backup = StaticCollectionsClass.artInTheMap;
                    artBeautyInTheMap_backup = StaticCollectionsClass.artBeautyInTheMap;

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
                                artTotalBeauty += QualityToValue(quality);
                            }
                           
                        }
                    }

                    float artRate;
                    if (art == 0)
                    {
                        artRate = 0;
                    }
                    else artRate = artTotalBeauty / art;

                    artInTheMap_backup = art;
                    artBeautyInTheMap_backup = artRate;
                    StaticCollectionsClass.artInTheMap = artInTheMap_backup;
                    StaticCollectionsClass.artBeautyInTheMap = artBeautyInTheMap_backup;
                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Reliquaries_Forbidden) != null ))
                {
                    int reliquaryCount = map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.Reliquary).EnumerableCount();
                    reliquariesInTheMap_backup = reliquaryCount;                 
                    StaticCollectionsClass.reliquariesInTheMap = reliquaryCount;

                }

                if (map.IsPlayerHome && (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Megaliths_Desired) != null))
                {
                    int megalithCount = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_Megalith).EnumerableCount();
                    megalithsInTheMap_backup = megalithCount;
                    StaticCollectionsClass.megalithsInTheMap = megalithCount;

                }


                tickCounter = 0;
            }



        }

        public int QualityToValue(QualityCategory quality)
        {
            switch (quality)
            {
                case QualityCategory.Awful:
                    return 0;
                case QualityCategory.Poor:
                    return 1;
                case QualityCategory.Normal:
                    return 2;
                case QualityCategory.Good:
                    return 3;
                case QualityCategory.Excellent:
                    return 4;
                case QualityCategory.Masterwork:
                    return 5;
                case QualityCategory.Legendary:
                    return 6;
                default:
                    return 0;

            }

        }




    }


}



