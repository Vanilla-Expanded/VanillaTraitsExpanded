using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Need_Beauty), "CurCategory", MethodType.Getter)]
	public static class CurCategory_Patch
	{
		private static bool Prefix(Need_Beauty __instance, Pawn ___pawn, ref BeautyCategory __result)
		{
			if (___pawn.HasTrait(VTEDefOf.VTE_Slob) && __instance.CurLevel < 0.35f)
            {
				__result = BeautyCategory.Neutral;
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Need_Joy), "FallPerInterval", MethodType.Getter)]
	public static class FallPerInterval_Patch
	{
		private static void Postfix(Pawn ___pawn, ref float __result)
		{
			if (___pawn.HasTrait(VTEDefOf.VTE_WorldWeary))
			{
				__result *= 2f;
			}
		}
	}
}
