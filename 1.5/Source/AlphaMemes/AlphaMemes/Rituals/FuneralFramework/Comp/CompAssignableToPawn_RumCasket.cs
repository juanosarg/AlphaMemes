using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;


namespace AlphaMemes
{
    public class CompAssignableToPawn_RumCasket: CompAssignableToPawn_Grave
    {
        public Building_RumVat Vat => parent as Building_RumVat;
        public override IEnumerable<Pawn> AssigningCandidates //Differ from base by not being restricted to colonists
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                var vatSet = Vat.GetStoreSettings();
                var corpses = from Corpse x in parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
                              where !x.IsNotFresh() && x.InnerPawn?.RaceProps?.Humanlike == true
                              && vatSet?.AllowedToAccept(x) == true
                              select x.InnerPawn;
                return corpses;
            }
        }
        //These need to be changed to a new desc because they say colonist when that is no longer the case to do later if I get time
        protected override string GetAssignmentGizmoLabel()
        {
            return base.GetAssignmentGizmoLabel();
        }
        protected override string GetAssignmentGizmoDesc()
        {
            return base.GetAssignmentGizmoDesc();
        }
        protected override bool ShouldShowAssignmentGizmo()
        {
            if (Vat.isFuneral == true)
            {
                return false;
            }
            return base.ShouldShowAssignmentGizmo();
        }
    }
}
