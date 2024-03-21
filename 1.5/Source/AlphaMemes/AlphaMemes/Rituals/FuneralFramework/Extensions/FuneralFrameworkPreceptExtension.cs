using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{

    public class FuneralPreceptExtension : DefModExtension
    {
        public List<BuildableDef> addDesignators = new List<BuildableDef>();
        public bool isColonistFuneral = false; //Can only have 1 colonist funeral type
        public bool replaceConflictRituals = false; //Decides whether to replace vanilla funeral on generation       
        public bool addNoCorpseFuneral = true;//Adds the base game funeral for no corpse (empty grave)
        public string corpseRitualRoleID = "AM_RitualRoleCorpse"; //add the role here if its not default

        //Weighting stuff
        public float _weighting = 0f;//This is for replacing vanilla funeral on RNG generation.
        public Dictionary<StyleCategoryDef,float> associatedStyles = new Dictionary<StyleCategoryDef, float>();//Used only for weighting purposes
        public Dictionary<MemeDef, float> associatedMemes = new Dictionary<MemeDef, float>();//Used only for weighting purposes

        //Animals
        public RitualObiligationTrigger_Animals animalObligationTrigger;
        public bool allowAnimals = false;

        //Special Conflicts
        public FuneralFramework_SpecialConflicts specialConflicts;

        public float Weighting(Ideo ideo, PreceptDef def)
        {
            float mod = 0f;
            if(!associatedMemes.NullOrEmpty())
            {
                foreach (KeyValuePair<MemeDef, float> kvp in associatedMemes)
                {
                    if (ideo.memes.Contains(kvp.Key))
                    {
                        mod += kvp.Value;
                    }
                }
            }
            if (!associatedStyles.NullOrEmpty())
            {
                foreach(KeyValuePair<StyleCategoryDef, float> kvp in associatedStyles)
                {
                    if (ideo.thingStyleCategories.Any(x => x.category == kvp.Key))
                    {
                        mod += kvp.Value;
                    }
                }
            }
            if(mod > 0f)
            {
               return _weighting * mod;
            }
            return _weighting;
        }
        //Simple way to find if we can add it
        public bool CanAddPrecept(Ideo ideo,PreceptDef def,FactionDef faction = null)
        {
            //I think i've lost my mind. Something in here null errored during world generation randomly while I wasnt even testing. and I couldnt replicate it in about 50+ new world gens when I actually had debugger attached
            //So if you look at this and go why is this bubble wrap extreme. Thats why
            //My best guess, is ritual pattern base or memes didnt fill right from Vanilla random generation that one time somehow...
            bool flag = true;
            if (specialConflicts != null)
            {
                flag = flag ? specialConflicts?.PreceptConflictSimple(ideo).Accepted ?? true: false;
                flag = flag ? specialConflicts?.ResearchConflicts(ideo).Accepted ?? true: false;
                flag = flag ? specialConflicts?.MemeConflicts(ideo, def).Accepted ?? true: false;
            }            
            if(faction != null)
            {
                flag = flag ? def?.ritualPatternBase?.CanFactionUse(faction) ?? true: false;
            }
            if (!def?.requiredMemes?.NullOrEmpty() ?? false)
            {
                flag = flag ? ideo?.memes?.Any(x => def.requiredMemes.Contains(x)) ?? true: false;
            }
            
            return flag;
        }

    }



}
