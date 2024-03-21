using System;
using RimWorld;



namespace AlphaMemes
{
	public class Precept_DespisedAnimal : Precept_ThingDef
	{
		public override string UIInfoFirstLine
		{
			get
			{
				if (base.ThingDef == null)
				{
					return base.UIInfoFirstLine;
				}
				return base.ThingDef.LabelCap;
			}
		}

		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.ThingDef.LabelCap;
			}
		}
	}
}
