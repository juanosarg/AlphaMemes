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
    public class FuneralFramework_ThingToSpawnPaste : FuneralFramework_ThingToSpawn
    {


        //spawning properties of the outcome
        public override Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome, Pawn pawnToSpawnOn, Corpse corpse)
        {
            ThingDef thingDef = thingDefToSpawn;
            float nutrition = corpse.GetStatValue(StatDefOf.Nutrition, true);           
            //paste dispenser .3 per nutrition
            int pasteToSpawn = (int)Math.Ceiling(nutrition / .3);
            Thing paste = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste);
            
            CompIngredients comp = paste.TryGetComp<CompIngredients>();
            ThingDef meat = corpse.InnerPawn.RaceProps.meatDef;
            if(meat != null)
            {
                comp.RegisterIngredient(meat);
            }
            
            if (worstOutcome)
            {
                pasteToSpawn = (int)Math.Round(pasteToSpawn * worstOutcomeMulti);
            }
            if (bestOutcome)
            {
                pasteToSpawn = (int)Math.Round(pasteToSpawn * bestOutcomeMulti);
            }
            if (pasteToSpawn <= 0)
            {
                return null;
            }
            paste.stackCount = pasteToSpawn;
            return paste;
        }


    }
}
