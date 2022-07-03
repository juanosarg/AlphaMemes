using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{

    public class FuneralPreceptExtension : DefModExtension
    {
       
        public bool isColonistFuneral = false; //Can only have 1 colonist funeral type
        public bool replaceConflictRituals = false; //Decides whether to replace vanilla funeral on generation
        public float _weighting = 0f;//This is for replacing vanilla funeral on RNG generation.

        public bool addNoCorpseFuneral = true;//Adds the base game funeral for no corpse (empty grave)
        public string corpseRitualRoleID = "AM_RitualRoleCorpse"; //add the role here if its not default

        //Animals
        public RitualObiligationTrigger_Animals animalObligationTrigger;
        public bool allowAnimals = false;

        //Special Conflicts
        public FuneralFramework_SpecialConflicts specialConflicts;

        public float Weighting(Ideo ideo, PreceptDef def)
        {
            if(def.associatedMemes != null)
            {
                if (ideo.memes.Any(x => def.associatedMemes.Contains(x)))//5 times more likely if there's associated
                {
                    return _weighting * 5f;
                }

            }
            return _weighting;
        }
        //Simple way to find if we can add it
        public bool CanAddPrecept(Ideo ideo,PreceptDef def,FactionDef faction = null)
        {
            
            bool flag = specialConflicts.PreceptConflictSimple(ideo).Accepted;
            flag = flag ? specialConflicts.ResearchConflicts(ideo).Accepted : false;
            flag = flag ? specialConflicts.MemeConflicts(ideo,def).Accepted : false;
            if(faction != null)
            {
                flag = flag ? def.ritualPatternBase.CanFactionUse(faction) : false;
            }
            if (!def.requiredMemes.NullOrEmpty())
            {
                flag = flag ? ideo.memes.Any(x => def.requiredMemes.Contains(x)) : false;
            }
            
            return flag;
        }
        //Changed mind on this. Make a unique precept def for no corpse
        //This will cause headaches on fluid ideos.
/*        public void SetNoCorpseFuneralDefName(Ideo ideo,PreceptDef precept)
        {
            //Used to make No Corpse Funeral have the same name as main funeral
            Precept_Ritual ritual = ideo.GetAllPreceptsOfType<Precept_Ritual>().First(
                x => x.def.HasModExtension<FuneralPreceptExtension>() ? x.def.GetModExtension<FuneralPreceptExtension>().addNoCorpseFuneral : false);
            if (ritual != null) { 
                
                precept.takeNameFrom = ritual.def;
            }
        }*/

    }



}
