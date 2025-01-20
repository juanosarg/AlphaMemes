using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using RimWorld.Planet;


namespace AlphaMemes
{
    public class WorldComponent_Relics : WorldComponent
    {
       
        public int relicsDestroyedThisGame = 0;

        public static WorldComponent_Relics Instance;

        public WorldComponent_Relics(World world) : base(world) => Instance = this;

       
        public override void ExposeData()
        {
            base.ExposeData();         
            Scribe_Values.Look<int>(ref this.relicsDestroyedThisGame, "relicsDestroyedThisGame", 0, true);
          
        }
    

    }


}
