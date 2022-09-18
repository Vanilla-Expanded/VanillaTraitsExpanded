using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "TryGiveThoughts", new Type[]
	{
		typeof(Pawn),
		typeof(DamageInfo?),
		typeof(PawnDiedOrDownedThoughtsKind)
	})]
	public static class TryGiveThoughts_Patch
	{
		public static void Postfix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			if (victim.RaceProps.IsMechanoid && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.MapHeld != null)
			{
				foreach (var pawn in victim.MapHeld.mapPawns.AllPawns)
				{
					if (pawn.HasTrait(VTEDefOf.VTE_Technophobe))
					{
						pawn.TryGiveThought(VTEDefOf.VTE_MechanoidIsKilled);
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(Bill), "PawnAllowedToStartAnew", new Type[]
	{
		typeof(Pawn)
	})]
	public static class PawnAllowedToStartAnew_Patch
	{
		public static bool Prefix(Pawn p, RecipeDef ___recipe)
		{
			if (p.HasTrait(VTEDefOf.VTE_AnimalLover) && ___recipe == DefDatabase<RecipeDef>.GetNamed("ButcherCorpseFlesh"))
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(IndividualThoughtToAdd), "Add")]
	public static class Add_Patch
	{
		public static void Postfix(IndividualThoughtToAdd __instance, Pawn ___otherPawn)
		{
			//Log.Message("IndividualThoughtToAdd: " + __instance.addTo + " gets " + __instance.thought);
			if (__instance.thought is Thought_MemorySocial thought_MemorySocial && (__instance.addTo.HasTrait(VTEDefOf.VTE_WorldWeary) 
				|| ___otherPawn != null && ___otherPawn.HasTrait(VTEDefOf.VTE_WorldWeary)))
			{
				thought_MemorySocial.opinionOffset /= 2f;
			}
		}
	}

	[HarmonyPatch(typeof(Thought), "MoodOffset")]
	public static class MoodOffset_Patch
	{
		public static void Postfix(Thought __instance, ref float __result)
		{
			if (__instance.def == ThoughtDefOf.Naked && __instance.pawn.HasTrait(VTEDefOf.VTE_Prude))
			{
				__result *= 2f;
			}
			if (__instance.def == VTEDefOf.SmokeleafHigh && __instance.pawn.HasTrait(VTEDefOf.VTE_Stoner))
            {
				__result *= 2f;
            }
			if (__instance.def == VTEDefOf.Inebriated && __instance.pawn.HasTrait(VTEDefOf.VTE_Lush))
            {
				__result *= 2f;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_RefinedPalate))
			{
				if (__instance.def == ThoughtDefOf.AteFineMeal || __instance.def.defName == "VCE_AteFineDessert")
				{
					__result = 0;
				}
				else if (__instance.def == ThoughtDefOf.AteLavishMeal || __instance.def.defName == "VCE_AteGourmetMeal" || __instance.def.defName == "VCE_AteLavishDessert")
				{
					__result *= 1.5f;
				}
			}
			if (TryGainMemory_Patch.animalThoughtDefs.Contains(__instance.def) && __instance.pawn.HasTrait(VTEDefOf.VTE_AnimalLover))
			{
				__result *= 2f;
			}
			if (__instance.def == ThoughtDefOf.KilledMyRival && __instance.pawn.HasTrait(VTEDefOf.VTE_Vengeful))
			{
				__result *= 2f;
			}
		}
	}

	//[HarmonyPatch(typeof(SituationalThoughtHandler), "TryCreateThought")]
	//public static class TryCreateThought_Patch
	//{
	//	public static void Postfix(SituationalThoughtHandler __instance, Thought_Situational __result, ThoughtDef def)
	//	{
	//		if (__result != null)
	//		{
	//			Log.Message("TryCreateThought: " + __instance.pawn + " gets " + __result);
	//		}
	//	}
	//}

	//[HarmonyPatch(typeof(SituationalThoughtHandler), "TryCreateSocialThought")]
	//public static class TryCreateSocialThought_Patch
	//{
	//	public static void Postfix(SituationalThoughtHandler __instance, Thought_SituationalSocial __result, ThoughtDef def, Pawn otherPawn)
	//	{
	//		if (__result != null)
	//		{
	//			Log.Message("TryCreateSocialThought: " + __instance.pawn + " gets " + __result);
	//		}
	//	}
	//}


	[HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[]
	{
		typeof(Thought_Memory),
		typeof(Pawn)
	})]
	public static class TryGainMemory_Patch
	{
		public static HashSet<ThoughtDef> animalThoughtDefs = new HashSet<ThoughtDef>
		{
			ThoughtDefOf.BondedAnimalBanished,
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalDied"),
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalLost"),
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalMaster"),
			DefDatabase<ThoughtDef>.GetNamed("KilledColonyAnimal"),
			DefDatabase<ThoughtDef>.GetNamed("KilledMyBondedAnimal"),
			DefDatabase<ThoughtDef>.GetNamed("NotBondedAnimalMaster"),
			DefDatabase<ThoughtDef>.GetNamed("SoldMyBondedAnimal"),
			ThoughtDefOf.SoldMyBondedAnimalMood,
			ThoughtDefOf.Nuzzled
		};

		public static Dictionary<ThoughtDef, ThoughtDef> inverseAnimalThoughDefs = new Dictionary<ThoughtDef, ThoughtDef>
		{
			{ThoughtDefOf.BondedAnimalBanished, VTEDefOf.VTE_BondedAnimalBanishedHater },
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalDied"), VTEDefOf.VTE_BondedAnimalDiedHater},
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalLost"), VTEDefOf.VTE_BondedAnimalLostHater },
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalMaster"), VTEDefOf.VTE_BondedAnimalMasterHater },
			{DefDatabase<ThoughtDef>.GetNamed("KilledColonyAnimal"), VTEDefOf.VTE_KilledColonyAnimalHater },
			{DefDatabase<ThoughtDef>.GetNamed("KilledMyBondedAnimal"), VTEDefOf.VTE_KilledMyBondedAnimalHater },
			{DefDatabase<ThoughtDef>.GetNamed("NotBondedAnimalMaster"), VTEDefOf.VTE_NotBondedAnimalMasterHater },
			{DefDatabase<ThoughtDef>.GetNamed("SoldMyBondedAnimal"), VTEDefOf.VTE_SoldMyBondedAnimalHater },
			{ThoughtDefOf.SoldMyBondedAnimalMood, VTEDefOf.VTE_SoldMyBondedAnimalMoodHater },
			{ThoughtDefOf.Nuzzled, VTEDefOf.VTE_NuzzledHater}
		};
		private static bool Prefix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, Pawn otherPawn)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_AnimalHater) && animalThoughtDefs.Contains(newThought.def))
			{
				newThought = (Thought_Memory)ThoughtMaker.MakeThought(inverseAnimalThoughDefs[newThought.def]);
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Squeamish) && newThought.def == VTEDefOf.ObservedLayingRottingCorpse)
			{
				var comp = Current.Game.GetComponent<TraitsManager>();
				if ((!comp.squeamishWithLastVomitedTick.ContainsKey(__instance.pawn) || GenTicks.TicksAbs >= comp.squeamishWithLastVomitedTick[__instance.pawn] + (30 * 60)) && Rand.Chance(0.5f))
				{
					Job vomit = JobMaker.MakeJob(JobDefOf.Vomit);
					__instance.pawn.jobs.TryTakeOrderedJob(vomit);
					comp.squeamishWithLastVomitedTick[__instance.pawn] = GenTicks.TicksAbs;
				}
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Desensitized) && horribleThoughts.Contains(newThought.def.defName))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ColdInclined) && 
				(newThought.CurStageIndex < 1 && newThought.def == ThoughtDef.Named("EnvironmentCold")
				|| newThought.def == ThoughtDefOf.SleptInCold))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_HeatInclined) && 
				(newThought.CurStageIndex < 1 && newThought.def == ThoughtDef.Named("EnvironmentHot")
				|| newThought.def == ThoughtDefOf.SleptInHeat))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ChildOfMountain) && newThought.def == ThoughtDef.Named("EnvironmentDark"))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ChildOfSea) && newThought.def == ThoughtDef.Named("SoakingWet"))
			{
				__instance.pawn.TryGiveThought(VTEDefOf.VTE_SoakingWetChildOfTheSea);
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_HeavySleeper) && newThought.def == ThoughtDefOf.SleepDisturbed)
            {
				return false;
            }
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_MadSurgeon) && 
				(newThought.def == ThoughtDefOf.KnowColonistOrganHarvested 
				|| newThought.def == ThoughtDefOf.KnowGuestOrganHarvested 
				|| newThought.def == ThoughtDefOf.ButcheredHumanlikeCorpse
				|| newThought.def == ThoughtDefOf.KnowButcheredHumanlikeCorpse
				|| newThought.def == VTEDefOf.ObservedLayingCorpse
				|| newThought.def == VTEDefOf.ObservedLayingRottingCorpse
				|| newThought.def == ThoughtDefOf.KnowPrisonerDiedInnocent
				|| newThought.def == ThoughtDefOf.KnowColonistExecuted && newThought.CurStageIndex == 3
				))
			{
				return false;
			}
			return true;
		}

		public static void Postfix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, Pawn otherPawn)
        {
			if (newThought != null && __instance.pawn.HasTrait(VTEDefOf.VTE_FunLoving) && (newThought.def == ThoughtDefOf.AttendedParty
				||	newThought.def.defName == "VFEV_AttendedFeast" || newThought.def.defName == "VFEV_TakingPartInFeast")) // vikings compatibility
            {
				newThought.durationTicksOverride = newThought.def.DurationTicks * 4;
			}
		}

		public static HashSet<string> horribleThoughts = new HashSet<string>
		{
			"AteKibble",
			"AteCorpse",
			"AteHumanlikeMeatDirect",
			"AteHumanlikeMeatAsIngredient",
			"AteInsectMeatDirect",
			"AteInsectMeatAsIngredient",
			"AteRottenFood",
			"KnowGuestExecuted",
			"KnowColonistExecuted",
			"KnowPrisonerDiedInnocent",
			"KnowColonistDied",
			"BondedAnimalDied",
			"PawnWithGoodOpinionDied",
			"PawnWithBadOpinionDied",
			"MySonDied",
			"MyDaughterDied",
			"MyHusbandDied",
			"MyWifeDied",
			"MyFianceDied",
			"MyFianceeDied",
			"MyLoverDied",
			"MyBrotherDied",
			"MySisterDied",
			"MyGrandchildDied",
			"MyFatherDied",
			"MyMotherDied",
			"MyNieceDied",
			"MyNephewDied",
			"MyHalfSiblingDied",
			"MyAuntDied",
			"MyUncleDied",
			"MyGrandparentDied",
			"MyCousinDied",
			"MyKinDied",
			"ColonistLost",
			"BondedAnimalLost",
			"PawnWithGoodOpinionLost",
			"PawnWithBadOpinionLost",
			"MySonLost",
			"MyDaughterLost",
			"MyHusbandLost",
			"MyWifeLost",
			"MyFianceLost",
			"MyFianceeLost",
			"MyLoverLost",
			"MyBrotherLost",
			"MySisterLost",
			"MyGrandchildLost",
			"MyFatherLost",
			"MyMotherLost",
			"MyNieceLost",
			"MyNephewLost",
			"MyHalfSiblingLost",
			"MyAuntLost",
			"MyUncleLost",
			"MyGrandparentLost",
			"MyCousinLost",
			"MyKinLost",
			"KnowPrisonerSold",
			"KnowGuestOrganHarvested",
			"KnowColonistOrganHarvested",
			"MyOrganHarvested",
			"ObservedLayingCorpse",
			"ObservedLayingRottingCorpse",
			"WitnessedDeathAlly",
			"WitnessedDeathNonAlly",
			"WitnessedDeathFamily",
			"WitnessedDeathBloodlust",
			"KilledHumanlikeBloodlust",
			"ColonistBanished",
			"ColonistBanishedToDie",
			"PrisonerBanishedToDie",
			"BondedAnimalBanished",
			"FailedToRescueRelative",
			"KilledMyFriend",
			"KilledMyRival",
			"KilledMyLover",
			"KilledMyFiance",
			"KilledMySpouse",
			"KilledMyFather",
			"KilledMyMother",
			"KilledMySon",
			"KilledMyDaughter",
			"KilledMyBrother",
			"KilledMySister",
			"KilledMyKin",
			"KilledMyBondedAnimal",
			"SoldPrisoner",
			"ExecutedPrisoner",
			"KilledColonist",
			"KilledColonyAnimal",
			"OtherTravelerDied"
		};
	}
}
