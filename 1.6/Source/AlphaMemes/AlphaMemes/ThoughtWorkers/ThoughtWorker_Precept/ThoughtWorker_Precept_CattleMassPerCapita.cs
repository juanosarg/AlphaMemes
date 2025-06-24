
using System.Collections.Generic;
using AlphaMemes;
using RimWorld;
using Verse;
namespace AlphaMemes
{
    public class ThoughtWorker_Precept_CattleMassPerCapita : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
    {
        private const float NoAnimals = 0f;

        private const float ScarceAnimals = 1f;

        private const float FewAnimals = 2f;

        private const float NoThought = 4f;

        private const float SomeAnimals = 6f;

        private const float LotsOfAnimals = 8f;

        private const int MinimumTicksBeforeFewAnimals = 900000;

        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p.Map?.IsPlayerHome != true)
            {
                return false;
            }
            if (p.IsSlave)
            {
                return false;
            }
            float num = PlayerCattleBodySizePerCapita();
            if (num <= 2f && GenTicks.TicksAbs < 900000)
            {
                return false;
            }
            if (num < 4f && def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < def.minExpectationForNegativeThought.order)
            {
                return false;
            }
            if (ThoughtStageFromAnimalDensity(num) < 0)
            {
                return false;
            }
            return ThoughtState.ActiveAtStage(ThoughtStageFromAnimalDensity(PlayerCattleBodySizePerCapita()));
        }

        public override string PostProcessDescription(Pawn p, string description)
        {
            return base.PostProcessDescription(p, description) + "\n\n" + "AM_CurrentTotalCattleBodySizePerColonist".Translate() + ": " + PlayerCattleBodySizePerCapita().ToString("F1") + "\n" + "MinAnimalBodySizePerColonist".Translate(4f.ToString("F1"));
        }

        private int ThoughtStageFromAnimalDensity(float density)
        {
            if (density < 0f)
            {
                return 0;
            }
            if (density < 1f)
            {
                return 1;
            }
            if (density < 2f)
            {
                return 2;
            }
            if (density < 4f)
            {
                return -1;
            }
            if (density < 6f)
            {
                return 3;
            }
            if (density < 8f)
            {
                return 4;
            }
            return 5;
        }

        public IEnumerable<NamedArgument> GetDescriptionArgs()
        {
            yield return ((string)("(" + "AnimalsBodySizePerColonist".Translate() + ": ") + 1f + ")").Named("STAGE1");
            yield return ((string)("(" + "AnimalsBodySizePerColonist".Translate() + ": ") + 2f + ")").Named("STAGE2");
            yield return ((string)("(" + "AnimalsBodySizePerColonist".Translate() + ": ") + 6f + ")").Named("STAGE4");
            yield return ((string)("(" + "AnimalsBodySizePerColonist".Translate() + ": ") + 8f + ")").Named("STAGE5");
        }

        public static float PlayerCattleBodySizePerCapita()
        {
            float num = 0f;
            int num2 = 0;
            List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
            for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.Count; i++)
            {
                Pawn pawn = allMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction[i];
                if (pawn.IsFreeColonist && !pawn.IsQuestLodger())
                {
                    num2++;
                }
                if (StaticCollections.cattleAnimals.Contains(pawn.kindDef))
                {
                    num += pawn.BodySize;
                }
            }
            if (num2 <= 0)
            {
                return 0f;
            }
            return num / (float)num2;
        }

    }
}