using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using HarmonyLib;

namespace AlphaMemes
{

    [HarmonyPatch(typeof(Dialog_BeginRitual))]
    [HarmonyPatch("BlockingIssues")]

    public static class AlphaMemes_Dialog_BeginRitual_BlockingIssues_Patch
    {

        public static IEnumerable<string> Postfix(IEnumerable<string> values, Precept_Ritual ___ritual)
        {
            if (___ritual?.def.GetModExtension<FuneralPreceptExtension>()?.isColonistFuneral==true)
            {
                List<string> resultingList = values.ToList();

                foreach (string issue in values)
                {
                    if (issue == "CantJoinRitualInExtremeWeather".Translate())
                    {
                        resultingList.Remove(issue);
                    }
                }
                return resultingList;
            }
            else return values;





        }

    }





}
