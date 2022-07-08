using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using System.Linq;


namespace AlphaMemes
{   //A way to add restrictions to can add rituals beyond just required meme
    public class FuneralFramework_SpecialConflicts
    {
        public List<PreceptDef> conflictingPreceptDefs = new List<PreceptDef>();
        public List<MemeDef> conflictingMemes = new List<MemeDef>();//only really used for animals, use preceptdef conflicting memes for entire ritual to be blocked
        public List<ResearchProjectDef> researchNeeded = new List<ResearchProjectDef>();//This only makes sense/is used if they are fluid

        public AcceptanceReport PreceptConflicts(Ideo ideo, out List<PreceptDef> ritualConflicts, FuneralPreceptExtension preceptExtension = null)
        {
           
            List<PreceptDef> preceptConflicts = new List<PreceptDef>();
            ritualConflicts = new List<PreceptDef>();
            foreach (Precept p in ideo.PreceptsListForReading.Where(x => x.def.HasModExtension<FuneralPreceptExtension>()))
            {
                if (conflictingPreceptDefs?.Contains<PreceptDef>(p.def)?? false)//No longer need to prevent 2 funerals
                {
                    if (p.def.issue.defName == "Ritual")
                    {
                        ritualConflicts.Add(p.def);
                    }
                    preceptConflicts.Add(p.def);
                }
            }
            if (preceptExtension.replaceConflictRituals && ritualConflicts.Count>0 && ritualConflicts.Count == preceptConflicts.Count)
            {

                return true;
             }
            if (preceptConflicts.Count > 0)
            {
                return "Funeral_ConflictingPrecepts".Translate(string.Join(", ", preceptConflicts.Select(x=> x.LabelCap)).Named("CONFLICTS"));
            }

            return true;
        }
        public AcceptanceReport PreceptConflictSimple(Ideo ideo)
        {
            if(ideo.PreceptsListForReading.Any(x=> conflictingPreceptDefs?.Contains(x.def)??false))
            {
                return false;
            }

            return true;
        }
        public bool AddingConflict(Ideo ideo, Precept addingPrecept)
        {

            if (conflictingPreceptDefs?.Contains(addingPrecept.def)?? false)
            {
                return true;
            }

            return false;
        }
        public AcceptanceReport MemeConflicts(Ideo ideo, PreceptDef def = null)
        {
            if(conflictingMemes?.Any(x => ideo.memes.Contains(x))?? false)
            {
                return "Funeral_ConflictingMemes".Translate(string.Join(", ", conflictingMemes.Where(x => ideo.memes.Contains(x)).Select(x => x.LabelCap)).Named("CONFLICTS"));
            }
            if(def != null) //Adding this so that I can easily check everything with the same methods
            {
                return !def.conflictingMemes?.Any(x => ideo.memes.Contains(x)) ?? true;
            }
            return true;
        }
        public AcceptanceReport ResearchConflicts(Ideo ideo)
        {
            if (!ideo.Fluid)
            {
                return true;
            }
            if (researchNeeded.Any(x => !x.IsFinished))
            {                
                return "Funeral_MissingResearch".Translate(string.Join(", ", researchNeeded.Where(x => !x.IsFinished).Select(x => x.LabelCap)).Named("RESARCH"));
            }

            return true;
        }
    }
}
