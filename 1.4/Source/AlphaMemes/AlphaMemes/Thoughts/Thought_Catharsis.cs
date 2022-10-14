using System;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Thought_Catharsis : Thought_Memory
    {

        public bool appliedOnce = false;




        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.appliedOnce, "appliedOnce", false, false);
        }


        public override int CurStageIndex
        {
            get
            {
                if (this.pawn?.Ideo?.HasMeme(InternalDefOf.AM_Madness) == true) //Pawn could be null depending on how the memory is created. TryGainMemory assigned the this.Pawn. So VTE was causing a call to it before pawn could be assigned
                {
                    return 1;
                }
                else
                    return 0;
            }
        }

        public override float MoodOffset()
        {
            if (!appliedOnce)
            {

                if (this.pawn?.Ideo?.HasMeme(InternalDefOf.AM_Madness) == true)
                {
                    this.pawn.health.AddHediff(InternalDefOf.AM_CatharsisHediff);
                    pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.AM_CatharsisHediff, false).Severity += 1;
                }

                appliedOnce = true;
            }


            return base.MoodOffset();
        }




    }
}

