using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;



namespace AlphaMemes
{
    public class ReflectionCache
    {

        public delegate void DoPreceptsIntDelegate(string stringDespised,string stringAddDespised,bool mainPrecepts,
           Ideo ideo, IdeoEditMode editMode,ref float curY,float width,Func<PreceptDef, bool> filter,bool group);

        public static readonly DoPreceptsIntDelegate DoPreceptsInt =(DoPreceptsIntDelegate)Delegate.CreateDelegate(
                typeof(DoPreceptsIntDelegate),AccessTools.Method(typeof(IdeoUIUtility), "DoPreceptsInt"));

       
    }
}
