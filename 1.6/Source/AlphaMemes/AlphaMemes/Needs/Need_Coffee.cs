using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    public class Need_Coffee : Need
    {
        public override float CurInstantLevel
        {
            get
            {
                if (this.lastCoffeeUseTick >= Find.TickManager.TicksGame - 10)
                {
                    return Mathf.Clamp01(this.lastCoffeeUsed);
                }
                return 0f;
            }
        }
        public CoffeeNeedCategory CurCategory
        {
            get
            {
                if (this.CurLevel < 0.1f)
                {
                    return CoffeeNeedCategory.Craving;
                }
                if (this.CurLevel < 0.2f)
                {
                    return CoffeeNeedCategory.Desiring;
                }
                if (this.CurLevel < 0.3f)
                {
                    return CoffeeNeedCategory.Wanting;
                }
                if (this.CurLevel < 0.7f)
                {
                    return CoffeeNeedCategory.RecentlyDrank;
                }
                if (this.CurLevel < 0.9f)
                {
                    return CoffeeNeedCategory.Full;
                }
                return CoffeeNeedCategory.CompletelyFull;
            }
        }

        public override bool ShowOnNeedList
        {
            get
            {
                if (Find.IdeoManager.classicMode) return false;
                if (InternalDefOf.AM_CoffeePrimacy is null || this.pawn.Ideo?.HasPrecept(InternalDefOf.AM_CoffeeDrinking_Required) != true
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
                    this.CurLevel -= this.CoffeeFallPerTick * 150f;
                }
            }
        }

        private float CoffeeFallPerTick
        {
            get
            {
                return this.def.fallPerDay / 60000f;
            }
        }

        public Need_Coffee(Pawn pawn) : base(pawn)
        {

        }

        public void CoffeeTaken(float coffee)
        {
            this.lastCoffeeUsed = coffee;
            this.lastCoffeeUseTick = Find.TickManager.TicksGame;
        }

        public float lastCoffeeUsed;

        public int lastCoffeeUseTick;




    }
}
