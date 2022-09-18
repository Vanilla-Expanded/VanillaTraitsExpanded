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
				if (TraitUtils.TraitsManager.wanderLustersWithLastMapExitedTick.ContainsKey(p))
                {
					var lastTick = TraitUtils.TraitsManager.wanderLustersWithLastMapExitedTick[p];
					if ((GenTicks.TicksAbs - lastTick) > 10 * GenDate.TicksPerDay)
                    {
						return ThoughtState.ActiveDefault;
                    }
				}
				else
                {
					TraitUtils.TraitsManager.wanderLustersWithLastMapExitedTick[p] = GenTicks.TicksAbs;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
