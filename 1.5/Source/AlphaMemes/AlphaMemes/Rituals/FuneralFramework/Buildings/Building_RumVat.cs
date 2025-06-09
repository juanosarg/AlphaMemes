﻿using System;
using System.Linq;
using System.Collections.Generic;
using Verse;
using Verse.Sound;
using System.Text;
using RimWorld;
using UnityEngine;
using LudeonTK;



namespace AlphaMemes
{
    [StaticConstructorOnStartup]
    public class Building_RumVat : Building_Grave
    {
        public int tickToFerment;
        public int age = 0;
        public int tickFilled;
        public bool isFuneral = false;
        public static float zOffset = 0.45f;
        public static float xOffset = 0.51f;
        public static float topXOffset = 0.0f;
        //[TweakValue("00", -10, 10)] public static float topYOffset = 1.0f;
        public static float topYOffset = 1.0f;
        private static readonly int maxAge = 3;
        public static readonly Material vatTop = MaterialPool.MatFrom("Things/Building/Misc/AM_RumBarrel");

        public CompAssignableToPawn_RumCasket CompAssignableToPawn_RumCasket => GetComp<CompAssignableToPawn_RumCasket>();

        public void Notify_CorpseBuried()
        {
            tickToFerment = Find.TickManager.TicksGame + 180000;        
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc,flip);
            if (HasCorpse)
            {
                Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(Rotation.AsAngle);
                Corpse.DynamicDrawPhaseAt(DrawPhase.Draw,Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + new Vector3(xOffset, 0, zOffset), false);
            }
            var pos = this.TrueCenter();
            pos.y += topYOffset;
            pos.z += topXOffset;
            var matrix = Matrix4x4.TRS(pos, Rotation.AsQuat, new Vector3(2, 1f, 2));
            Graphics.DrawMesh(MeshPool.plane10, matrix, vatTop, 0);
        }
        private int FermentTime
        {
            get
            {
       
                if(age == 0)
                {
                    return 180000;
                }
                if(age == 1)
                {
                    return 60000 * 7;
                }
                if(age== 2)
                {
                    return 60000 * 21;
                }
                return 0;
            }
        }
        public override void TickRare()
        {
            if (HasCorpse)
            {
                if (tickToFerment > 0 && tickToFerment < Find.TickManager.TicksGame && tickToFerment > 0 && age < maxAge)
                {
                    age++;
                    if(age == 3)
                    {
                        tickToFerment = 0;
                        Map.designationManager.AddDesignation(new Designation(this, DesignationDefOf.Open));
                        Messages.Message("AM_RumComplete".Translate(),new LookTargets(this),MessageTypeDefOf.NeutralEvent);
                    }
                    {
                        tickToFerment = Find.TickManager.TicksGame + FermentTime;
                    }                    
                }
            }
            base.TickRare();
        }
        private int RumCount
        {
            get
            {
                return age * 25;
            }
        }
        public override void Open()
        {
            if (age == 0) //If it hasn't fermented yet spit out corpse like regular grave
            {
                base.Open();
                Cleanup();
                return;
            }
            var rumDef = isFuneral ? InternalDefOf.AM_CorpseRumFine : InternalDefOf.AM_CorpseRumBasic;
            var skull = ThingMaker.MakeThing(ThingDefOf.Skull);
            var compSource = skull.TryGetComp<CompHasSources>();
            if(compSource != null)
            {
                compSource.AddSource(Corpse.InnerPawn.LabelShort);
            }
            GenPlace.TryPlaceThing(skull, PositionHeld, Map, ThingPlaceMode.Near, null, null, default(Rot4));//place skull
            var rum = ThingMaker.MakeThing(rumDef);
            compSource = rum.TryGetComp<CompHasSources>();
            if( compSource != null)
            {
                compSource.AddSource(Corpse.InnerPawn.LabelShort);
            }
            rum.stackCount = RumCount;
            GenPlace.TryPlaceThing(rum,PositionHeld,Map, ThingPlaceMode.Near, null, null, default(Rot4));
            //Cleanup the container for next corpse
            innerContainer.ClearAndDestroyContents();//Destroy the corpse
            Cleanup();
        }
        
        private void Cleanup()
        {
            age = 0;
            tickToFerment = 0;
            isFuneral = false;
            if (AssignedPawn != null) 
            { 
                CompAssignableToPawn_RumCasket.TryUnassignPawn(AssignedPawn);
            }
        }
        public override bool Accepts(Thing thing)
        {
            //has to be a fresh corpse       
            if (base.Accepts(thing))
            {
                var comp = thing.TryGetComp<CompRottable>();                
                if (comp?.Stage != RotStage.Fresh)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
        {
            base.Notify_HauledTo(hauler, thing, count);
            Notify_CorpseBuried();
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (HasCorpse)
            {
                if (Tile != -1)
                {
                    //! not max Fermenting x days left
                    //! 0 age, open to recieve x rum
                    var ticksLeft = tickToFerment - Find.TickManager.TicksGame;
                    float progress = ticksLeft / FermentTime;     
                    if(tickToFerment != 0)
                    {
                        stringBuilder.AppendInNewLine("FermentationProgress".Translate(progress.ToStringPercent(), ticksLeft.ToStringTicksToPeriod()));
                        if(age > 0)
                        {
                            stringBuilder.AppendInNewLine("AM_RumOutput".Translate(RumCount));                             
                        }
                    }                   
                 }
            }
            return stringBuilder.ToString();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickToFerment, "tickToFerment");
            Scribe_Values.Look(ref isFuneral, "isFuneral");
            Scribe_Values.Look(ref age, "age");
        }
    }


}
