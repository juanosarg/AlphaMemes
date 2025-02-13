using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaMemes;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace AlphaMemes
{

    public class CompRandomRaidSpawner : ThingComp
    {
        public CompProperties_RandomRaidSpawner Props => (CompProperties_RandomRaidSpawner)this.props;

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.IsHashIntervalTick(500))
            {
                SpawnHostileRaid();
            }
        }

        public void SpawnHostileRaid()
        {
            Faction faction = Find.FactionManager.RandomRaidableEnemyFaction(allowHidden: false, allowDefeated: false, allowNonHumanlike: false, TechLevel.Industrial);
            bool causeEclipse = false;
            if (faction != null)
            {
                PawnKindDef kindDef = faction.RandomPawnKind();
                IntRange numberOfPawns = new IntRange(2, 5);
                

                List<XenotypeDef> allowedXenotypes = new List<XenotypeDef>
                {
                    XenotypeDefOf.Sanguophage
                };
                if (InternalDefOf.VRE_Bruxa != null)
                {
                    allowedXenotypes.Add(InternalDefOf.VRE_Bruxa);
                    allowedXenotypes.Add(InternalDefOf.VRE_Strigoi);
                    allowedXenotypes.Add(InternalDefOf.VRE_Ekkimian);
                }
                if (DefDatabase<XenotypeDef>.GetNamedSilentFail("AG_Malachai") != null)
                { allowedXenotypes.Add(DefDatabase<XenotypeDef>.GetNamedSilentFail("AG_Malachai")); }

                for(int i = 0; i < numberOfPawns.RandomInRange; i++)
                {
                    PawnGenerationRequest request = new PawnGenerationRequest(mustBeCapableOfViolence: true, colonistRelationChanceFactor: 1f, forceAddFreeWarmLayerIfNeeded: false,
                    allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false,
                    forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, kind: kindDef, faction: faction, context: PawnGenerationContext.NonPlayer, tile: -1,
                    forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, validatorPreGear: null, validatorPostGear: null,
                    minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null,
                    fixedIdeo: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null,
                    forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f);
                    XenotypeDef chosenXenotype = allowedXenotypes.RandomElement();
                    request.ForcedXenotype = chosenXenotype;
                    if(chosenXenotype == DefDatabase<XenotypeDef>.GetNamedSilentFail("AG_Malachai"))
                    {
                        causeEclipse = true;

                    }

                    Pawn pawn = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(pawn, this.parent.Position, this.parent.Map);

                    Lord lord = null;
                    if (pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != pawn))
                    {
                        lord = ((Pawn)GenClosest.ClosestThing_Global(pawn.Position, pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction), 99999f, (Thing p) => p != pawn && ((Pawn)p).GetLord() != null)).GetLord();
                    }
                    if (lord == null || !lord.CanAddPawn(pawn))
                    {
                        lord = LordMaker.MakeNewLord(pawn.Faction, new LordJob_DefendPoint(pawn.Position,30,30), Find.CurrentMap);
                    }
                    if (lord != null && lord.LordJob.CanAutoAddPawns)
                    {
                        lord.AddPawn(pawn);
                    }

                }


            }
            foreach(Pawn pawnFriendly in this.parent.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_SanguophageCampRaided, pawnFriendly.Named(HistoryEventArgsNames.Doer)), true);

            }
            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_SanguophageCampRaided_DevPoints), true);
            if (causeEclipse)
            {
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.Eclipse.category, this.parent.Map);
                IncidentDefOf.Eclipse.Worker.TryExecute(parms);
            }

            this.parent.Destroy();
        }

    }
}
