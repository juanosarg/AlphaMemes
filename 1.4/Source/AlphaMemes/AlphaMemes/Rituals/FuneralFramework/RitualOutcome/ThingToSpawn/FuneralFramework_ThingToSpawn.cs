using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{   //Using this for thingdef now
    //Allows for multiple things to be spawned, and ability to create comps to do different things easier
    public class FuneralFramework_ThingToSpawn
    {

        //spawning properties of the outcome

        public ThingDef thingDefToSpawn = null;
        public ThingDef stuffToUse = null;  //
        public ThingDef stuffDefToSpawn = null;
        public int thingCount = 0;
        public int stuffCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        public List<ThingDef> stuffOptions = new List<ThingDef>(); 
        public List<StuffCategoryDef> stuffCategoryDefs;
        public Dictionary<ThingDef, int> thingsRequired = new Dictionary<ThingDef, int>();//For specific things needed to start a ritual
        public bool stuffLastResult; 
    
        public virtual bool CanStartStuff(bool recheck = true)
        {
            Map map = Find.CurrentMap;
            if (stuffDefToSpawn != null && recheck)
            {
  
                recheck = map.resourceCounter.GetCount(stuffDefToSpawn) <= stuffCount;
                if (!recheck)
                {
                    recheck = !stuffOptions.Any(x => map.resourceCounter.GetCount(x) >= stuffCount);
                }              
            }


            if (recheck)
            {
                if(stuffCategoryDefs.Count>0 || stuffToUse != null)
                {
                    if (FindStuffForThing(recheck))
                    {
                        return true;
                    }
                    return false;
                }

            }
            return true;
            
        }
        //Maybe need to use listerthings as not all things are in resource. But if I have a ritual where I come across that might do something different as I fear perfomance
        //Can use target gets called seemingly every tick when selecting a building with gizmos 
        public virtual AcceptanceReport CanStartThings()
        {
            Map map = Find.CurrentMap;
            StringBuilder missing = new StringBuilder();
            if (thingsRequired.Count > 0)
            {
                foreach (KeyValuePair<ThingDef, int> thing in thingsRequired)
                {
                    if (map.resourceCounter.GetCount(thing.Key) <= thing.Value) 
                    {
                        missing.AppendLine(thing.Key.LabelCap + ": " + thing.Value);                        
                    }
                }                
            }
            if(missing.Length > 0)
            {
                return missing.ToString();
            }
            return true;
        }
        public virtual bool FindStuffForThing(bool recheck)
        {//
            
            if (stuffToUse != null)
            {

                stuffDefToSpawn = stuffToUse;
                return false;
            }
            if(stuffOptions.Count>0 && !recheck)//Obligation filters get called a lot so dont check resource list unless we have to
            {
                return true;
            }
            stuffOptions.Clear();
            foreach (KeyValuePair<ThingDef, int> allCountedAmount in Find.CurrentMap.resourceCounter.AllCountedAmounts) 
            {
                if (allCountedAmount.Key.IsStuff && stuffCategoryDefs.Any(x=> allCountedAmount.Key.stuffProps.categories.Contains(x)) && allCountedAmount.Value >= stuffCount)
                {
                    stuffOptions.Add(allCountedAmount.Key);
                }
            }
            if(stuffOptions.Count() > 0)
            {
                stuffDefToSpawn = stuffOptions.RandomElement();
                return true;
                
            }            
            return false;
        }
        public virtual void DestroyThingsUsed(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome)
        {
            RitualBehaviorWorker_FuneralFramework behaivor = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;            
            foreach (Thing thing in ritual.usedThings) //Handles removing things used to spawn 
            {
                if (thing.def == behaivor.stuffToUse)
                {
                    stuffDefToSpawn = thing.def;
                    thing.DeSpawn(DestroyMode.Vanish);
                }
                if (thingsRequired?.ContainsKey(thing.def)?? false)
                {
                    thing.DeSpawn(DestroyMode.Vanish);
                }
            }
        }

        public virtual Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome,Pawn pawnToSpawnOn, Corpse corpse)
        {
         
            ThingDef thingDef = CustomThingDefLogic(ritual,pawnToSpawnOn,corpse);
            thingDef = thingDef == null ? thingDefToSpawn : thingDef;
            if (thingDef == null)
            {
                return null;               
            }            
            int stacksize = thingCount;            
            ThingDef stuff = stuffDefToSpawn;
            if (thingDef.MadeFromStuff ? !GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined).Contains(stuff) :false)
            {
                stuff = GenStuff.DefaultStuffFor(thingDefToSpawn);
            }
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef, stuff);
            //initComps(thingToSpawn, corpse, ritual,bestOutcome,worstOutcome);
            if (thingToSpawn is Building)
            {
                thingToSpawn = thingToSpawn.MakeMinified();
            }
            if (worstOutcome)
            {
                stacksize = (int)Math.Round(stacksize * worstOutcomeMulti);
            }
            if (bestOutcome)
            {
                stacksize = (int)Math.Round(stacksize * bestOutcomeMulti);
            }
            if (stacksize <= 0)
            {
                return null;
            }
            thingToSpawn.stackCount = stacksize;
            
            return thingToSpawn;
        }

        
        //Seperated out so all processing is done on thing first.
        //Was going to put it in the effect worker but this allows making a separte thingToSpawn class rather then a full worker for variations
        public virtual void initComps(Thing thing, Corpse corpse, LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome, OutcomeChance outcome, RitualOutcomeEffectWorker_FuneralFramework worker)
        {
            //Im using the crafter outcome comp for this but it's not strictly required

            Thing thingTemp = thing.GetInnerIfMinified();
            RitualOutcomeComp_Crafter crafterComp = worker.GetComp<RitualOutcomeComp_Crafter>();
            int skill = 1;
            Pawn pawn = null;
            if (crafterComp != null)
            {
                pawn = ritual.assignments.FirstAssignedPawn(crafterComp.roleId);
                skill = pawn?.skills?.GetSkill(crafterComp.skill).levelInt ?? 1;
            }
            Comp_CorpseContainer corpseContainer = thingTemp.TryGetComp<Comp_CorpseContainer>();
            if (corpseContainer != null)
            {
                corpseContainer.InitComp_CorpseContainer(corpse);
            }
            CompQuality quality = thingTemp.TryGetComp<CompQuality>();
            if (quality != null)
            {                 

                QualityCategory thingQuality = QualityUtility.GenerateQualityCreatedByPawn(skill, bestOutcome);//If its best outcome its counted as inspired
                if (worstOutcome)
                {
                    //Going to slap whoever made qualityutility Addlevels private
                    int levels = -2;
                    thingQuality = (QualityCategory)Mathf.Min((int)(thingQuality + (byte)levels), 1);
                }
                quality.SetQuality(thingQuality, ArtGenerationContext.Colony);
                //Art generation doesnt matter we init below

            }
            CompArt art = thingTemp.TryGetComp<CompArt>();
            if (art != null)
            {
                art.InitializeArt(corpse.InnerPawn);
                if (pawn != null)
                {
                    art.JustCreatedBy(pawn);
                }

            }

        }
        public virtual ThingDef CustomThingDefLogic(LordJob_Ritual ritual, Pawn pawnToSpawnOn, Corpse corpse)
        {
            return null;
        }


    }
}
