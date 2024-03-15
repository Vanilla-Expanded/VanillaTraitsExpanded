using HarmonyLib;
using Verse;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Pawn), "Destroy")]
	public static class Destroy_Patch
	{
		private static void Prefix(Pawn __instance)
		{
			var comp = Current.Game.GetComponent<TraitsManager>();
			comp.RemoveDestroyedPawn(__instance);
		}
	}
}
