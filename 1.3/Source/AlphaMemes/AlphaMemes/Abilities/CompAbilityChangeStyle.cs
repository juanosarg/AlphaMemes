using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld;
using Verse;
using System.Linq;

namespace AlphaMemes
{
    public class CompAbilityChangeStyle : CompAbilityEffect
    {


        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {

            base.Apply(target, dest);

            List<ThingStyleCategoryWithPriority> listStyles = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo.thingStyleCategories;




            HashSet<Thing> hashSet = new HashSet<Thing>(target.Cell.GetThingList(this.parent.pawn.Map));
            if (hashSet != null)
            {
                foreach (Thing thing in hashSet)
                {
                    if (thing != null && thing.def.CanBeStyled())
                    {

                        if (AlphaMemes_Settings.makeChangeStyleAbilityUseAllStyles) {

                           

                            List<StyleCategoryDef> listStylesAll = (from x in DefDatabase<StyleCategoryDef>.AllDefsListForReading
                                                                   where x.GetStyleForThingDef(thing.def) != null select x).ToList();

                            thing.StyleDef = listStylesAll.RandomElement().GetStyleForThingDef(thing.def); 
                        } else { 
                            thing.StyleDef = listStyles.RandomElement().category.GetStyleForThingDef(thing.def); 
                        }
                        thing.DirtyMapMesh(thing.Map);
                    }

                }
            }








        }



    }
}
