using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;
using System.Threading.Tasks;

namespace AlphaMemes
{   //Using this for thingdef now
    //Allows for multiple things to be spawned, and ability to create comps to do different things easier
    public class FuneralFramework_ThingToSpawnGenetics : FuneralFramework_ThingToSpawn
    {


        //spawning properties of the outcome
        public override Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome, Pawn pawnToSpawnOn, Corpse corpse)
        {
            ThingDef thingDef = null;
            thingDef = CustomThingDefLogic(ritual, pawnToSpawnOn, corpse);
            if(thingDef != null)
            {
                base.thingDefToSpawn = thingDef;
            }
            else
            {
                //If no genome do a template \o/
                thingDef = DefDatabase<ThingDef>.AllDefsListForReading.First(x => x.defName == "GR_TemplateGenetic");
                base.thingDefToSpawn = thingDef;
            }

            return base.GetThingToSpawn(ritual, bestOutcome, worstOutcome, pawnToSpawnOn, corpse);
        }
        public override ThingDef CustomThingDefLogic(LordJob_Ritual ritual, Pawn pawnToSpawnOn, Corpse corpse)
        {

            ThingDef thingDef = null;
            thingDef = (ThingDef)Traverse.CreateWithType("GeneticRim.StaticCollectionsClass")?.Method("GenomeForPawn", new Type[] { typeof(Pawn) }).GetValue(corpse.InnerPawn);
            return thingDef;
        }

    }
}
