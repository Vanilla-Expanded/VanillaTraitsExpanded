using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Lord))]
	[HarmonyPatch("AddPawn")]

	public static class AddPawn_Patch
	{
		private static bool Prefix(Lord __instance, Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Anxious) && !(__instance.LordJob is LordJob_BestowingCeremony) && (__instance.LordJob is LordJob_Joinable_Gathering 
				|| __instance.LordJob is LordJob_Joinable_MarriageCeremony marriageCeremony && marriageCeremony.firstPawn != p && marriageCeremony.secondPawn != p))
            {
				return false;
            }
			return true;
		}
	}

	[HarmonyPatch(typeof(GatheringsUtility))]
	[HarmonyPatch("ShouldPawnKeepGathering")]

	public static class ShouldPawnKeepGathering_Patch
	{
		private static bool Prefix(Pawn p, GatheringDef gatheringDef)
		{
			if (p.HasTrait(VTEDefOf.VTE_Anxious))
			{
				return false;
			}
			return true;
		}
	}
}
