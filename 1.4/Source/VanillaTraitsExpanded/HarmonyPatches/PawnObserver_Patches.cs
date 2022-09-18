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
	[HarmonyPatch(typeof(PawnObserver), "ObserveSurroundingThings")]
	public static class ObserveSurroundingThings_Patch
	{
		public static void Postfix(Pawn ___pawn)
        {
			if (!___pawn.HasTrait(VTEDefOf.VTE_Squeamish) && !___pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) || ___pawn.needs.mood == null)
			{
				return;
			}
			Map map = ___pawn.Map;
			for (int i = 0; (float)i < 100f; i++)
			{
				IntVec3 intVec = ___pawn.Position + GenRadial.RadialPattern[i];
				if (!intVec.InBounds(map) || !GenSight.LineOfSight(intVec, ___pawn.Position, map, skipFirstCell: true))
				{
					continue;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				int num = 0;
				for (int j = 0; j < thingList.Count; j++)
				{
					if (thingList[j].def == ThingDefOf.Filth_Blood)
                    {
						num++;
                    }
				}
				if (num > 4)
                {
					___pawn.TryGiveThought(VTEDefOf.VTE_ObservedManyBlood);
					var comp = Current.Game.GetComponent<TraitsManager>();
					if ((!comp.squeamishWithLastVomitedTick.ContainsKey(___pawn) || GenTicks.TicksAbs >= comp.squeamishWithLastVomitedTick[___pawn] + (30 * 60)) && Rand.Chance(0.5f))
					{
						Job vomit = JobMaker.MakeJob(JobDefOf.Vomit);
						___pawn.jobs.TryTakeOrderedJob(vomit);
						comp.squeamishWithLastVomitedTick[___pawn] = GenTicks.TicksAbs;
					}
                }
			}
		}
	}
}
