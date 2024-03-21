using System;
using RimWorld;
using Verse;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
    //This is to handle creating just 1 letter for all possible funeral obligations.
    //The letter gets pretty hefty when you stack them up
    public static class FuneralFramework_ObligationUtility
    {
        public static void Cleanup()
        {
            obligations.Clear();
            targets.Clear();
            rituals.Clear();
        }
        public static void SendLetter(Ideo ideo, Pawn pawn)
        {
            if(obligations.Count == 0) { return; }
            LookTargets lookTargets = targets;
            if (obligations.Count == 1)
            {
                RitualObligation obligation = obligations[0];
                Find.LetterStack.ReceiveLetter(obligation.LetterLabel, obligation.LetterText.Resolve(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
            }
            else
            {
                string label = "Funeral_OpportunitiesFor".Translate(pawn.Name.ToStringShort.Named("PAWN"));
                StringBuilder lettertext = new StringBuilder();
                lettertext.Append("Funeral_OptionsCount".Translate(obligations.Count.ToString()));
                foreach (RitualObligation obligation in obligations)
                {
                    lettertext.AppendLine().AppendLine();//Create some spacing
                    lettertext.AppendLine(obligation.precept.def.label.CapitalizeFirst().Colorize(obligation.precept.ideo.TextColor)).AppendLine(); //Including the def label so you can get a better idea which on it is because it only shows the name
                    lettertext.AppendLine(obligation.LetterLabel);
                    lettertext.AppendInNewLine(obligation.LetterText.Resolve());
                }
                Find.LetterStack.ReceiveLetter(label, lettertext.ToString(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null);
            }
            if(obligations.Count != rituals.Count)
            {
                Log.Warning("Alpha Memes Mismatch of obligations and Rituals in FuneralFramework_ObligationUtility"); //This shouldnt happen unless a ritual is adding more then one obligation per corpse. Which it shouldnt and should be investigated. Added mostly for mod devs, but if it ever comes up with users somethign to investigate.
                //Also because of me overriding sending letters per obligation and combining. The usual easy to tell way something is wrong of the res now 2 letters will not happen.
            }
        }

        public static List<RitualObligation> obligations = new List<RitualObligation>();
        public static List<TargetInfo> targets = new List<TargetInfo>();
        public static List<Precept_Ritual> rituals= new List<Precept_Ritual>();

    }




}
