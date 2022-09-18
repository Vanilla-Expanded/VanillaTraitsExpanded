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
	[HarmonyPatch(typeof(Pawn_FilthTracker))]
	[HarmonyPatch("TryDropFilth")]

	public static class TryDropFilth_Patch
	{
		private static FilthSourceFlags AdditionalFilthSourceFlags (Pawn pawn)
		{
			if (pawn.Faction != null || !pawn.RaceProps.Animal)
			{
				return FilthSourceFlags.Unnatural;
			}
			return FilthSourceFlags.Natural;
		}
		private static bool Prefix(Pawn ___pawn, List<Filth> ___carriedFilth)
		{
			if (___pawn.HasTrait(VTEDefOf.VTE_Neat))
            {
				return false;
			}
			else if (___pawn.HasTrait(VTEDefOf.VTE_Slob))
            {
				if (___carriedFilth.Count > 0)
				{
					for (int num = ___carriedFilth.Count - 1; num >= 0; num--)
					{
						if (___carriedFilth[num].CanDropAt(___pawn.Position, ___pawn.Map))
						{
							FilthMaker.TryMakeFilth(___pawn.Position, ___pawn.Map, ___carriedFilth[num].def, ___carriedFilth[num].sources, AdditionalFilthSourceFlags(___pawn));
						}
					}
				}
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(Pawn_FilthTracker))]
	[HarmonyPatch("TryPickupFilth")]

	public static class TryPickupFilth_Patch
	{
		private static bool Prefix(Pawn ___pawn)
		{
			if (___pawn.HasTrait(VTEDefOf.VTE_Neat))
			{
				return false;
			}
			return true;
		}
	}
}
