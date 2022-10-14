using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace AlphaMemes
{
    public class GameComponent_ReligiousMemesAndRelics : GameComponent
    {
       
        public int relicsDestroyedThisGame = 0;

        public GameComponent_ReligiousMemesAndRelics(Game game) : base()
        {

        }
        public override void ExposeData()
        {
            base.ExposeData();         
            Scribe_Values.Look<int>(ref this.relicsDestroyedThisGame, "relicsDestroyedThisGame", 0, true);
          
        }
        public override void FinalizeInit()
        {
            StaticCollectionsClass.relicsDestroyedThisGame = relicsDestroyedThisGame;

            if (DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Serketist") != null)
            {
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Serketist"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_ChthonianCult"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Pantheism"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Omnism"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Shintaoism"));               
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_GodEmperor"));
               


            }

            if (DefDatabase<MemeDef>.GetNamedSilentFail("VME_ViolentConversion") != null)
            {
                StaticCollectionsClass.AddProselytizerMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_ViolentConversion"));
               



            }



            base.FinalizeInit();
        }

       



    }


}
