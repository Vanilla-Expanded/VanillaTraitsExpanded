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
	[HarmonyPatch(typeof(JobDriver_ViewArt))]
	[HarmonyPatch("WaitTickAction")]
	public static class WaitTickAction_Patch
	{
		public static void Postfix(JobDriver_ViewArt __instance)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Snob))
            {
				var ArtThing = __instance.pawn.CurJob.targetA.Thing;
				float num = (ArtThing.GetStatValue(StatDefOf.Beauty) / ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty)) * 2f;
				float extraJoyGainFactor = (num > 0f) ? num : 0f;
				__instance.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(__instance.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)ArtThing);
			}
		}
	}

	[HarmonyPatch(typeof(JobDriver_WatchBuilding))]
	[HarmonyPatch("MakeNewToils")]
	public class MakeNewToils_Patch
	{
		private static void Postfix(JobDriver_WatchBuilding __instance, ref IEnumerable<Toil> __result)
		{
			var list = __result.ToList();
			Toil t = new Toil
			{
				initAction = delegate ()
				{
					if (__instance.pawn.HasTrait(VTEDefOf.VTE_CouchPotato) && __instance.job.def == VTEDefOf.WatchTelevision)
                    {
						__instance.pawn.TryGiveThought(VTEDefOf.VTE_WatchedTelevisor);
                    }
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			list.Add(t);
			__result = list;
		}
	}

	//[HarmonyPatch(typeof(Pawn_JobTracker), "EndCurrentJob")]
	//public class EndCurrentJobPatch3
	//{
	//	private static bool Prefix(Pawn_JobTracker __instance, Pawn ___pawn, JobCondition condition, ref bool startNewJob, bool canReturnToPool = true)
	//	{
	//		if ((condition == JobCondition.Incompletable || condition == JobCondition.InterruptForced) 
	//			&& ___pawn.CurJobDef == JobDefOf.LayDown && ___pawn.HasTrait(VTEDefOf.VTE_HeavySleeper) && !___pawn.Downed)
	//		{
	//			return false;
	//		}
	//		return true;
	//	}
	//}
}
