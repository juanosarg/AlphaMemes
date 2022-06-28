using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_FuneralFramework : RitualOutcomeComp_Quality
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return null;
        }
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return 0f;
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        //spawning properties of the outcome

        public virtual Pawn BestPawnForRole(List<Pawn> pawns, RitualRoleAssignments assignments, out string roleId)
        {
            roleId = null;
            return null;
        }
        




    }
}
