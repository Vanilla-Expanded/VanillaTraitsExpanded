﻿using HarmonyLib;
using Verse;

namespace VanillaTraitsExpanded
{
    [HarmonyPatch(typeof(Hediff), "ShouldRemove", MethodType.Getter)]
    public static class ShouldRemove_Patch
    {
        public static void Postfix(Hediff __instance, ref bool __result)
        {
            if (__result)
            {
                if (__instance.def == VTEDefOf.AlcoholAddiction && __instance.pawn.HasTrait(VTEDefOf.VTE_Lush))
                {
                    __result = false;
                }
                else if (__instance.def == VTEDefOf.SmokeleafAddiction && __instance.pawn.HasTrait(VTEDefOf.VTE_Stoner))
                {
                    __result = false;
                }
            }
        }
    }
}
