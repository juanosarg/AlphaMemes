using RimWorld;
using UnityEngine;
using Verse;


namespace AlphaMemes
{



    public class AlphaMemes_Mod : Mod
    {


        public AlphaMemes_Mod(ModContentPack content) : base(content)
        {
            GetSettings<AlphaMemes_Settings>();
        }
        public override string SettingsCategory()
        {

            return "Alpha Memes";



        }



        public override void DoSettingsWindowContents(Rect inRect)
        {
            AlphaMemes_Settings.DoWindowContents(inRect);
        }
    }


}
