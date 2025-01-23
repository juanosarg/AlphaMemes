using RimWorld;
using RimWorld.Planet;
using Verse;
using RimWorld.QuestGen;
using AlphaMemes;

namespace AlphaMemes
{

    public class WorldComponent_KillsTracker : WorldComponent
    {
        public int ticksWithoutSanguoKill = 0;

        public static WorldComponent_KillsTracker Instance;

        public WorldComponent_KillsTracker(World world) : base(world) => Instance = this;

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
            if (ticksWithoutSanguoKill <= int.MaxValue) {
                ticksWithoutSanguoKill++;
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.ticksWithoutSanguoKill, nameof(this.ticksWithoutSanguoKill));
           
        }
    }
}
