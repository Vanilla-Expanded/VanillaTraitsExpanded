using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Faction), "TryAffectGoodwillWith")]
	public static class TryAffectGoodwillWith_Patch
	{
		public static int SnobCount()
        {
			int num = 0;
			if (TraitsManager.Instance?.snobs != null)
            {
				foreach (var pawn in TraitsManager.Instance.snobs.TryGetPawns(VTEDefOf.VTE_Snob))
				{
					if (pawn != null)
                    {
						if (pawn.Spawned && !pawn.Dead)
						{
							num++;
						}
					}

				}
			}
			return num;
        }
		public static void Prefix(Faction __instance, Faction other, ref int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (goodwillChange > 0)
            {
				if (__instance == Faction.OfPlayer && other == Faction.OfEmpire)
				{
					var snobCount = SnobCount();
					var newGoodWillChange = (int)(goodwillChange * (1 + (SnobCount() / 10f)));
					goodwillChange = newGoodWillChange;
				}
				else if (other == Faction.OfPlayer && __instance == Faction.OfEmpire)
				{
					var snobCount = SnobCount();
					var newGoodWillChange = (int)(goodwillChange * (1 + (SnobCount() / 10f)));
					goodwillChange = newGoodWillChange;
				}
			}
		}
	}
}
