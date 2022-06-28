using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using RimWorld.Planet;
using Verse;
using RimWorld;

namespace AlphaMemes
{
    
    public class RitualOutcomeEffectWorker_FuneralBlastOff: RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_FuneralBlastOff() 
        { 
        }
        public RitualOutcomeEffectWorker_FuneralBlastOff(RitualOutcomeEffectDef def) : base(def)
        {
        }
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            
            Thing transporter = jobRitual.selectedTarget.Thing;
            CompLaunchable compLaunchable = transporter.TryGetComp<CompLaunchable>();
            CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
            IntVec3 cell = transporter.Position;

            float fuel = Mathf.Max(CompLaunchable.FuelNeededToLaunchAtDist(CompLaunchable.MaxLaunchDistanceAtFuelLevel(compLaunchable.FuelingPortSourceFuel)), 1f);
            compTransporter.Launchable.FuelingPortSource?.TryGetComp<CompRefuelable>().ConsumeFuel(fuel);

            ActiveDropPod innerthing = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
            innerthing.Contents = new ActiveDropPodInfo();
            innerthing.Contents.innerContainer.TryAddRangeOrTransfer(compTransporter.GetDirectlyHeldThings());

            //Getting hacky here cause lordritual is not a fan of corpse actually flying off
            FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(compLaunchable.Props.skyfallerLeaving, innerthing);
            flyShipLeaving.createWorldObject = false;
            transporter.Destroy();
            GenSpawn.Spawn(flyShipLeaving, cell, jobRitual.Map, WipeMode.Vanish);



        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            List<Settlement> settlements = NearbyTradeableSettlements(jobRitual.selectedTarget.Thing);
            if(settlements.Count > 0)
            {
                if (OutcomeChanceWorst(jobRitual, outcome) && outcomeExtension.worstOutcomeDesc != null)
                {
                    Settlement settlement = settlements.RandomElementWithFallback(null);
                    extraOutcomeDesc = outcomeExtension.worstOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"), settlement.Name.Named("SETTLEMENT"));
                    //Harm Relations
                    Faction.OfPlayer.TryAffectGoodwillWith(settlement.Faction, -15, true, true);

                }
                if (outcome.BestPositiveOutcome(jobRitual))
                {
                    extraOutcomeDesc = outcomeExtension.bestOutcomeDesc.Formatted(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"));
                    IncidentParms parms = new IncidentParms();
                    parms.target = jobRitual.Map;
                    parms.traderKind = DefDatabase<TraderKindDef>.AllDefs.Where(x => x.orbital).RandomElement();
                    IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(parms);
                }
            }
           
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
        }
        public List<Settlement> NearbyTradeableSettlements(Thing pod)
        {
            
            CompLaunchable compLaunchable = pod.TryGetComp<CompLaunchable>();
            List<Settlement> settlements = new List<Settlement>();
            int maxDistance = CompLaunchable.MaxLaunchDistanceAtFuelLevel(compLaunchable.FuelingPortSourceFuel);
            foreach (Settlement settlement in Find.WorldObjects.SettlementBases.Where(x=> Find.WorldGrid.ApproxDistanceInTiles(x.Tile,pod.Map.Tile) <= maxDistance))
            {
                if (settlement.Visitable)
                {
                    settlements.Add(settlement);
                }
            }

            return settlements;
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }


        
        
    }
}
