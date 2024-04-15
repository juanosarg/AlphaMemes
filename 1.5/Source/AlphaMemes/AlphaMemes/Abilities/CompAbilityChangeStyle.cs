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
        public new CompProperties_AbilityChangeStyle Props => (CompProperties_AbilityChangeStyle)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {

            base.Apply(target, dest);

            if (Props.randomStyle)
            {
                HashSet<Thing> thingToChange = new HashSet<Thing>(target.Cell.GetThingList(this.parent.pawn.Map));
                if (thingToChange != null)
                {
                    foreach (Thing thing in thingToChange)
                    {
                        if (thing != null && thing.def.CanBeStyled())
                        {
                            List<StyleCategoryDef> listStyles = new List<StyleCategoryDef>();
                            if (AlphaMemes_Settings.makeChangeStyleAbilityUseAllStyles)
                            {
                                listStyles = (from x in DefDatabase<StyleCategoryDef>.AllDefsListForReading where x.GetStyleForThingDef(thing.def) != null select x).ToList();
                            }
                            else
                            {
                                List<ThingStyleCategoryWithPriority> listStylesWithPriority = Current.Game.World.factionManager.OfPlayer.ideos.PrimaryIdeo.thingStyleCategories;
                                foreach (ThingStyleCategoryWithPriority style in listStylesWithPriority)
                                {
                                    if (style.category.GetStyleForThingDef(thing.def) != null) { listStyles.Add(style.category); }

                                }
                            }
                            if (listStyles.Count > 0)
                            {
                                thing.StyleDef = listStyles.RandomElement().GetStyleForThingDef(thing.def);
                                thing.DirtyMapMesh(thing.Map);

                            }
                        }
                    }
                }

            }
            else if (Props.affectArea)
            {
                HashSet<Thing> thingsToChange = new HashSet<Thing>();
                foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, parent.pawn.Map, Props.area, useCenter: true))
                {

                    if (thing != null && thing.def.CanBeStyled())
                    {
                        thingsToChange.Add(thing);
                    }
                }
                if (!thingsToChange.NullOrEmpty())
                {
                    Dialog_ChangeStyles_Area window = new Dialog_ChangeStyles_Area(thingsToChange);
                    Find.WindowStack.Add(window);
                }

            }
            else if (Props.affectAllMap)
            {
                HashSet<Thing> thingsToChange = new HashSet<Thing>();
                foreach (Thing thing in parent.pawn.Map.listerThings.AllThings)
                {
                    if (thing != null && thing.def.CanBeStyled())
                    {
                        thingsToChange.Add(thing);
                    }
                }
                if (!thingsToChange.NullOrEmpty())
                {
                    Dialog_ChangeStyles_Area window = new Dialog_ChangeStyles_Area(thingsToChange);
                    Find.WindowStack.Add(window);
                }

            }
            else
            {
                HashSet<Thing> thingToChange = new HashSet<Thing>(target.Cell.GetThingList(this.parent.pawn.Map));
                if (thingToChange != null)
                {
                    foreach (Thing thing in thingToChange)
                    {
                        if (thing != null && thing.def.CanBeStyled())
                        {
                            Dialog_ChangeStyles window = new Dialog_ChangeStyles(thing);
                            Find.WindowStack.Add(window);
                        }
                    }
                }

            }



        }




    }
}
