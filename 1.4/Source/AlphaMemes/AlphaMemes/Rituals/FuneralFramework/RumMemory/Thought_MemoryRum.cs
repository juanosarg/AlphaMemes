using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_MemoryRum : Thought_Memory
    {
        private string cachedLabel;
        private string cachedDesc;
        private Pawn cachedOtherPawn;
        public Pawn rumPawn; //heck...Why other pawn nulls between creating the thought and consuming no idea. But this exists
        public override string LabelCap
        {
            get
            {
                if(cachedLabel == null || cachedOtherPawn != rumPawn)
                {
                    cachedOtherPawn = rumPawn;
                    cachedLabel = CurStage.label.Formatted(rumPawn.LabelShort).CapitalizeFirst();
                }
                return cachedLabel;
            }
        }
        public override string Description
        {
            get
            {
                if (cachedDesc == null)
                {
                    cachedDesc = CurStage.description.Formatted(pawn.LabelShort, rumPawn.NameFullColored).CapitalizeFirst();
                }
                return cachedDesc;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref rumPawn, "rumPawn", true);
        }

    }
}
