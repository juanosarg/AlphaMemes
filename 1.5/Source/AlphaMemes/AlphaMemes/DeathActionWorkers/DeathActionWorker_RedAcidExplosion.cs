﻿
using RimWorld;
using Verse;
using Verse.AI.Group;
namespace AlphaMemes
{
    public class DeathActionWorker_RedAcidExplosion : DeathActionWorker
    {



        public override void PawnDied(Corpse corpse, Lord previousLord)
        {
            float radius;
            if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
            {
                radius = 1.9f;
            }
            else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
            {
                radius = 2.9f;
            }
            else
            {
                radius = 3.9f;
            }



            GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, InternalDefOf.AM_AcidSpit, corpse.InnerPawn, 10, -1, InternalDefOf.AM_GooPop, null, null, null, InternalDefOf.AM_Filth_RedSlime, 1f, 1, null, false, null, 0f, 1);
        }


    }
}
