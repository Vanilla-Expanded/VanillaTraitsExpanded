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
				foreach (var pawn in TraitsManager.Instance.snobs)
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
					Log.Message("Faction.OfPlayer gets new relationship change to Empire due to " + snobCount + " snobs in the faction. Old value: " + goodwillChange + " - new value: " + newGoodWillChange);
					goodwillChange = newGoodWillChange;
				}
				else if (other == Faction.OfPlayer && __instance == Faction.OfEmpire)
				{
					var snobCount = SnobCount();
					var newGoodWillChange = (int)(goodwillChange * (1 + (SnobCount() / 10f)));
					Log.Message("Faction.OfPlayer gets new relationship change to Empire due to " + snobCount + " snobs in the faction. Old value: " + goodwillChange + " - new value: " + newGoodWillChange);
					goodwillChange = newGoodWillChange;
				}
			}
		}
	}
}
