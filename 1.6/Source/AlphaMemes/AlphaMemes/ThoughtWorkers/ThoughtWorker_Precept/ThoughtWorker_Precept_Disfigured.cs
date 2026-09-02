
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Disfigured : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
                
            if (!IsDisfigured(p))
            {
                return ThoughtState.ActiveAtStage(1);
            }
            return ThoughtState.ActiveAtStage(0);
        }

        public static bool IsDisfigured(Pawn pawn)
        {
          
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i].Part == null || !hediffs[i].Part.def.beautyRelated)
                {
                    continue;
                }
                Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
                if (hediff_MissingPart != null)
                {
                    if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part))
                    {
                        continue;
                    }
                }
                else if (!(hediffs[i] is Hediff_Injury))
                {
                    continue;
                }
                return true;
            }
            return false;
        }
    }
}