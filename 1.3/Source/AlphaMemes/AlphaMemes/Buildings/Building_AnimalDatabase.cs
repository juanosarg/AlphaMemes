using System;
using Verse;

using System.Collections.Generic;

using UnityEngine;

namespace AlphaMemes
{
	public class Building_AnimalDatabase : Building
	{

		public List<PawnKindDef> analyzedAnimalList;

		public override IEnumerable<Gizmo> GetGizmos()
        {
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			yield return new Command_Action
			{
				defaultLabel = "AM_ShowAnimalAnalysis".Translate(),
				action = new Action(this.ShowAnimals),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
			yield break;
		}

		public void ShowAnimals()
        {


			Dialog_AnimalDatabase window = new Dialog_AnimalDatabase(this);
			Find.WindowStack.Add(window);
			

		}

    }
}
