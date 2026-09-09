using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace AlphaMemes
{
    public class AlphaMemes_Mod : Mod
    {
        public static Harmony harmony;

        public AlphaMemes_Mod(ModContentPack content) : base(content)
        {
            harmony = new Harmony("com.alphamemes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            if (!ModLister.HasActiveModWithName("Vanilla Ideology Expanded - Memes and Structures"))
            {
                PatchVEMemesOptions(harmony);
            }
            GetSettings<AlphaMemes_Settings>();
        }
        public override string SettingsCategory()
        {
            return "Alpha Memes";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            AlphaMemes_Settings.DoWindowContents(inRect);
        }



        public static void PatchVEMemesOptions(Harmony harmony)
        {
            var method = AccessTools.PropertyGetter(typeof(Dialog_ChooseMemes), "MemeCountRangeAbsolute");
            var postfix = typeof(AlphaMemes_Dialog_ChooseMemes_MemeCountRangeAbsolute_Patch).GetMethod("SetMaxToOptions");
            harmony.Patch(method, postfix: postfix);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoMemes");
            var prefix = typeof(AlphaMemes_IdeoUIUtility_DoMemes_Patch).GetMethod("MakeBoxSmaller");
            var prefix2 = typeof(AlphaMemes_IdeoUIUtility_DoMemes_Patch).GetMethod("MoreThanEightMemes");
            postfix = typeof(AlphaMemes_IdeoUIUtility_DoMemes_Patch).GetMethod("MakeBoxBigger");
            harmony.Patch(method, prefix: prefix);
            harmony.Patch(method, prefix: prefix2);
            harmony.Patch(method, postfix: postfix);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoPrecepts");
            postfix = typeof(AlphaMemes_IdeoUIUtility_DoPrecepts_Amount_Patch).GetMethod("EnableMorePrecepts");
            harmony.Patch(method, postfix: postfix);

            method = AccessTools.Method(typeof(IdeoUIUtility), "AddPrecept");
            var transpiler = typeof(AlphaMemes_IdeoUIUtility_AddPrecept_Patch).GetMethod("TranspileAddPrecept");
            harmony.Patch(method, transpiler: transpiler);

            method = AccessTools.Method(typeof(IdeoUIUtility), "DoStyles");
            transpiler = typeof(AlphaMemes_IdeoUIUtility_DoStyles_Patch).GetMethod("TranspileStyles");
            harmony.Patch(method, transpiler: transpiler);

            method = AccessTools.Method(typeof(IdeoUtility), "IsMemeAllowedForInitialFluidIdeo");
            postfix = typeof(AlphaMemes_IdeoUtility_IsMemeAllowedForInitialFluidIdeo_Patch).GetMethod("AllowAllMemes");
            harmony.Patch(method, postfix: postfix);

        }

    }


}
