using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Need_Tea : Need
    {
        public override float CurInstantLevel
        {
            get
            {
                if (this.lastTeaUseTick >= Find.TickManager.TicksGame - 10)
                {
                    return Mathf.Clamp01(this.lastTeaUsed);
                }
                return 0f;
            }
        }
        public TeaNeedCategory CurCategory
        {
            get
            {
                if (this.CurLevel < 0.1f)
                {
                    return TeaNeedCategory.Craving;
                }
                if (this.CurLevel < 0.2f)
                {
                    return TeaNeedCategory.Desiring;
                }
                if (this.CurLevel < 0.3f)
                {
                    return TeaNeedCategory.Wanting;
                }
                if (this.CurLevel < 0.7f)
                {
                    return TeaNeedCategory.RecentlyDrank;
                }
                if (this.CurLevel < 0.9f)
                {
                    return TeaNeedCategory.Full;
                }
                return TeaNeedCategory.CompletelyFull;
            }
        }

        public override bool ShowOnNeedList
        {
            get
            {
                if (Find.IdeoManager.classicMode) return false;
                if (InternalDefOf.AM_TeaPrimacy is null || this.pawn.Ideo?.HasPrecept(InternalDefOf.AM_TeaDrinking_Required) != true
                    || ExpectationsUtility.CurrentExpectationFor(this.pawn).order <= 2)
                {
                    return false;
                }
                else return true;


            }
        }



        public override void NeedInterval()
        {
            if (!this.IsFrozen)
            {
                if (ExpectationsUtility.CurrentExpectationFor(this.pawn).order > 2)
                {

                    this.CurLevel -= this.TeaFallPerTick * 150f;
                }

            }
        }

        private float TeaFallPerTick
        {
            get
            {
                return this.def.fallPerDay / 60000f;
            }
        }

        public Need_Tea(Pawn pawn) : base(pawn)
        {

        }

        public void TeaTaken(float tea)
        {
            this.lastTeaUsed = tea;
            this.lastTeaUseTick = Find.TickManager.TicksGame;
        }

        public float lastTeaUsed;

        public int lastTeaUseTick;




    }
}
