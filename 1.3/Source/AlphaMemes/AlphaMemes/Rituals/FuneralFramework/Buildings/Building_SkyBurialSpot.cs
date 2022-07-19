using System;
using System.Linq;
using System.Collections.Generic;
using Verse;
using Verse.Sound;
using System.Text;
using RimWorld;
using UnityEngine;



namespace AlphaMemes
{
	public class Building_SkyBurialSpot : Building_CorpseCasket
    {   
        public int tickToEat;
        private static int minEatTick = 13000;
        private static int maxEatTick =24500;
        private static float radius = 24.9f;//Should probably be a comp so i could put this in a def but meh adding comp just for this seems minor
        private float nutrition = 0.2f;
        public static float zOffset = 0.60f;

        public void Notify_CorpseBuried()
        {
            nutrition = (float)Math.Round(Corpse.GetStatValue(StatDefOf.Nutrition, true)/25,2);//Nutrition to take based on roughly 25 munches
            tickToEat = Find.TickManager.TicksGame + 2000; //So it doesnt eat immediately
        }
        public override void Draw()
        {
            base.Draw();
            if (base.HasCorpse)
            {
                base.Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(base.Rotation.AsAngle);
                base.Corpse.DrawAt(base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + new Vector3(0,0,zOffset), false);
            }
        }
        public override void DrawExtraSelectionOverlays()
        {           
            base.DrawExtraSelectionOverlays();
            MeditationUtility.DrawArtificialBuildingOverlay(Position, def, Map, radius); //From FocusStrengthOffset_ArtificialBuildings
        }
        public override void TickRare()
        {
            if (HasCorpse)
            {
                if (tickToEat < Find.TickManager.TicksGame && tickToEat > 0)
                {
                    tickToEat = Find.TickManager.TicksGame + Rand.RangeInclusive(minEatTick, maxEatTick);
                    //corpse BestBodyPartToEat is private so duplicated for my purpose here
                    //I would love to do something where I could make a tempthing bird to come down and munch or something.
                    var part = GetBestBodyPartToEat(Corpse, nutrition);
                    int bloodAmount = 1;
                    if (part == null)//Fall back
                    {
                        part = Corpse.InnerPawn.RaceProps.body.corePart;
                    }
                    if (part == Corpse.InnerPawn.RaceProps.body.corePart)
                    {
                        innerContainer.ClearAndDestroyContents();
                        bloodAmount = 3;
                        tickToEat = 0;
                    }
                    else
                    {
                        var missingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, Corpse.InnerPawn, part);
                        missingPart.lastInjury = HediffDefOf.Bite;
                        missingPart.IsFresh = true;
                        Corpse.InnerPawn.health.AddHediff(missingPart);                       
                    }
                    FilthMaker.TryMakeFilth(this.RandomAdjacentCell8Way(), base.Map, ThingDefOf.Filth_Blood, bloodAmount, FilthSourceFlags.None);
                }
            }
            base.TickRare();
        }
        public BodyPartRecord GetBestBodyPartToEat(Corpse corpse,float nutritionWanted)
        {
            IEnumerable<BodyPartRecord> source = from x in corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
            where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(corpse, x) > 0.001f
            select x;
            if (!source.Any<BodyPartRecord>())
            {
                return null;
            }
            return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(corpse, x) - nutritionWanted));
        }
        public override bool StorageTabVisible
        {
            get
            {
                return false; //Never show it because only let ppl in via funeral
            }
        }
        public override bool CanOpen
        {
            get { return false; }//Dont want it to be opened
        }
        public override bool Accepts(Thing thing)
        {
            if(Find.IdeoManager.GetActiveRitualOn(this) != null)
            {
                return true;
            }
            return false;
        }
        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            return base.TryAcceptThing(thing, allowSpecialEffects);
        }
        public override string GetInspectString()
        {            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (base.HasCorpse)
            {
                if (base.Tile != -1)
                {
                    string str = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(base.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
                    stringBuilder.AppendLine();
                    stringBuilder.Append("DiedOn".Translate(str));
                    float f = 1f - Corpse.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(Corpse.InnerPawn.RaceProps.body.corePart);
                    if (f != 0f)
                    {
                        stringBuilder.AppendInNewLine("CorpsePercentMissing".Translate() + ": " + f.ToStringPercent());
                    }                    
                }
            }
            return stringBuilder.ToString();
         }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickToEat, "tickToEat");
            Scribe_Values.Look(ref nutrition, "nutrition");

        }
    }
}
