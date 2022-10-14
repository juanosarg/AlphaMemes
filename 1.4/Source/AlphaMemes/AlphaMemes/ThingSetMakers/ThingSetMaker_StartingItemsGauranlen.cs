
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlphaMemes
{

    public class ThingSetMaker_StartingItemsGauranlen : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {

            PawnKindDef dryad = InternalDefOf.AM_UnshackledDryad;

            PawnGenerationRequest request = new PawnGenerationRequest(dryad, Faction.OfPlayer, PawnGenerationContext.NonPlayer);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            Pawn pawn2 = PawnGenerator.GeneratePawn(request);
            Pawn pawn3 = PawnGenerator.GeneratePawn(request);
            outThings.Add(pawn);
            outThings.Add(pawn2);
            outThings.Add(pawn3);


          
        }

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            throw new NotImplementedException();
        }



    }
}

