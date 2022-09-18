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
	[HarmonyPatch(typeof(InteractionWorker_Breakup), "RandomSelectionWeight")]
	public static class RandomSelectionWeight_Patch
	{
		public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
		{
			if (initiator.HasTrait(VTEDefOf.VTE_Insatiable))
            {
				__result *= 4f;
            }
			else if (initiator.HasTrait(VTEDefOf.VTE_Prude))
			{
				__result = 0f;
			}
		}
	}

	[HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
	public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight_Patch
	{
		public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
		{
			if (initiator.HasTrait(VTEDefOf.VTE_Insatiable))
			{
				__result *= 4f;
			}
		}
	}

	[HarmonyPatch(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin")]
	public static class GenerateRandomMinTicksToNextLovin_Patch
	{
		public static void Postfix(ref int __result, JobDriver_Lovin __instance)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Insatiable))
			{
				__result = (int)(__result / 4f);
			}
		}
	}

	[HarmonyPatch(typeof(JobGiver_DoLovin), "TryGiveJob")]
	public static class JobGiver_DoLovin_TryGiveJob_Patch
	{
		public static bool Prefix(Pawn pawn)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Prude) && !pawn.GetSpouses(false).Any())
			{
				return false;
			}
			return true;
		}
	}


	[HarmonyPatch(typeof(RelationsUtility), "TryDevelopBondRelation")]
	public static class TryDevelopBondRelation_Patch
	{
		public static List<string> cats = new List<string>
		{
			"Cat",
			"AEXP_CatSphynx",
			"AEXP_CatSomali",
			"AEXP_CatSiamese",
			"AEXP_CatPersian",
			"AEXP_CatNorwegianForest",
			"AEXP_CatMunchkin",
			"AEXP_CatMaineCoon",
			"AEXP_CatBritishShorthair",
			"AEXP_CatBengal",
			"AEXP_CatAbyssinian",
		};

		public static List<string> dogs = new List<string>
		{

			"YorkshireTerrier",
			"LabradorRetriever",
			"Husky",
			"AEXP_WelshTerrier",
			"AEXP_Shih-Tzu",
			"AEXP_Rottweiler",
			"AEXP_Pug",
			"AEXP_Poodle",
			"AEXP_GreatDane",
			"AEXP_GermanShepherd",
			"AEXP_FrenchBulldog",
			"AEXP_Corgi",
			"AEXP_Chihuahua",
			"AEXP_Beagle",
		};
		public static bool Prefix(Pawn humanlike, Pawn animal, ref float baseChance)
		{
			if (humanlike.HasTrait(VTEDefOf.VTE_CatPerson))
			{
				if (dogs.Contains(animal.def.defName))
                {
					baseChance = 0f;
					return false;
                }
				else if (cats.Contains(animal.def.defName))
                {
					baseChance *= 4f;
                }
			}
			else if (humanlike.HasTrait(VTEDefOf.VTE_DogPerson))
			{
				if (cats.Contains(animal.def.defName))
				{
					baseChance = 0f;
					return false;
				}
				else if (dogs.Contains(animal.def.defName))
				{
					baseChance *= 4f;
				}
			}
			return true;
		}
	}
}
