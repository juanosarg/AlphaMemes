using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class IngestestionOutcomeDoer_AddThoughtRum : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if(pawn.needs?.mood != null)
            {
                Thought_Memory thought = (Thought_Memory)ThoughtMaker.MakeThought(InternalDefOf.AM_CorpseRumThought);
                pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
            }
        }
    }
}
