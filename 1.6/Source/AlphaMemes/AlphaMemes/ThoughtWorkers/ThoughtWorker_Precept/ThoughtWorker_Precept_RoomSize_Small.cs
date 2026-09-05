
using RimWorld;
using System;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_RoomSize_Small : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p.needs.roomsize == null)
            {
                return ThoughtState.Inactive;
            }
            if (!p.Awake())
            {
                return ThoughtState.Inactive;
            }
            Room room = p.GetRoom();
            if (room == null || room.PsychologicallyOutdoors)
            {
                return ThoughtState.Inactive;
            }
            RoomSizeCategory curCategory = p.needs.roomsize.CurCategory;
           
            switch (curCategory)
            {
                case RoomSizeCategory.VeryCramped:
                    return ThoughtState.ActiveAtStage(0);
                case RoomSizeCategory.Cramped:
                    return ThoughtState.ActiveAtStage(1);
                case RoomSizeCategory.Normal:
                    return ThoughtState.Inactive;
                case RoomSizeCategory.Spacious:
                    return ThoughtState.ActiveAtStage(2);
                default:
                    throw new InvalidOperationException("Unknown RoomSizeCategory");
            }
        }
    }
}