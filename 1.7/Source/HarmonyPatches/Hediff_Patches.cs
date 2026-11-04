using HarmonyLib;
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
	[HarmonyPatch(typeof(HediffComp_SeverityPerDay), "CompTipStringExtra", MethodType.Getter)]
    public static class CompTipStringExtra_Patch
    {
        public static void Postfix(HediffComp_SeverityPerDay __instance, ref string __result)
        {
            if (__result == null)
            {
                return;
            }
            if (__instance.parent.def == VTEDefOf.AlcoholAddiction && __instance.Pawn.HasTrait(VTEDefOf.VTE_Lush))
            {
                __result = "VTE.LushNeverRecovers".Translate();
            }
            else if (__instance.parent.def == VTEDefOf.SmokeleafAddiction && __instance.Pawn.HasTrait(VTEDefOf.VTE_Stoner))
            {
                __result = "VTE.StonerNeverRecovers".Translate();
            }
        }
    }
}
