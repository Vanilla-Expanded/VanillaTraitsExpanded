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
	[HarmonyPatch(typeof(JobGiver_GetRest))]
	[HarmonyPatch("TryGiveJob")]
	public static class TryGiveJob_Patch
	{
		public static bool Prefix(Pawn pawn)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Workaholic) && Rand.Chance(0.8f))
            {
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(JoyGiver_VisitGrave))]
	[HarmonyPatch("TryGiveJob")]
	public static class JoyGiver_VisitGrave_TryGiveJob_Patch
	{
		public static bool Prefix(Pawn pawn)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Anxious))
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(JoyGiver))]
	[HarmonyPatch("GetChance")]
	public static class GetChance_Patch
	{
		public static void Postfix(JoyGiver __instance, Pawn pawn, ref float __result)
		{
			if (__instance.def == DefDatabase<JoyGiverDef>.GetNamed("WatchTelevision") && pawn.HasTrait(VTEDefOf.VTE_CouchPotato))
			{
				__result *= 2f;
			}
		}
	}
}
