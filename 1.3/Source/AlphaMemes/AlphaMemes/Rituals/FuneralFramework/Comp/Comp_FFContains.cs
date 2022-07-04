using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class Comp_CorpseContainer : ThingComp
    {
        //I just want my urn to say who it contains T_T
        //If there was another way I couldnt find it and Im sad cause this seems like way to much effort for something so small
        private TaggedString pawnName = null;
        private TaggedString pawnNickname = null;
        private Pawn innerPawn = null;
        private string deathDate;
        private TaleReference taleRef;
        public CompProperties_CorpseContainer Props
        {
            get
            {
                return (CompProperties_CorpseContainer)props;
            }
        }
        public bool Active //This is probably a good idea, who knows what wonky things can happen addings things
        {
            get { return taleRef != null; }
        }
        public override string CompInspectStringExtra()
        {
            if (!Active)
            {
                return base.CompInspectStringExtra();
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Props.inspectString.Formatted(pawnName.Named("CORPSE")).Resolve());
            stringBuilder.AppendLine();
            stringBuilder.Append("DiedOn".Translate(deathDate));
            return stringBuilder.ToString();
        }
        public override string TransformLabel(string label)
        {
            if (!Active)
            {
                return base.TransformLabel(label);
            }
            string returnString = Props.transformLabel.Formatted(pawnNickname.CapitalizeFirst().Named("CORPSE"), label.Named("LABEL")).Resolve();
            return base.TransformLabel(returnString);
        }
        public void InitComp_CorpseContainer(Corpse corpse)
        {
            
            pawnName = corpse.InnerPawn.NameFullColored;
            pawnNickname = corpse.InnerPawn.NameShortColored;
            innerPawn = corpse.InnerPawn;
            taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(corpse.InnerPawn);
            deathDate = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(corpse.timeOfDeath), Find.WorldGrid.LongLatOf(corpse.Tile));
        }
        public override string GetDescriptionPart()
        {
            if (!Active)
            {
                return base.GetDescriptionPart();
            }
            return "CasketContains".Translate() + ": " + pawnName.CapitalizeFirst() + "\n\n" + GenerateImageDescription();
        }
        public TaggedString GenerateImageDescription()
        {
            return taleRef.GenerateText(TextGenerationPurpose.ArtDescription, Props.descriptionMaker);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<TaggedString>(ref pawnName, "pawnName", null, false);
            Scribe_Values.Look<TaggedString>(ref pawnNickname, "pawnNickname", null, false);
            Scribe_Values.Look<string>(ref deathDate, "deathDate", null, false);
            Scribe_Deep.Look<TaleReference>(ref taleRef, "taleRef", Array.Empty<object>());
            Scribe_References.Look(ref innerPawn, "innerPawn", true);
        }
    }
}
