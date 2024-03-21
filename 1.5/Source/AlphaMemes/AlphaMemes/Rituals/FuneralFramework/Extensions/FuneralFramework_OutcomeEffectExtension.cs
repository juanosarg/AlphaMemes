using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
    
    public class OutcomeEffectExtension : DefModExtension
    {   //Every funeral is going to have their down outcome effect def anyway 
        public bool stripcorpse = false;
        public bool destroyCorpse = true; //Default true, false for something like a thing container
        public bool despawnCorpse = false;
        public string roleToSpawnOn = "AM_RitualRoleCorpse";
        public string bestOutcomeDesc = null;
        public string worstOutcomeDesc = null;
        public List<ThingDef> relevantThingDefs = new List<ThingDef>();//Just general list for if you want to put a list of thingdefs to use in outcome. Not implemented in anything generic
        public List<FuneralFramework_ThingToSpawn> outcomeSpawners;
    }




}
