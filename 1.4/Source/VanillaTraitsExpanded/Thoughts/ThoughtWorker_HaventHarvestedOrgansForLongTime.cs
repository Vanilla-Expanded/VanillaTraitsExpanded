using RimWorld;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_HaventHarvestedOrgansForLongTime : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
				if (TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick.ContainsKey(p))
                {
					if (p.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(VTEDefOf.VTE_HarvestedOrgans) == null)
                    {
						var lastTick = TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick[p];
						if (GenTicks.TicksAbs - lastTick > 10 * GenDate.TicksPerDay)
						{
							return ThoughtState.ActiveDefault;
						}
					}
				}
				else
                {
					TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick[p] = GenTicks.TicksAbs;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
