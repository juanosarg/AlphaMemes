using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using RimWorld.Planet;
using System.Linq;


namespace AlphaMemes
{
    public class WorldComponent_RandomMood : WorldComponent
    {

        public int tickCounter = tickInterval;
        public const int tickInterval = 60000;
        public Dictionary<Pawn, int> colonist_and_random_mood = new Dictionary<Pawn, int>();
        List<Pawn> list2;
        List<int> list3;

        public static WorldComponent_RandomMood Instance;

        public WorldComponent_RandomMood(World world) : base(world) => Instance = this;

       

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref colonist_and_random_mood, "colonist_and_random_mood", LookMode.Reference, LookMode.Value, ref list2, ref list3);

            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterRandomMood", 0, true);

        }

        public override void WorldComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_Mood_Volatile) != null)
                {
                    List<Pawn> allPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.InRandomOrder().ToList();
                    foreach (Pawn pawn in allPawns)
                    {
                        int randomMood = Rand.RangeSeeded(0, 9, Current.Game.tickManager.TicksAbs + allPawns.IndexOf(pawn));
                        AddColonistRandomMood(pawn, randomMood);

                    }

                }

               
                tickCounter = 0;
            }



        }

        public void AddColonistRandomMood(Pawn pawn, int mood)
        {
            if (!colonist_and_random_mood.ContainsKey(pawn))
            {
                colonist_and_random_mood.Add(pawn, mood);
            }
            else { colonist_and_random_mood[pawn] = mood; }
        }


    }


}
