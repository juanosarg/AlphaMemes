using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Need this to prevent corpses from being added to spectator after brute forcing them into list
    public class RitualRoleFuneralCorpse_RitualSpectatorFilterNotCorpse : RitualSpectatorFilter
    {
        public override bool Allowed(Pawn p)
        {
            return !p.Dead;
        }
    }
}
