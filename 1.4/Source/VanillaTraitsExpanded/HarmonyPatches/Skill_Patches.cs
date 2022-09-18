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
	[HarmonyPatch(typeof(SkillRecord), "Interval")]
	public static class Interval_Patch
	{
		public static bool Prefix(SkillRecord __instance, Pawn ___pawn)
		{
			if (___pawn.HasTrait(VTEDefOf.VTE_Prodigy))
            {
				float num = 0.5f;
				switch (__instance.levelInt)
				{
					case 10:
						__instance.Learn(-0.1f * num);
						break;
					case 11:
						__instance.Learn(-0.2f * num);
						break;
					case 12:
						__instance.Learn(-0.4f * num);
						break;
					case 13:
						__instance.Learn(-0.6f * num);
						break;
					case 14:
						__instance.Learn(-1f * num);
						break;
					case 15:
						__instance.Learn(-1.8f * num);
						break;
					case 16:
						__instance.Learn(-2.8f * num);
						break;
					case 17:
						__instance.Learn(-4f * num);
						break;
					case 18:
						__instance.Learn(-6f * num);
						break;
					case 19:
						__instance.Learn(-8f * num);
						break;
					case 20:
						__instance.Learn(-12f * num);
						break;
				}
				return false;
			}
			return true;
		}
	}

}
