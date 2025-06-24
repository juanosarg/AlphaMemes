using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class CompAbilityOcularConversion : CompAbilityEffect
    {
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            bool alphaAnimalsFound = InternalDefOf.AA_AlienTree != null;
            bool alphaBiomesFound = InternalDefOf.AB_AlienTree != null;

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

            bool alphaAnimalsFound = InternalDefOf.AA_AlienTree != null;
          
            ThingDef treeDef = null;
            ThingDef grassDef = null;
            ThingDef plant1Def = null;
            ThingDef plant2Def = null;
            if (alphaAnimalsFound)
            {
                treeDef = InternalDefOf.AA_AlienTree;
                grassDef = InternalDefOf.AA_AlienGrass;
                plant1Def = InternalDefOf.AA_RedLeaves;
                plant2Def = InternalDefOf.AA_RedPlantsTall;
            }
            else 
            {
                treeDef = InternalDefOf.AB_AlienTree;
                grassDef = InternalDefOf.AB_AlienGrass;
                plant1Def = InternalDefOf.AB_RedLeaves;
                plant2Def = InternalDefOf.AB_RedPlantsTall;
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
                                if (plant.IsTree && (plantTarget.def != InternalDefOf.AB_AlienTree) && (plantTarget.def != InternalDefOf.AA_AlienTree) && (plantTarget.def != ThingDefOf.Plant_TreeAnima) && (plantTarget.def != ThingDefOf.Plant_TreeGauranlen))
                                {
                                    Plant thing2 = (Plant)GenSpawn.Spawn(treeDef, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                    Plant thingToDestroy = (Plant)plantTarget;
                                    thing2.Growth = thingToDestroy.Growth;
                                    plantTarget.Destroy();
                                }
                                else if (!plant.IsTree && (plantTarget.def != InternalDefOf.AB_AlienGrass) && (plantTarget.def != InternalDefOf.AB_RedLeaves) && (plantTarget.def != InternalDefOf.AB_RedPlantsTall)
                                   && (plantTarget.def != InternalDefOf.AA_AlienGrass) && (plantTarget.def != InternalDefOf.AA_RedLeaves) && (plantTarget.def != InternalDefOf.AA_RedPlantsTall) && (plantTarget.def != ThingDefOf.Plant_GrassAnima) && (plantTarget.def != ThingDefOf.Plant_PodGauranlen)
                                   && (plantTarget.def != InternalDefOf.AM_Plant_CorruptedPodGauranlen)
                                   )
                                {
                                    var value = Rand.Value;
                                    if (value < 0.4)
                                    {
                                        Plant thing2 = (Plant)GenSpawn.Spawn(grassDef, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                        Plant thingToDestroy = (Plant)plantTarget;
                                        thing2.Growth = thingToDestroy.Growth;
                                        plantTarget.Destroy();
                                    }
                                    else if (value < 0.7)
                                    {
                                        Plant thing2 = (Plant)GenSpawn.Spawn(plant1Def, plantTarget.Position, plantTarget.Map, WipeMode.Vanish);
                                        Plant thingToDestroy = (Plant)plantTarget;
                                        thing2.Growth = thingToDestroy.Growth;
                                        plantTarget.Destroy();
                                    }
                                    else
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
