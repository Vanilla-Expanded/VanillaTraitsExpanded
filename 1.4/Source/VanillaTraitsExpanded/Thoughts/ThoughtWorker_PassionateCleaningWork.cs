using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_PassionateCleaningWork : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Neat) && p.CurJobDef == JobDefOf.Clean)
            {
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
