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

	[HarmonyPatch(typeof(Pawn_MeleeVerbs), "TryMeleeAttack")]
	public static class TryMeleeAttack_Patch
	{
		public static void Postfix(bool __result, Pawn_MeleeVerbs __instance, Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (__result && __instance.Pawn.HasTrait(VTEDefOf.VTE_MartialArtist) && target is Pawn victim && (!victim.RaceProps?.IsMechanoid ?? false))
            {
				if (victim.equipment?.Primary != null && victim.Position.DistanceTo(__instance.Pawn.Position) <= 1f && Rand.Chance(0.5f))
                {
					victim.equipment.TryDropEquipment(victim.equipment.Primary, out ThingWithComps resultingEq, victim.Position);
					Messages.Message("VTE.VictimDropsEquipmentMartialArtist".Translate(victim.Named("VICTIM"), __instance.Pawn.Named("PAWN")), victim, MessageTypeDefOf.NeutralEvent, historical: false);
				}
			}
		}
	}

}
