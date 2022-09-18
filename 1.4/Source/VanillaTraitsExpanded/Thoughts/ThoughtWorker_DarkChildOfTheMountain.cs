using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_DarkChildOfTheMountain : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.HasTrait(VTEDefOf.VTE_ChildOfMountain) && p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
