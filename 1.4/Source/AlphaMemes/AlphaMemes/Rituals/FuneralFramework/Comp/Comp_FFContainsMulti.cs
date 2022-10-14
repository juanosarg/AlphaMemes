using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace AlphaMemes
{ 
    public class Comp_CorpseContainerMulti : Comp_CorpseContainer
    {

        public List<Pawn> innerPawns = new List<Pawn>();
        public List<Corpse> innerCorpses = new List<Corpse>();//I might do something eventually to let ppl pull a corpse out. I didnt think about resurrect purposes at first. Adding this now so if I get the urge to figure out that headache its somewhattt backwards compatible (corpse wont tick so things like decay will need to be sorted out)
        public Dictionary<string, string> pawnNameDateDeath = new Dictionary<string, string>();
        private CompProperties_CorpseContainerMulti Props
        {
            get
            {
                return (CompProperties_CorpseContainerMulti)props;
            }
        }
        public override Pawn VisitedPawn
        {
            get
            {
                return innerPawns.RandomElement();
            }
        }
        public override bool Active 
        {
            get { return innerPawns.Count> 0; }
        }
        public bool NotFull
        {
            get { return innerPawns.Count < Props.maxCorpse; }
        }
        public override void RemoveCorpse(Corpse corpse)
        {
            base.RemoveCorpse(corpse);
            innerCorpses.Remove(corpse);
            innerPawns.Remove(corpse.InnerPawn);
            var key = corpse.InnerPawn.NameFullColored.CapitalizeFirst();//Why did I do it like this...Stupid me
            if (pawnNameDateDeath.ContainsKey(key))
            {
                pawnNameDateDeath.Remove(key);
            }

        }
        public override string CompInspectStringExtra()
        {
            if (!Active || Props.inspectString == null)
            {
                return base.CompInspectStringExtra();
            }       
            string corpses = string.Join(", ", innerPawns.Select(x => x.NameShortColored));
            
            return Props.inspectString.Formatted(corpses.Named("CORPSES"));
        }
        public override void CompTickRare()
        {
            if (Active)
            {
                foreach (var corpse in innerCorpses)
                {
                    if (!corpse.Destroyed)
                    {
                        var compRot = corpse.GetComp<CompRottable>();
                        compRot.CompTickRare();
                    }
                }
            }

        }
        public override string TransformLabel(string label)
        {
            return base.TransformLabel(label);
        }
        public override bool CanOpen => innerCorpses.Any(x=>!x.Destroyed);
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (CanOpen && Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DesignatorOpen".Translate(),
                    defaultDesc = "AM_CorpseContainerOpen".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true),
                    action = () =>
                    {
                        var options = new List<FloatMenuOption>();
                        foreach (var corpse in innerCorpses)
                        {
                            if (!corpse.Destroyed)
                            {
                                options.Add(new FloatMenuOption(corpse.InnerPawn.NameFullColored, () => RemoveCorpse(corpse)));
                            }
                        }
                        Find.WindowStack.Add(new FloatMenu(options));
                    }
                };
            }
        }
        public override void InitComp_CorpseContainer(Corpse corpse)
        {
            string deathDate = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(corpse.timeOfDeath), Find.WorldGrid.LongLatOf(corpse.Tile));
            innerPawns.Add(corpse.InnerPawn);
            innerCorpses.Add(corpse);
            pawnNameDateDeath.Add(corpse.InnerPawn.NameFullColored.CapitalizeFirst(), deathDate);
            
        }
        public override string GetDescriptionPart()
        {
            if (!Active)
            {
                return base.GetDescriptionPart();
            }
            StringBuilder sB = new StringBuilder("CasketContains".Translate());
            foreach (KeyValuePair<string, string> kvp in pawnNameDateDeath)
            {
                sB.AppendLine();
                sB.AppendWithSeparator("AM_CorpseContainerMultiDiedOn".Translate(kvp.Key.Named("NAME"), kvp.Value.Named("DATE")),", ");
            }
            return sB.ToString();

        }


        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref pawnNameDateDeath, "pawnNameDateDeath", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref innerPawns,true, "innerPawns",LookMode.Reference);
            if (innerCorpses?.Any(x => x.Destroyed) ?? false) 
            {
                innerCorpses.RemoveAll(x => x.Destroyed);
            }
            Scribe_Collections.Look(ref innerCorpses, false, "innerCorpses", LookMode.Deep);
        }
    }
}
