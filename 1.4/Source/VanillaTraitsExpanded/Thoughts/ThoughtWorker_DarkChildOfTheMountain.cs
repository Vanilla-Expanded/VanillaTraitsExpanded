using RimWorld;
using Verse;

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
