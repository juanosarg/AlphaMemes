using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;
using System.Threading.Tasks;

namespace AlphaMemes
{ 
    public class FuneralFramework_ThingToSpawnMummy : FuneralFramework_ThingToSpawn
    {

        //Used to select thingdef for bodytype. Probably a way to do this with graphics but \o/ 
        public override ThingDef CustomThingDefLogic(LordJob_Ritual ritual, Pawn pawnToSpawnOn, Corpse corpse)
        {
            Gender gender = corpse.InnerPawn.gender;
            ThingDef thingdef = null;
            switch (gender)
            {
                case Gender.Male:
                    thingdef = InternalDefOf.AM_MummyMale;
                    break;
                case Gender.Female:
                    thingdef = InternalDefOf.AM_MummyFemale;
                    break;
                default: //Sorry enby friends no option here for you
                    thingdef = Rand.Bool ? InternalDefOf.AM_MummyMale : InternalDefOf.AM_MummyFemale;
                    break;
            }

            return thingdef;
        }

    }
}
