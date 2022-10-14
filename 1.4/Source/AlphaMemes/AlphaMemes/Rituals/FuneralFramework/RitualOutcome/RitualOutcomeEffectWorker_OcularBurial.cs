using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Threading.Tasks;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using RimWorld;

namespace AlphaMemes
{

    public class RitualOutcomeEffectWorker_OcularBurial : RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_OcularBurial()
        {
        }
        public RitualOutcomeEffectWorker_OcularBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }
        private static readonly List<string> ocularCreatures = new List<string>() { "AA_Eyeling", "AA_OcularJelly", "AA_OcularNightling", "AA_EngorgedTentacularAberration", "AA_UnblinkingEye", "AA_RedGoo" };
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            //This is the max combat power
            float combatPower = 10f; 
            switch (outcome.positivityIndex)
            {
                case -2:
                    combatPower *= 0;
                    break;
                case -1:
                    combatPower *= 2; 
                    break;
                case 1:
                    combatPower *= 8;
                    break;
                case 2:
                    combatPower *= 20;
                    break;
            }
            
            if (combatPower > 0)
            {
                Faction faction = Find.FactionManager.OfPlayer;
                List<PawnKindDef> options = new List<PawnKindDef>();
                foreach (var creature in ocularCreatures)
                {
                    var def = PawnKindDef.Named(creature);
                    if (def.combatPower <= combatPower)
                    {
                        options.Add(def);
                    }
                }
                if (!options.NullOrEmpty())
                {
                    List<Pawn> spawnedPawns = new List<Pawn>();
                    while (combatPower >= 10)
                    {
                        PawnKindDef creature = options.Where(x => x.combatPower <= combatPower).RandomElementWithFallback(PawnKindDef.Named("AA_Eyeling"));
                        PawnGenerationRequest request = new PawnGenerationRequest()
                        {
                            KindDef = creature,
                            Faction = faction,
                            AllowedDevelopmentalStages = DevelopmentalStage.Newborn,
                        };

                        Pawn spawnedCreature = PawnGenerator.GeneratePawn(request);
                        GenSpawn.Spawn(spawnedCreature, jobRitual.selectedTarget.Cell, jobRitual.Map);
                        spawnedPawns.Add(spawnedCreature);
                        combatPower -= creature.combatPower;
                    }

                    Messages.Message("AM_OcularBurialCreated".Translate(string.Join(", ", spawnedPawns.Select(x => x.Label)).Named("CREATURES"),corpse.InnerPawn.NameShortColored.Named("CORPSE")), new LookTargets(spawnedPawns), MessageTypeDefOf.PositiveEvent);
                    SoundDefOf.Hive_Spawn.PlayOneShot(SoundInfo.InMap(jobRitual.selectedTarget));
                    
                }
            }
            var slime = ThingDef.Named("AM_Filth_OcularGoo");
            var cells = GenRadial.RadialCellsAround(jobRitual.selectedTarget.Cell, 5f, true);
            var room = jobRitual.selectedTarget.Thing.GetRoom();
            var map = jobRitual.Map;
            foreach (var cell in cells)
            {
                float lengthHorizontal = (cell - jobRitual.Spot).LengthHorizontal;
                if (cell.GetRoom(map) == room && Rand.Chance(1f / lengthHorizontal))
                {
                    FilthMaker.TryMakeFilth(cell, map, slime);
                }
            }
            base.ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);
        }



    }
}
