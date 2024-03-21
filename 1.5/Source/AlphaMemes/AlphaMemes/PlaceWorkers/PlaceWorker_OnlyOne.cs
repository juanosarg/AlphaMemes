using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaMemes
{
    public class PlaceWorker_OnlyOne : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            int numberOfDatabases = 0;

            foreach (Map existingMap in Current.Game.Maps)
            {
                if (existingMap.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_AnimalDatabase).Count()>0)
                {
                    numberOfDatabases++;
                    
                }
            }
            if (numberOfDatabases != 0)
            {
                return new AcceptanceReport("AM_OnlyOneDatabase".Translate());
            }



            return true;
        }

       




    }


}


