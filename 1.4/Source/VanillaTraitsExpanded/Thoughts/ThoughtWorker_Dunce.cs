using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_Dunce : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Dunce))
            {
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
