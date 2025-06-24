using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace AlphaMemes
{
    public class ThoughtWorker_Precept_Rain : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {

            if (p.Map?.weatherManager?.curWeather.rainRate>0)
            {
                return true;
            }
           
            else return false;






        }
    }
}