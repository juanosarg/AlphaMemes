using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using RimWorld;

namespace AlphaMemes
{

    public class RitualOutcomeEffectWorker_Fleshcrafting : RitualOutcomeEffectWorker_FuneralFramework
    {

        public List<string> list = new List<string>() {"VME_FleshcraftedArm","VME_FleshcraftedHand","VME_FleshcraftedLeg","VME_FleshcraftedHeart","VME_FleshcraftedLung","VME_FleshcraftedKidney","VME_FleshcraftedLiver",
            "VME_FleshcraftedEye","VME_FleshcraftedEar","VME_FleshcraftedNose","VME_FleshcraftedJaw","VME_FleshcraftedStomach","VME_FleshcraftedTongue","VME_FleshcraftedSpine","VME_FleshcraftedFinger",
            "VME_FleshcraftedToe","VME_FleshcraftedFoot"};

        public RitualOutcomeEffectWorker_Fleshcrafting()
        {
        }
        public RitualOutcomeEffectWorker_Fleshcrafting(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            for (int i = 0; i < 20; i++)
            {
                IntVec3 c;
                CellFinder.TryFindRandomReachableCellNear(jobRitual.selectedTarget.Cell, jobRitual.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                FilthMaker.TryMakeFilth(c, jobRitual.Map, ThingDefOf.Filth_Blood);

            }
            SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(jobRitual.selectedTarget.Cell, jobRitual.Map, false));

            ThingDef newThing;
            string newThingName;
            ThingDef secondaryOrgan;
            string secondaryOrganName;

            ThingDef tertiaryOrgan;

            switch (outcome.positivityIndex) { 

               
                case -1:
                    newThing = ThingDef.Named(list.RandomElement());                   
                    GenSpawn.Spawn(newThing, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                   
                    break;
                case 1:
                    newThingName = list.RandomElement();
                    newThing = ThingDef.Named(newThingName);
                    GenSpawn.Spawn(newThing, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                    secondaryOrganName = (from x in list where x != newThingName select x).RandomElement();
                    secondaryOrgan = ThingDef.Named(secondaryOrganName);
                    GenSpawn.Spawn(secondaryOrgan, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                    break;
                case 2:
                    newThingName = list.RandomElement();
                    newThing = ThingDef.Named(newThingName);
                    GenSpawn.Spawn(newThing, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                    secondaryOrganName = (from x in list where x != newThingName select x).RandomElement();
                    secondaryOrgan = ThingDef.Named(secondaryOrganName);
                    GenSpawn.Spawn(secondaryOrgan, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                    tertiaryOrgan = ThingDef.Named((from x in list where x != newThingName && x != secondaryOrganName select x).RandomElement());                   
                    GenSpawn.Spawn(tertiaryOrgan, jobRitual.selectedTarget.Cell, jobRitual.Map, WipeMode.Vanish);
                    break;

            }

            



        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            base.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out extraOutcomeDesc, ref letterLookTargets);
        }


        public override void ExposeData()
        {
            base.ExposeData();
        }




    }
}
