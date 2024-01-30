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
                Thought_MemoryRum thought = (Thought_MemoryRum)ThoughtMaker.MakeThought(InternalDefOf.AM_CorpseRumThought);
                var sources = ingested.TryGetComp<CompHasPawnSources>();
                if(!sources.pawnSources.NullOrEmpty())
                {    
                    Pawn otherPawn = sources.pawnSources[0];
                    thought.otherPawn = otherPawn;
                    thought.rumPawn = otherPawn; //I don't want to know why this is needed, but it must be something really dumb
                }
                pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
            }
        }
    }
}
