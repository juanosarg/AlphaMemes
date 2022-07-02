using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Not Actually a real obligation trigger gets called via harmony patch
     //moved logic here so it can be called via mod extensions allowing flexibility
    public class RitualObiligationTrigger_Animals 
    {
        public bool bondedAnimals = true;//i'd love to take these options out of the def and make it deterministic on things like memes/precepts future idea
        public bool veneratedAnimals = true;
        public bool namedanimals = false;
        public bool allAnimals = false;//Probably a bad idea to turn this on but its an option
        public FuneralFramework_SpecialConflicts animalConflicts;//This is so you can set conflicts on just the animal side. without preventing the main funeral

        public virtual bool CanAddPawn(Pawn pawn, Ideo ideo, Precept_Ritual ritual, out Pawn target2)
        {
            //check if conflicting precept/meme
            target2 = null;
            if(animalConflicts != null)
            {
                List<PreceptDef> conflicts = new List<PreceptDef>();//Has no use here
                AcceptanceReport report = animalConflicts?.MemeConflicts(ideo) ?? true;
                if (report.Accepted)
                {
                    report = animalConflicts?.PreceptConflictSimple(ideo) ?? true;
                    if (!report.Accepted)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            
            bool flag = false;
            bool bonded = TrainableUtility.GetAllColonistBondsFor(pawn).Any(x => x.Ideo == ideo);
            bool venerated = ideo.VeneratedAnimals.Any(x => x == pawn.def);
            bool named = !pawn.Name.Numerical;
            if (bonded && bondedAnimals)
            {
                target2 = TrainableUtility.GetAllColonistBondsFor(pawn).FirstOrDefault(x => x.Ideo == ideo);
                flag = true;
            }
            if ((venerated && veneratedAnimals) || (named && namedanimals) || allAnimals)
            {
                flag = true;
            }
            return flag;

        }
        



    }


}
