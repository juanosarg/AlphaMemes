using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace AlphaMemes
{
    public class GameComponent_ReligiousMemes : GameComponent
    {



        public GameComponent_ReligiousMemes(Game game) : base()
        {

        }

        public override void FinalizeInit()
        {
            if (DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Serketist") != null)
            {
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Serketist"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_ChthonianCult"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Pantheism"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Omnism"));
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_Structure_Shintaoism"));               
                StaticCollectionsClass.AddReligiousMeme(DefDatabase<MemeDef>.GetNamedSilentFail("VME_GodEmperor"));
               


            }



            base.FinalizeInit();
        }



    }


}
