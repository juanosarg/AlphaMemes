using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AlphaMemes
{
    public class RitualBehaviorWorker_Tantrum : RitualBehaviorWorker
    {
        //Users and there thinking up ways to break your memes
        public RitualBehaviorWorker_Tantrum()
        {
        }
        public RitualBehaviorWorker_Tantrum(RitualBehaviorDef def) : base(def)
        {
        }
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            if (!target.Map.IsPlayerHome)
            {
                return "AM_TantrumMustBePlayerHome".Translate();
            }
            return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
        }
    }
}
