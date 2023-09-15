using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(WorkGiver_Researcher))]
	[HarmonyPatch("ShouldSkip")]
	public static class ShouldSkip_Patch
	{
		private static void Postfix(ref bool __result, Pawn pawn, bool forced = false)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Technophobe) && Find.ResearchManager.currentProj?.techLevel > TechLevel.Medieval)
            {
				__result = false;
            }
		}
	}
}
