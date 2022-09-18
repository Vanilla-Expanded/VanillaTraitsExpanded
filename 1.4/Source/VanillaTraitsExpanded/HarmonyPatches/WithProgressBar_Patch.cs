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
	[HarmonyPatch(typeof(ToilEffects))]
	[HarmonyPatch("WithProgressBar")]
	public static class WithProgressBar_Patch
	{
		private static void Postfix(this Toil __result)
		{
			__result.AddPreTickAction(delegate
			{
				if (__result.actor.HasTrait(VTEDefOf.VTE_Clumsy) && Rand.Chance(0.000005f))
                {
					BodyPartRecord partRecord = __result.actor.health.hediffSet.GetNotMissingParts().Where(x => x.depth == BodyPartDepth.Outside && x.coverageAbs > 0).RandomElement();
					var bruise = HediffMaker.MakeHediff(HediffDefOf.Bruise, __result.actor, partRecord);
					__result.actor.health.AddHediff(bruise);
					//Log.Message(__result.actor + " has a clumsy trait and is getting a bruise in " + partRecord + " this time while doing " + __result.actor.CurJobDef);
					Messages.Message("VTE.GotBruise".Translate(__result.actor.Named("PAWN")), __result.actor, MessageTypeDefOf.NeutralEvent, historical: false);
				}
				if (__result.actor.HasTrait(VTEDefOf.VTE_Perfectionist))
                {
					if (__result.actor.CurJobDef == JobDefOf.FinishFrame && Rand.Chance(0.0001f))
                    {
						//Log.Message(__result.actor + " has Perfectionist trait and randomly decises interrupt current construction job");
						Messages.Message("VTE.DecisesInterruptCurrentCostructionJob".Translate(__result.actor.Named("PAWN"), __result.actor.CurJob.GetTarget(TargetIndex.A).Thing.Label), __result.actor, MessageTypeDefOf.NeutralEvent, historical: false);
						TraitsManager.Instance.perfectionistsWithJobsToStop.Add(__result.actor);
					}
					else if ( __result.actor.CurJob?.bill?.recipe?.workAmount >= 2200 && Find.TickManager.TicksGame % GenDate.TicksPerHour * 3 == 0 && Rand.Chance(0.25f))
					{
						//Log.Message(p + " has Perfectionist trait and randomly decises interrupt current bill job");
						TraitsManager.Instance.perfectionistsWithJobsToStop.Add(__result.actor);
					}
				}
			});
		}
	}
}
