using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace AlphaMemes
{
    public class GameComponent_WeaponRanges : GameComponent
    {



      
        public Dictionary<Thing, float> weapon_ranges = new Dictionary<Thing, float>();
        List<Thing> list2;
        List<float> list3;


        public GameComponent_WeaponRanges(Game game) : base()
        {

        }

      

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref weapon_ranges, "weapon_ranges", LookMode.Reference, LookMode.Value, ref list2, ref list3);

        }

        public void AddWeaponAndRange(Thing weapon, float range)
        {
            if (!weapon_ranges.ContainsKey(weapon))
            {
                weapon_ranges.Add(weapon, range);
            }
            
        }

        public void RemoveWeaponAndRange(Thing weapon)
        {
            if (weapon_ranges.ContainsKey(weapon))
            {
                weapon_ranges.Remove(weapon);
            }

        }

    }


}
