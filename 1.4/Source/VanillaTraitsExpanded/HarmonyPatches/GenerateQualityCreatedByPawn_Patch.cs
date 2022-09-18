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
	[HarmonyPatch(typeof(QualityUtility))]
	[HarmonyPatch("GenerateQualityCreatedByPawn")]
	[HarmonyPatch(new Type[]
		{
			typeof(Pawn),
			typeof(SkillDef)
		}, new ArgumentType[]
		{
			0,
			0
		})]
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
				if (__result == QualityCategory.Normal || __result == QualityCategory.Awful || __result == QualityCategory.Poor)
				{
					pawn.TryGiveThought(VTEDefOf.VTE_CreatedLowQualityItem);
				}
				if (__result == QualityCategory.Legendary && !__state)
                {
					if (ModsConfig.IdeologyActive)
                    {
						var role = pawn.Ideo.GetRole(pawn);
						if (role != null && role.def.defName == "IdeoRole_ProductionSpecialist")
                        {
							return; // we allow legendary for production specialist
                        }
                    }
					__result = QualityCategory.Masterwork;
				}
			}
		}
	}
}
