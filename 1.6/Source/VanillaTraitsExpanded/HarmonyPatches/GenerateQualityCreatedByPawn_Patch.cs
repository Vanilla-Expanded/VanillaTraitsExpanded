using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using Verse;


namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(QualityUtility))]
	[HarmonyPatch("GenerateQualityCreatedByPawn")]
	[HarmonyPatch(new Type[]
		{
			typeof(Pawn),
			typeof(SkillDef),

            typeof(bool)
        })]
	[HarmonyPriority(Priority.First)]
	public static class GenerateQualityCreatedByPawn_Patch
	{
		private static void Prefix(Pawn pawn, out bool __state)
        {
			if (pawn.InspirationDef == InspirationDefOf.Inspired_Creativity)
            {
				__state = true;
			}
			else
            {
				__state = false;
            }
        }
		private static void Postfix(ref QualityCategory __result, Pawn pawn, SkillDef relevantSkill, bool __state)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Perfectionist))
            {
				if (__result != QualityCategory.Legendary)
                {
					var newResult = (QualityCategory)((int)__result + 1);
					__result = newResult;
				}
				else
				{
					// Allow legendary items if the current quality
					// was already legendary, even without inspiration.
					__state = true;
				}
				if (__result == QualityCategory.Normal || __result == QualityCategory.Awful || __result == QualityCategory.Poor)
				{
					pawn.TryGiveThought(VTEDefOf.VTE_CreatedLowQualityItem);
				}
				if (__result == QualityCategory.Legendary && !__state)
                {
					if (ModsConfig.IdeologyActive)
                    {
						var effect = pawn.Ideo.GetRole(pawn)?.def.roleEffects.OfType<RoleEffect_ProductionQualityOffset>().FirstOrDefault();
						if (effect != null && effect.offset > 0)
                        {
							return; // we allow legendary for any roles boosting production quality
                        }
                    }
					__result = QualityCategory.Masterwork;
				}
			}
		}
	}
}
