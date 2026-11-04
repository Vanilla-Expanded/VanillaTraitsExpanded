using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{

	[HarmonyPatch(typeof(Pawn_MeleeVerbs), "TryMeleeAttack")]
	public static class TryMeleeAttack_Patch
	{
		public static void Postfix(bool __result, Pawn_MeleeVerbs __instance, Thing target, Verb verbToUse = null, bool surpriseAttack = false)
		{
			if (__result && __instance.Pawn.HasTrait(VTEDefOf.VTE_MartialArtist) && target is Pawn victim && (!victim.RaceProps?.IsMechanoid ?? false))
			{
				// Max distance of 1 allows for cardinal direction, 1.5 allows for ordinal ones as well
				const float maxDistance = 1.5f;
				const float maxDistanceSquared = maxDistance * maxDistance;

				if (victim.equipment?.Primary != null && victim.Position.DistanceToSquared(__instance.Pawn.Position) <= maxDistanceSquared && Rand.Chance(0.5f))
                {
					victim.equipment.TryDropEquipment(victim.equipment.Primary, out _, victim.Position);
					Messages.Message("VTE.VictimDropsEquipmentMartialArtist".Translate(victim.Named("VICTIM"), __instance.Pawn.Named("PAWN")), victim, MessageTypeDefOf.NeutralEvent, historical: false);
				}
			}
		}
	}

}
