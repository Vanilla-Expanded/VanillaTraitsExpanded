using HarmonyLib;
using Verse;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Pawn), "Destroy")]
	public static class Destroy_Patch
	{
		private static void Prefix(Pawn __instance)
		{
            TraitsManager.Instance.RemoveDestroyedPawn(__instance);
		}
	}
}
