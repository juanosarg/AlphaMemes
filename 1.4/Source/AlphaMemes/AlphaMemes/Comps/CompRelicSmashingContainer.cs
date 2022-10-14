
using RimWorld;
using Verse;
namespace AlphaMemes
{
	public class CompRelicSmashingContainer : CompThingContainer
	{
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Reliqary"))
			{
				parent.Destroy();
			}
			else
			{
				base.PostSpawnSetup(respawningAfterLoad);
			}
		}

		public static bool IsRelic(Thing thing)
		{
			return thing.IsRelic();
		}

		public override bool Accepts(Thing thing)
		{
			return IsRelic(thing);
		}

		public override bool Accepts(ThingDef thingDef)
		{
			return false;
		}

		
	}
}
