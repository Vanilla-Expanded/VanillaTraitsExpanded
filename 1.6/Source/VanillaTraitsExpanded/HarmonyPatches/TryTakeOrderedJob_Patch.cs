﻿using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch(nameof(Pawn_JobTracker.TryTakeOrderedJob))]
	public static class TryTakeOrderedJob_Patch
	{
		public static HashSet<JobDef> jobsToExclude = new HashSet<JobDef>
		{
			JobDefOf.Ingest,
			JobDefOf.Flee,
			JobDefOf.Vomit,
			JobDefOf.Wait_Combat,
			JobDefOf.BestowingCeremony,
			JobDefOf.LayDown,
			JobDefOf.Wait_Downed,
			JobDefOf.Deathrest,
			JobDefOf.BringBabyToSafety
		};

		private static bool Prefix(Pawn ___pawn, Job job)
        {
			if (___pawn.HasTrait(VTEDefOf.VTE_HeavySleeper) && ___pawn.CurJobDef == JobDefOf.LayDown)
            {
				return false;
            }
			return true;
        }
		private static void Postfix(Pawn ___pawn, Job job)
		{
			if (!jobsToExclude.Contains(job.def) && (job.workGiverDef is null || job.workGiverDef.emergency is false))
            {
				if (___pawn.HasTrait(VTEDefOf.VTE_AbsentMinded))
				{
					TraitsManager.Instance.forcedJobs[___pawn] = job;
				}
			}
		}
	}

	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch(nameof(Pawn_JobTracker.StartJob))]
	public static class StartJob_Patch
	{
		private static bool Prefix(Pawn ___pawn, Job newJob, JobCondition lastJobEndCondition)
		{
			if (newJob.def == JobDefOf.Vomit && ___pawn.HasTrait(VTEDefOf.VTE_IronStomach))
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(FoodUtility))]
	[HarmonyPatch(nameof(FoodUtility.AddFoodPoisoningHediff))]
	public static class AddFoodPoisoningHediff_Patch
	{
		private static bool Prefix(Pawn pawn)
		{
			return !pawn.HasTrait(VTEDefOf.VTE_IronStomach);
		}
	}

	[HarmonyPatch]
	public static class GiveNastyGutBug_Patch
	{
		private static bool Prepare()
		{
			var t = AccessTools.TypeByName("DubsBadHygiene.SanitationUtil");
			return t != null;
		}
		
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method(
				AccessTools.TypeByName("DubsBadHygiene.SanitationUtil"), "GiveNastyGutBug"
				);
		}
		
		private static bool Prefix(Pawn pawn)
		{
			return !pawn.HasTrait(VTEDefOf.VTE_IronStomach);
		}
	}
}
