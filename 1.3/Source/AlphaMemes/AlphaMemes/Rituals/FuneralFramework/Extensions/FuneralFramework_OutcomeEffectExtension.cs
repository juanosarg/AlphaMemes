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
        public string roleToSpawnOn = "AM_RitualRoleCorpse";
        public string bestOutcomeDesc = null;
        public string worstOutcomeDesc = null;
        public List<FuneralFramework_ThingToSpawn> outcomeSpawners;
    }




}
