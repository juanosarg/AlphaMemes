using RimWorld;
using RimWorld.Planet;
using Verse;
using RimWorld.QuestGen;
using AlphaMemes;

namespace AlphaGenes
{

    public class WorldComponent_SanguophageCampQuests : WorldComponent
    {
        public int tickCounter;
        public int ticksToNextQuest = 60000 * 30;

        public WorldComponent_SanguophageCampQuests(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            if (tickCounter > ticksToNextQuest)
            {
                if(Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.AM_SanguophageCamps_RaidingDesired) != null)
                {
                    Slate slate = new Slate();
                    Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.AM_OpportunitySite_SanguophageCamp, slate);

                    QuestUtility.SendLetterQuestAvailable(quest);
                    ticksToNextQuest = (int)(60000 * Rand.RangeInclusive(28, 32));

                }
                
                tickCounter = 0;

            }
            tickCounter++;

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
            Scribe_Values.Look(ref this.ticksToNextQuest, nameof(this.ticksToNextQuest));
        }
    }
}
