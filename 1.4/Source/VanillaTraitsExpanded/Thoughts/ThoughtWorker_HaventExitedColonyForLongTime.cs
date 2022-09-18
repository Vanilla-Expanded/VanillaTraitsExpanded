using RimWorld;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_HaventExitedColonyForLongTime : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Wanderlust))
            {
				if (TraitsManager.Instance.wanderLustersWithLastMapExitedTick.ContainsKey(p))
                {
					var lastTick = TraitsManager.Instance.wanderLustersWithLastMapExitedTick[p];
					if ((GenTicks.TicksAbs - lastTick) > 10 * GenDate.TicksPerDay)
                    {
						return ThoughtState.ActiveDefault;
                    }
				}
				else
                {
					TraitsManager.Instance.wanderLustersWithLastMapExitedTick[p] = GenTicks.TicksAbs;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
