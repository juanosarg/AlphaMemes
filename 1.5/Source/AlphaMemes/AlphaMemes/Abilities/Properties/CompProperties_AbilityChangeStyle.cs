using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace AlphaMemes
{
	public class CompProperties_AbilityChangeStyle : CompProperties_AbilityEffect
	{

		public bool affectArea = false;
		public bool affectAllMap = false;
		public bool randomStyle = false;

        public float area;

		public CompProperties_AbilityChangeStyle()
		{
			this.compClass = typeof(CompAbilityChangeStyle);
		}


	}
}
