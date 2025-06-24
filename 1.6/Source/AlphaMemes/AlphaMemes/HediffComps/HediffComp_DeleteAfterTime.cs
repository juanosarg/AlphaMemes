using System.Collections.Generic;
using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;




namespace AlphaMemes
{
    public class HediffComp_DeleteAfterTime : HediffComp
    {

        public HediffCompProperties_DeleteAfterTime Props => (HediffCompProperties_DeleteAfterTime)props;

        public int timer;

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref timer, "timer", 0);

        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            timer++;
            if (timer > Props.disappearsAfterTicks)
            {
                this.parent.pawn.health.RemoveHediff(parent);
                

            }


        }

      

        public override string CompLabelInBracketsExtra
        {
            get
            {

                return (Props.disappearsAfterTicks - timer).ToStringTicksToPeriod(allowSeconds: true, shortForm: true, canUseDecimals: true, allowYears: true, true);
            }
        }


    }
}
