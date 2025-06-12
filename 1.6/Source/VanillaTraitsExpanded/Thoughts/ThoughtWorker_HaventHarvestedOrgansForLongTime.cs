using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_HaventHarvestedOrgansForLongTime : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
				if (TraitsManager.Instance.madSurgeonsWithLastHarvestedTick.TryGetPawns(VTEDefOf.VTE_MadSurgeon)
					.TryGetValue(p, out var lastTick))
                {
					if (p.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(VTEDefOf.VTE_HarvestedOrgans) == null)
                    {
						if (GenTicks.TicksAbs - lastTick > 10 * GenDate.TicksPerDay)
						{
							return ThoughtState.ActiveDefault;
						}
					}
				}
				else
                {
					TraitsManager.Instance.madSurgeonsWithLastHarvestedTick[p] = GenTicks.TicksAbs;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
