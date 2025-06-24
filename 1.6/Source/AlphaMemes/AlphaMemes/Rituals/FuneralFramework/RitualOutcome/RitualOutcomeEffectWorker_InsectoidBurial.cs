using System;
using System.Collections;
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

    public class RitualOutcomeEffectWorker_InsectoidBurial : RitualOutcomeEffectWorker_FuneralFramework
    {
        public RitualOutcomeEffectWorker_InsectoidBurial()
        {
        }
        public RitualOutcomeEffectWorker_InsectoidBurial(RitualOutcomeEffectDef def) : base(def)
        {
        }
        
        public override void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome)
        {
            //This is the max combat power
            float combatPower = 40f; //Base is megascarab of 40
            switch (outcome.positivityIndex)
            {
                case -2:
                    combatPower *= 0;
                    break;
                case -1:
                    combatPower *= 2; //92% scarab 8% speloped
                    break;
                case 1:
                    combatPower *= 4;//25% scarab 69% spelopede 6% mega spider
                    break;
                case 2:
                    combatPower *= 10;//13% scarab 24% spelopede 64% MegaSpider
                    break;
            }
            if (combatPower > 0)
            {
                //I dont want it auto tamed, but if the faction isnt the player (like random cave hive I want it to be that faction)
                Thing ritualThing = jobRitual.selectedTarget.Thing;
                Faction faction = ritualThing.Faction;
                if(faction != null)
                {
                    if (faction.IsPlayer)
                    {
                        faction = null;
                    }
                }
                List<PawnKindDef> insects = null;
                //If the ritual thing has a spawnablePawnKinds use that
                //Access tools to give better compatability then is Hive (Or at least Alpha Animals/VFEI-1 has the exact same field so it adds compat for that. No idea about other mod hives)
                //They would also need a patch operation anyway to make work with ritual so \o/
                FieldInfo info = AccessTools.Field(ritualThing.GetType(), "spawnablePawnKinds");
                if (info != null && !info.IsStatic)
                {
                    //This is because some hives don't expose their data
                    MethodInfo method = AccessTools.Method(ritualThing.GetType(), "ResetStaticData");
                    if (method != null && !method.IsStatic && method.GetParameters().Length == 0)
                    {
                        method.Invoke(ritualThing, Array.Empty<object>());
                    }

                    if (info.GetValue(ritualThing) is List<PawnKindDef> list)
                    {
                        insects = list.Where(x => x.combatPower <= combatPower).ToList();
                    }
                }
                // Handle vanilla CompSpawnerPawn and possible subtypes
                CompSpawnerPawn comp = ritualThing.TryGetComp<CompSpawnerPawn>();
                if(insects.NullOrEmpty() && comp?.props is CompProperties_SpawnerPawn props)
                {
                    // VFE-I2 compat. The mod stores the pawns in an insectTypes field, but uses
                    // its own type to store PawnKindDef and the type of the insectoid. This
                    // getter will only get all the defs without the types.
                    var method = AccessTools.PropertyGetter(comp.GetType(), "AllAvailableInsects");
                    if (method != null && !method.IsStatic && method.Invoke(comp, Array.Empty<object>()) is List<PawnKindDef> list)
                    {
                        insects = list.Where(x => x.combatPower <= combatPower).ToList();
                    }
                    // Handle vanilla hives with VFE-I2, as it has a custom insect hive
                    // comp (CompInsectSpawner) that it replaces the vanilla comp with.
                    var field = AccessTools.Field(props.GetType(), "geneline");
                    if (field != null && !field.IsStatic && field.GetValue(props) is Def genelineDef)
                    {
                        field = AccessTools.Field(genelineDef.GetType(), "insects");
                        if (field != null && !field.IsStatic && field.GetValue(genelineDef) is IList possibleInsects && possibleInsects.Count > 0)
                        {
                            field = AccessTools.Field(possibleInsects[0].GetType(), "kind");
                            if (field != null && !field.IsStatic && field.FieldType == typeof(PawnKindDef))
                            {
                                insects = new List<PawnKindDef>();
                                foreach (var insect in possibleInsects)
                                {
                                    // Prevent queens, as the mod does not allow the hives to spawn them
                                    if (field.GetValue(insect) is PawnKindDef def && def.combatPower <= combatPower && def.defName != "VFEI2_Queen")
                                    {
                                        insects.Add(def);
                                    }
                                }
                            }
                        }
                    }

                    // Handle the vanilla hives/spawners that aren't using VFE-I2 comp.
                    // In case it's VFE-I2 hive, but has no specified pawns - attempt to use vanilla field.
                    if (insects.NullOrEmpty())
                    {
                        insects = props.spawnablePawnKinds.Where(x => x.combatPower <= combatPower).ToList();
                    }
                }
                // A fallback if the thing does not have supported PawnKindDef list
                if (insects.NullOrEmpty())
                {
                    //Has genders is to filter out VFE Insect vat grown. LifeStages is so its only things that have a larve form
                    insects = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.RaceProps.Insect && x.RaceProps.hasGenders
                    && x.combatPower <= combatPower && x.lifeStages.Count > 1 && x.lifeStages.All(y => y.bodyGraphicData != null)).ToList();
                }
                if (!insects.NullOrEmpty())
                {
                    PawnKindDef insectKind = insects.RandomElementByWeight(x => GetWeight(x, x.combatPower / combatPower));
                    PawnGenerationRequest request = new PawnGenerationRequest(insectKind)
                    {
                        Faction = faction,
                        AllowedDevelopmentalStages = DevelopmentalStage.Newborn,
                        Context = PawnGenerationContext.NonPlayer,
                    };
                    Pawn insect = PawnGenerator.GeneratePawn(request);
                    GenSpawn.Spawn(insect, jobRitual.selectedTarget.Cell, jobRitual.Map);
                    Messages.Message("AM_InsectBurialInsectBorn".Translate(insect.Label.Named("INSECT"),corpse.InnerPawn.NameShortColored.Named("CORPSE")), new LookTargets(insect), MessageTypeDefOf.PositiveEvent);
                    InternalDefOf.Hive_Spawn.PlayOneShot(SoundInfo.InMap(jobRitual.selectedTarget));
                    FilthMaker.TryMakeFilth(jobRitual.selectedTarget.Cell, jobRitual.Map, ThingDefOf.Filth_Slime,6);
                }
            }

            base.ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);
        }
        //Weight explination, pawn kind combat power / max combat power compared against a curve thats makes things that are half the combat power most common. then both low and high combat power things rarer
        //with just this mod its a bit excessive, but expecting this would pretty much always be used with VFE Insectoids, alpha animals, and probably many other mods that add insects I dont know.
        private float GetWeight(PawnKindDef x,float curvePoint)
        {
            return combatPowerCurve.Evaluate(curvePoint);
        }
        //Just used to confirm things are working how I expect
        private void Debug()
        {
            for (int i = 1; i < 20; i++)
            {
                float combatPower = 40f * i;
                float maxWeight = 0f;
                Log.Message("Iteration start CombatPower:" + combatPower.ToString());
                List<PawnKindDef> insects = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.RaceProps.Insect && x.RaceProps.hasGenders
                && x.combatPower <= combatPower && x.lifeStages.Count > 1 && x.lifeStages.All(y => y.bodyGraphicData != null)).ToList();
                foreach (PawnKindDef insect in insects)
                {
                    Log.Message(insect.label);
                    float curvePoint = insect.combatPower / combatPower;
                    Log.Message("Curve Point " + curvePoint.ToString());
                    float weight = GetWeight(insect, curvePoint);
                    maxWeight += weight;
                    Log.Message("Weight " + weight.ToString());
                }                
                Log.Message("Iteration End Max Weight " + maxWeight.ToString());
                foreach (PawnKindDef insect in insects)
                {
                    float curvePoint = insect.combatPower / combatPower;
                    float weight = GetWeight(insect, curvePoint);
                    float chance = weight / maxWeight;
                    Log.Message(insect.label + " " + chance.ToStringPercent());
                }
            }
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
        }


        public override void ExposeData()
        {
            base.ExposeData();
        }

        private static readonly SimpleCurve combatPowerCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0f),
                true
            },
            {
                new CurvePoint(0.25f, 0.33f),
                true
            },
            {
                new CurvePoint(0.5f, 1f),
                true
            },
            {
                new CurvePoint(0.75f, 0.33f),
                true
            },
            {
                new CurvePoint(1.01f, 0f),
                true
            }
        };


    }
}
