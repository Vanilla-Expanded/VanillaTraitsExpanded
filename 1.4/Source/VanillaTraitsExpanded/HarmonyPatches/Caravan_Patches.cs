using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using static Verse.DamageWorker;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(new Type[]
	{
		typeof(List<Pawn>),
		typeof(float),
		typeof(float),
		typeof(StringBuilder)
	})]
	[HarmonyPatch(typeof(CaravanTicksPerMoveUtility), "GetTicksPerMove")]
	public static class GetTicksPerMove_Patch
	{
		private static void Postfix(List<Pawn> pawns, ref int __result, float massUsage, float massCapacity, StringBuilder explanation = null)
		{
			if (pawns.Where(x => x.HasTrait(VTEDefOf.VTE_Wanderlust)).Count() == pawns.Count())
            {
				__result /= 2;
				if (explanation != null)
                {
					explanation.AppendLine("VTE.CavaranSpeedIsDoubledWanderlusts".Translate());
                }
            }
		}
	}


	[HarmonyPatch(typeof(Pawn), "ExitMap")]
	public static class ExitMap_Patch
	{
		private static void Postfix(Pawn __instance)
		{
			if (__instance.HasTrait(VTEDefOf.VTE_Wanderlust))
            {
				TraitsManager.Instance.wanderLustersWithLastMapExitedTick[__instance] = GenTicks.TicksAbs;
			}
		}
	}
}
