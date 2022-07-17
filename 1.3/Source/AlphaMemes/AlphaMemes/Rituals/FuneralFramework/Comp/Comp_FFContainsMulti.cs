using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class Comp_CorpseContainerMulti : Comp_CorpseContainer
    {

        public List<Pawn> innerPawns = new List<Pawn>();
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
        public override string CompInspectStringExtra()
        {
            if (!Active || Props.inspectString == null)
            {
                return base.CompInspectStringExtra();
            }       
            string corpses = string.Join(", ", innerPawns.Select(x => x.NameShortColored));
            
            return Props.inspectString.Formatted(corpses.Named("CORPSES"));
        }

        public override string TransformLabel(string label)
        {
            return base.TransformLabel(label);
        }
        public override void InitComp_CorpseContainer(Corpse corpse)
        {
            string deathDate = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(corpse.timeOfDeath), Find.WorldGrid.LongLatOf(corpse.Tile));
            innerPawns.Add(corpse.InnerPawn);
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
        }
    }
}
