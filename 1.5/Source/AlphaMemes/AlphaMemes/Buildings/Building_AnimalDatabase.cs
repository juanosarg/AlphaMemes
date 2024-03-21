using System;
using Verse;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlphaMemes
{
	public class Building_AnimalDatabase : Building
	{

		public List<PawnKindDef> analyzedAnimalList = new List<PawnKindDef>();
		public int totalAnimals = 0;
		public const int tickTotal = 10;
		public int tickCounter = 0;

		public override IEnumerable<Gizmo> GetGizmos()
        {
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			yield return new Command_Action
			{
				defaultLabel = "AM_ShowAnimalAnalysis".Translate(),
				defaultDesc = "AM_ShowAnimalAnalysisDesc".Translate(),
				action = new Action(this.ShowAnimals),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
			yield break;
		}

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {

			List<PawnKindDef> totalAnimalList = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
											where x.RaceProps.Animal && !x.RaceProps.Dryad
											select x).ToList();
			totalAnimals = totalAnimalList.Count;
			base.SpawnSetup(map, respawningAfterLoad);

        }

        public override void ExposeData()
        {
			Scribe_Collections.Look(ref analyzedAnimalList, "analyzedAnimalList", LookMode.Def);
			base.ExposeData();
        }

        public void ShowAnimals()
        {


			Dialog_AnimalDatabase window = new Dialog_AnimalDatabase(this);
			Find.WindowStack.Add(window);
			

		}

        public override void TickRare()
        {
            base.TickRare();
            if (tickCounter>tickTotal && totalAnimals!=0) {
				StaticCollectionsClass.databaseCompletion = (float)this.analyzedAnimalList.Count / (float)this.totalAnimals;
				tickCounter = 0;
			}
			tickCounter++;
			
        }

        public override string GetInspectString()
        {
			string completionDescription = "";
			float completion = 0;

			if (totalAnimals != 0)
			{
				completion = (float)this.analyzedAnimalList.Count / (float)this.totalAnimals;
				
			}

			completionDescription+="AM_CompletionDescription".Translate(completion.ToStringPercent());

			if (completionDescription != "") { return completionDescription; } else return null;
			
        }

    }
}
