using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class CompAbilityOcularConversion : CompAbilityEffect
    {
        private System.Random rand = new System.Random();

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            bool alphaAnimalsFound = DefDatabase<ThingDef>.GetNamedSilentFail("AA_AlienTree") != null;
            bool alphaBiomesFound = DefDatabase<ThingDef>.GetNamedSilentFail("AB_AlienTree") != null;

            if(!alphaAnimalsFound && !alphaBiomesFound)
            {
                Messages.Message("AM_OtherAlphaModsNeeded".Translate(), null, MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            return true;          
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            FleckMaker.Static(target.Cell, this.parent.pawn.Map, InternalDefOf.PsycastPsychicEffect);

            bool alphaAnimalsFound = DefDatabase<ThingDef>.GetNamedSilentFail("AA_AlienTree") != null;
            ThingDef treeDef;
            ThingDef grassDef;
            ThingDef plant1Def;
            ThingDef plant2Def;
            if (alphaAnimalsFound)
            {
                treeDef = DefDatabase<ThingDef>.GetNamedSilentFail("AA_AlienTree");
                grassDef = DefDatabase<ThingDef>.GetNamedSilentFail("AA_AlienGrass");
                plant1Def = DefDatabase<ThingDef>.GetNamedSilentFail("AA_RedLeaves");
                plant2Def = DefDatabase<ThingDef>.GetNamedSilentFail("AA_RedPlantsTall");
            }
            else
            {
                treeDef = DefDatabase<ThingDef>.GetNamedSilentFail("GU_AlienTree");
                grassDef = DefDatabase<ThingDef>.GetNamedSilentFail("GU_AlienGrass");
                plant1Def = DefDatabase<ThingDef>.GetNamedSilentFail("GU_RedLeaves");
                plant2Def = DefDatabase<ThingDef>.GetNamedSilentFail("GU_RedPlantsTall");
            }

            HashSet<Thing> hashSet = new HashSet<Thing>(target.Cell.GetThingList(this.parent.pawn.Map));
            if (hashSet != null)
            {
                foreach (Thing current in hashSet)
                {
                    Plant plantTarget = current as Plant;
                    if (plantTarget != null)
                    {
                        PlantProperties plant = plantTarget.def.plant;
                        bool flag = (plant != null);
                        if (flag)
                        {
                            if (plant.IsTree && (plantTarget.def.defName != "GU_AlienTree") && (plantTarget.def.defName != "AA_AlienTree") && (plantTarget.def.defName != "Plant_TreeAnima") && (plantTarget.def.defName != "Plant_TreeGauranlen"))
                            {
                                Plant thing2 = (Plant)GenSpawn.Spawn(treeDef, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                Plant thingToDestroy = (Plant)plantTarget;
                                thing2.Growth = thingToDestroy.Growth;
                                plantTarget.Destroy();
                            }
                            else if (!plant.IsTree && (plantTarget.def.defName != "GU_AlienGrass") && (plantTarget.def.defName != "GU_RedLeaves") && (plantTarget.def.defName != "GU_RedPlantsTall")
                               && (plantTarget.def.defName != "AA_AlienGrass") && (plantTarget.def.defName != "AA_RedLeaves") && (plantTarget.def.defName != "AA_RedPlantsTall") && (plantTarget.def.defName != "Plant_GrassAnima")
                               )
                            {
                                if (rand.NextDouble() < 0.4)
                                {
                                    Plant thing2 = (Plant)GenSpawn.Spawn(grassDef, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                    Plant thingToDestroy = (Plant)plantTarget;
                                    thing2.Growth = thingToDestroy.Growth;
                                    plantTarget.Destroy();
                                }
                                else if (rand.NextDouble() > 0.4 && rand.NextDouble() < 0.7)
                                {
                                    Plant thing2 = (Plant)GenSpawn.Spawn(plant1Def, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                    Plant thingToDestroy = (Plant)plantTarget;
                                    thing2.Growth = thingToDestroy.Growth;
                                    plantTarget.Destroy();
                                }
                                else if (rand.NextDouble() > 0.7)
                                {
                                    Plant thing2 = (Plant)GenSpawn.Spawn(plant2Def, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                    Plant thingToDestroy = (Plant)plantTarget;
                                    thing2.Growth = thingToDestroy.Growth;
                                    plantTarget.Destroy();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
