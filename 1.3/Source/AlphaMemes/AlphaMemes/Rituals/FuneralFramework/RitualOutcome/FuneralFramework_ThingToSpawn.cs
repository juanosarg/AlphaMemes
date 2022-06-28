using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
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
        public List<ThingDef> stuffOptions = new List<ThingDef>(); //If I solve above this will be better
        public List<StuffCategoryDef> stuffCategoryDefs;

        
        public virtual bool CanStart(bool recheck = true)
        {
            Map map = Find.CurrentMap;
            if (stuffDefToSpawn != null && !recheck)
            {
                recheck = map.resourceCounter.GetCount(stuffDefToSpawn) <= stuffCount;
                if (!recheck)
                {
                    recheck = !stuffOptions.Any(x => map.resourceCounter.GetCount(x) >= stuffCount);
                }
               
                
            }
            if (recheck)
            {
                FindStuffForThing(recheck);
                foreach(ThingDef thing in stuffOptions)
                {
                    if (map.resourceCounter.GetCount(thing) >= stuffCount)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
            
        }

        public virtual void FindStuffForThing(bool recheck)
        {//
            
            if (stuffToUse != null)
            {

                stuffDefToSpawn = stuffToUse;
                return;
            }
            if(stuffOptions.Count>0 && !recheck)//Obligation filters get called a lot so dont check resource list unless we have to
            {
                return;
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
                
            }            
            
        }

        public virtual Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome,Pawn pawnToSpawnOn, Corpse corpse)
        {
            //This shit is wonky and I have no idea if it'll work. But itd be kinda neat if it did.
            //Though I should find a better way            

            ThingDef thingDef = CustomThingDefLogic(ritual,pawnToSpawnOn,corpse);
            thingDef = thingDef == null ? thingDefToSpawn : thingDef;
            if (thingDef == null)
            {
                return null;               
            }            
            int stacksize = thingCount;
            RitualBehaviorWorker_FuneralFramework behaivor = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;
            ThingDef stuff = stuffDefToSpawn;//fallback
            foreach (Thing thing in ritual.usedThings)
            {
                if(thing.def == behaivor.stuffToUse)
                {
                    stuff = thing.def;
                    thing.DeSpawn(DestroyMode.Vanish);
                    break;
                }
            }
            
            if (thingDef.MadeFromStuff ? !GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined).Contains(stuff) :false)
            {
                stuff = GenStuff.DefaultStuffFor(thingDefToSpawn);
            }
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef, stuff);
            initComps(thingToSpawn, corpse);
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
        public virtual ThingDef CustomThingDefLogic(LordJob_Ritual ritual, Pawn pawnToSpawnOn, Corpse corpse)
        {
            return null;
        }

        public virtual void initComps(Thing thing,Corpse corpse)//Probably going to need more
        {
            Comp_CorpseContainer comp = thing.TryGetComp<Comp_CorpseContainer>();
            if(comp != null)
            {
                comp.InitComp_CorpseContainer(corpse);
            }
                

        }
    }
}
