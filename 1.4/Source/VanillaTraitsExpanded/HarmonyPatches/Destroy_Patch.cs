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
