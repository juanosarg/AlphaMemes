using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace AlphaMemes

{
    [StaticConstructorOnStartup]
    public class Pawn_Detonator : Pawn
    {

        public static readonly Texture2D Detonate = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true);


        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            yield return new Command_Action
            {

                defaultLabel = "AM_Detonate".Translate(),
                defaultDesc = "AM_DetonateDesc".Translate(),
                icon = Detonate,
                action = delegate ()
                {

                    this.health.AddHediff(InternalDefOf.AM_Kamikaze);
                    HealthUtility.AdjustSeverity(this, InternalDefOf.AM_Kamikaze, 1);
                }
            };
            yield break;
        }

    }
}