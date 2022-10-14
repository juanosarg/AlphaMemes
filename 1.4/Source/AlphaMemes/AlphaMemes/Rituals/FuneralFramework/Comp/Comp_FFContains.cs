using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace AlphaMemes
{ 
    public class Comp_CorpseContainer : ThingComp
    {
        //I just want my urn to say who it contains T_T
        //If there was another way I couldnt find it and Im sad cause this seems like way to much effort for something so small
        private TaggedString pawnName = null;
        private TaggedString pawnNickname = null;
        public Pawn innerPawn = null;
        public Corpse innerCorpse = null;        
        private string deathDate;
        private TaleReference taleRef;
        private CompProperties_CorpseContainer Props
        {
            get
            {
                return (CompProperties_CorpseContainer)props;
            }
        }
        public virtual Pawn VisitedPawn
        {
            get{ return innerPawn; }
        }
        public virtual bool Active //This is probably a good idea, who knows what wonky things can happen addings things
        {
            get { return innerPawn != null; }
        }
        public virtual int OpenTicks
        {
            get { return Props.ticksToOpen; }
        }
        public virtual bool CanOpen
        {
            get { return innerCorpse != null && !innerCorpse.Destroyed; }
        }
        public override string CompInspectStringExtra()
        {
            if (!Active || Props.inspectString == null)
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
            if (!Active || Props.transformLabel == null)
            {
                return base.TransformLabel(label);
            }
            QualityCategory quality;
            string returnString = label;
            if (parent.TryGetQuality(out quality))
            {
                returnString = Props.transformLabel.Formatted(pawnNickname.CapitalizeFirst().Named("CORPSE"), label.Named("LABEL"),quality.GetLabel().CapitalizeFirst().Named("QUALITY")).Resolve();
            }
            else
            {
                returnString = Props.transformLabel.Formatted(pawnNickname.CapitalizeFirst().Named("CORPSE"), label.Named("LABEL")).Resolve();
            }
            
            return base.TransformLabel(returnString);
        }
        public virtual void InitComp_CorpseContainer(Corpse corpse)
        {
            
            pawnName = corpse.InnerPawn.NameFullColored;
            pawnNickname = corpse.InnerPawn.NameShortColored;
            innerPawn = corpse.InnerPawn;
            innerCorpse = corpse;
            taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(corpse.InnerPawn);
            deathDate = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(corpse.timeOfDeath), Find.WorldGrid.LongLatOf(corpse.Tile));
        }
        public virtual void RemoveCorpse(Corpse corpse)
        {
            if (!GenDrop.TryDropSpawn(corpse, parent.InteractionCell, parent.Map, ThingPlaceMode.Near, out var thing))
            {
                Messages.Message("AM_CorpseContainerFailedToEject".Translate(), MessageTypeDefOf.NegativeEvent);
            }
            innerCorpse = null;
            innerPawn=null;
            if (Props.destroyOnOpen) parent.Destroy(DestroyMode.KillFinalize);

        }
        //Rather then calculating rot at time of taking it out. I'm just going to tick rare it. Still less impact then ticking the corpse itself but performance impact of either are probably nothing
        //But rottable is only thing that needs to tick
        public override void CompTickRare()
        {
            if (innerCorpse != null && !innerCorpse.Destroyed)
            {
                var compRot = innerCorpse.GetComp<CompRottable>();
                compRot.CompTickRare();
            }
        }
        public override string GetDescriptionPart()
        {
            if (!Active)
            {
                return base.GetDescriptionPart();
            }
            if(parent.GetComp<CompArt>() != null) //So we dont double up
            {
                return "CasketContains".Translate() + ": " + pawnName.CapitalizeFirst();
            }
            return "CasketContains".Translate() + ": " + pawnName.CapitalizeFirst() + "\n\n" + GenerateImageDescription();
        }
        public TaggedString GenerateImageDescription()
        {
            return taleRef.GenerateText(TextGenerationPurpose.ArtDescription, Props.descriptionMaker);
        }
        
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            //I'd like to figure out how to make this so a pawn goes and does it. But that needs a designator, a job and more complicated things
            //I think I know how to do it. Add my own designationDef that only gets used via this. Create a workgiver, create a jobdriver
            //Because I dont want to add that complication right now. I'm adding just this version with devmode.
            if (CanOpen && Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DesignatorOpen".Translate(),
                    defaultDesc = "AM_CorpseContainerOpen".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true),
                    action = ()=>
                    {
                        RemoveCorpse(innerCorpse);
                    }
                };
            }
        }
        
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<TaggedString>(ref pawnName, "pawnName", null, false);
            Scribe_Values.Look<TaggedString>(ref pawnNickname, "pawnNickname", null, false);
            Scribe_Values.Look<string>(ref deathDate, "deathDate", null, false);
            Scribe_Deep.Look<TaleReference>(ref taleRef, "taleRef", Array.Empty<object>());
            if (innerCorpse != null && innerCorpse.Destroyed)//I dont understand why it complains when I tell it to not save destroyed things but it does so doing this.
            {
                innerCorpse = null;
            }
            Scribe_Deep.Look(ref innerCorpse,false, "innerCorpse", Array.Empty<object>());
            Scribe_References.Look(ref innerPawn, "innerPawn", true);
        }
    }
}
