using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using System;
using System.Reflection;
using VEF.AnimalGenes;
using System.Runtime.CompilerServices;

namespace AlphaMemes
{
    [HarmonyPatch]
    public static class AlphaMemes_JobDriver_PlantSow_MakeNewToils_Patch
    {
       
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            return typeof(JobDriver_PlantSow).GetNestedTypes(AccessTools.all).SelectMany(x => x.GetMethods(AccessTools.all)
                            .Where(y => y.Name.Contains("<MakeNewToils>") && y.ReturnType == typeof(void))).ToList()[1];
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddHistoryEvent(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            MethodInfo historyEventMethod = AccessTools.Method(typeof(HistoryEventsManager), nameof(HistoryEventsManager.RecordEvent));
            MethodInfo addHistoryEventMethod = AccessTools.Method(typeof(AlphaMemes_JobDriver_PlantSow_MakeNewToils_Patch), nameof(HistoryEvent));
            bool found = false;

            for (var i = 0; i < codes.Count; i++)
            {

                if (codes[i].opcode == OpCodes.Callvirt && codes[i].OperandIs(historyEventMethod))
                {
                    yield return codes[i];
                    if (!found)
                    {
                        
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldloc_1);
                        yield return new CodeInstruction(OpCodes.Call, addHistoryEventMethod);
                        found = true;
                    }
                    

                }

                else yield return codes[i];
            }
        }

        public static void HistoryEvent(Pawn actor, Plant plant)
        {
            if(plant?.Map!=null && !plant.Map.roofGrid.Roofed(plant.Position))
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(InternalDefOf.AM_SowedPlantOutside, actor.Named(HistoryEventArgsNames.Doer)));
            }
        }

       



    }
}