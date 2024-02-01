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
    public class Building_RumVat : Building_Grave
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
        private bool needFix = false;

        public void Notify_CorpseBuried()
        {
            
            
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
                if (needFix)
                {
                    needFix = false;
                    var corpse = Corpse;
                    for(int i = 0; i < innerContainer.Count; i++)
                    {
                        if (innerContainer[i] != corpse)
                        {
                            innerContainer.TryDrop(innerContainer[i], ThingPlaceMode.Near, innerContainer[i].stackCount, out var outThing);
                        }
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
        public override void Notify_CorpseBuried(Pawn worker)
        {
            tickToFerment = Find.TickManager.TicksGame + FermentTime;
            if (Corpse.AnythingToStrip())
            {
                Corpse.Strip();
            }            
            base.Notify_CorpseBuried(worker);
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
            if (!base.Accepts(thing))
            {
                return false;
            }
            var comp = thing.TryGetComp<CompRottable>();
            if (comp?.Stage != RotStage.Fresh)
            {
                return false;
            }
            //All of this is to prevent the vat from being loaded with a colonist with an active funeral from being auto loaded
            var corpse = thing as Corpse;
            if (corpse.InnerPawn.IsColonist)
            {
                var precept = corpse.InnerPawn.Faction.ideos.GetPrecept(InternalDefOf.AM_RumBurial) as Precept_Ritual;
                if(precept != null)
                {
                    var obligations = precept.activeObligations;
                    if(!obligations.NullOrEmpty())
                    {
                        if(obligations.Any(x=>x.targetA == corpse.InnerPawn))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (base.TryAcceptThing(thing, allowSpecialEffects))
            {
                return true;
            }
            return false;
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
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                foreach(var thing in innerContainer)
                {
                    if(thing != Corpse)
                    {
                        needFix = true;
                    }
                }
            }
        }
    }


}
