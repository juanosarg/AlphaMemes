using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using RimWorld.Planet;
using System.Linq;


namespace AlphaMemes
{
    public class WorldComponent_GenericIdeosTracker : WorldComponent
    {

        public int tickCounter = tickInterval;
        public const int tickInterval = 60000;

        public bool teaBoost = false;

        public static WorldComponent_GenericIdeosTracker Instance;

        public WorldComponent_GenericIdeosTracker(World world) : base(world) => Instance = this;


        public override void WorldComponentTick()
        {

            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_TeaCultivation_Improved) != null)
                {
                    teaBoost = true;

                }else teaBoost = false;
             
                tickCounter = 0;
            }

        }


    }


}
