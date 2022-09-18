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
	[HarmonyPatch(typeof(Pawn_DraftController), "Drafted", MethodType.Setter)]
	public static class Drafted_Patch
	{
		private static bool Prefix(Pawn_DraftController __instance, ref bool value)
		{
            if ((__instance.pawn?.jobs?.curDriver?.asleep ?? false) && __instance.pawn.HasTrait(VTEDefOf.VTE_HeavySleeper))
            {
				value = false;
				Messages.Message("VTE.CantBeWokenUpHeavySleeper".Translate(__instance.pawn.Named("PAWN")), __instance.pawn, MessageTypeDefOf.NeutralEvent, historical: false);
				return false;
			}
			return true;
		}
	}
}
