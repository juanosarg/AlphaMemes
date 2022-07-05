using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(RitualRoleAssignments))]
    [HarmonyPatch("ExposeData")]
    //This patch is to get around an issue where the expose data cant save reference to a corpse. Which causes errors when you load a save made during middle of ritual
    public static class RitualRoleAssignments_ExposeData_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(RitualRoleAssignments __instance, List<Pawn> ___allPawns, Dictionary<string, SerializablePawnList> ___assignedRoles)
        {
            if(Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (__instance.Ritual.def.HasModExtension<FuneralPreceptExtension>())
                {
                    //Fixing the lists
                    RitualBehaviorWorker_FuneralFramework behavior = __instance.Ritual.behavior as RitualBehaviorWorker_FuneralFramework;
                    if(behavior == null) { return; }    
                    Pawn pawn = behavior.corpse;
                    if (___allPawns.Any(x => x == null))
                    {
                        ___allPawns.RemoveAll(x => x == null);
                        ___allPawns.Add(pawn);
                    }
                    
                    if (___assignedRoles.Values.Any(x=> x.Pawns.Contains(null)))
                    {
                        string key = ___assignedRoles.FirstOrDefault(x => x.Value.Pawns.Any(y=>y==null)).Key;
                        ___assignedRoles.Remove(key);
                        SerializablePawnList serializablePawnList = new SerializablePawnList(new List<Pawn>());
                        serializablePawnList.Pawns.Add(pawn);
                        ___assignedRoles.Add(key, serializablePawnList);                        
                    }
                    
                }
            }




        }

    }

}
