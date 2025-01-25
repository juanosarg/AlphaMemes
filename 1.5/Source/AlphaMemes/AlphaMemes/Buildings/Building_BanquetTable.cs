using Verse;
using Verse.Grammar;

namespace AlphaMemes
{
    public class Building_BanquetTable : Building
    {

        public string labelText = "";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.labelText, "labelText", "", false);
        }

        public override string Label
        {
            get
            {
                if (labelText == "")
                {
                    GrammarRequest request = default(GrammarRequest);
                    request.Includes.Add(def.ideoBuildingNamerBase);


                    labelText = GrammarResolver.Resolve("buildingName", request, null, forceLog: false, null, null, null, capitalizeFirstSentence: false);
                }

                return labelText;
            }
        }
    }
}