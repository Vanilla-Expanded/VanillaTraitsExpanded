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
	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch("TryTakeOrderedJob")]
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
			if (!jobsToExclude.Contains(job.def))
            {
				if (___pawn.HasTrait(VTEDefOf.VTE_AbsentMinded))
				{
					TraitUtils.TraitsManager.forcedJobs[___pawn] = job;
				}
				if (___pawn.HasTrait(VTEDefOf.VTE_Rebel))
				{
					var slowWorkHediff = HediffMaker.MakeHediff(VTEDefOf.VTE_SlowWorkSpeed, ___pawn);
					___pawn.health.AddHediff(slowWorkHediff);
				}
				if (___pawn.HasTrait(VTEDefOf.VTE_Submissive))
				{
					var slowWorkHediff = ___pawn.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowWorkSpeed);
					if (slowWorkHediff != null)
                    {
						___pawn.health.RemoveHediff(slowWorkHediff);
                    }
				}
			}
		}
	}

	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch("StartJob")]
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
    [HarmonyPatch(typeof(Pawn_JobTracker), "EndCurrentJob")]
    public static class EndCurrentJobPatch
    {
        private static void Prefix(Pawn ___pawn)
        {
			if (___pawn.HasTrait(VTEDefOf.VTE_Rebel))
            {
				var rebelHediff = ___pawn.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowWorkSpeed);
				if (rebelHediff != null)
                {
					___pawn.health.RemoveHediff(rebelHediff);
				}
			}

			if (___pawn.HasTrait(VTEDefOf.VTE_Submissive))
			{
				if (___pawn.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowWorkSpeed) == null)
                {
					var slowWorkHediff = HediffMaker.MakeHediff(VTEDefOf.VTE_SlowWorkSpeed, ___pawn);
					___pawn.health.AddHediff(slowWorkHediff);
                }
			}
		}
    }

    [HarmonyPatch(typeof(FoodUtility))]
	[HarmonyPatch("AddFoodPoisoningHediff")]
	public static class AddFoodPoisoningHediff_Patch
	{
		private static bool Prefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_IronStomach))
			{
				return false;
			}
			return true;
		}
	}
}
