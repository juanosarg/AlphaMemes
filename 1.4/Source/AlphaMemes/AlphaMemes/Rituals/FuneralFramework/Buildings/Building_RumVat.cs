using System;
using System.Linq;
using System.Collections.Generic;
using Verse;
using Verse.Sound;
using System.Text;
using RimWorld;
using UnityEngine;



namespace AlphaMemes
{
    [StaticConstructorOnStartup]
    public class Building_RumVat : Building_CorpseCasket
    {
        public int tickToFerment;
        public int age = 0;
        public int tickFilled;
        public bool isFuneral = false;
        public static float zOffset = 0.51f;
        public static float xOffset = 0.51f;
        public static float topYOffset = 1.15f;
        public static float topXOffset = 0.0f;
        private static readonly int maxAge = 3;
        public static readonly Material vatTop = MaterialPool.MatFrom("Things/Building/Misc/AM_RumBarrel");

        public void Notify_CorpseBuried()
        {
            tickToFerment = Find.TickManager.TicksGame + FermentTime;        
        }
        public override void Draw()
        {
            base.Draw();
            if (HasCorpse)
            {
                Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(Rotation.AsAngle);
                Corpse.DrawAt(Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + new Vector3(xOffset, 0, zOffset), false);
            }
            var pos = this.TrueCenter();
            pos.y += topYOffset;
            pos.z += topXOffset;  
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.identity, new Vector3(2, 1, 2));
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
                if( tickToFerment == 0)
                {
                    Notify_CorpseBuried();//Because the job driver doesn't actually call anything when hauled
                }
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
            if (age == 0)
            {
                //Dialog box doesn't work here because it needs to go on the gizmo *todo
                /*    Dialog_MessageBox.CreateConfirmation("AM_RumVatInteruptFerment".Translate(), () =>
                    {
                        base.Open();
                        Cleanup();
                    });*/
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
            var pawnSource = rum.TryGetComp<CompHasPawnSources>();
            if(pawnSource != null)
            {
                pawnSource.AddSource(Corpse.InnerPawn);
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
        //Soooo this is actaully never used, its only used for cyrpto casket, not making my own job for this so adding to tick
        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            return base.TryAcceptThing(thing, allowSpecialEffects);
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
                    ticksLeft = ticksLeft < 0 ? 0 : ticksLeft;
                    float ticksPassed = FermentTime - ticksLeft;
                    float progress = ticksPassed / FermentTime;     
                    if(tickToFerment != 0)
                    {
                        stringBuilder.AppendInNewLine("FermentationProgress".Translate(progress.ToStringPercent(), ticksLeft.ToStringTicksToPeriod()));
                        if(age > 0)
                        {
                            stringBuilder.AppendInNewLine("AM_RumOutput".Translate(RumCount));                             
                        }
                        if (age == 3)
                        {
                            stringBuilder.AppendInNewLine("AM_RumComplete".Translate());
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
