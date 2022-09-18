using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_ChildOfMountain : ThoughtWorker
	{
        public Dictionary<Map, MapCheck> mapChecks = new Dictionary<Map, MapCheck>();
        public bool HasEnoughMountainCheck(Map map)
        {
            if (mapChecks.TryGetValue(map, out MapCheck check))
            {
                if (GenTicks.TicksAbs < check.lastTickCheck + 2000)
                {
                    return check.value;
                }
            }

            bool value = HasEnoughMountain(map); // every 2000 ticks
            mapChecks[map] = new MapCheck
            {
                lastTickCheck = GenTicks.TicksAbs,
                value = value
            };
            return value;
        }
        public bool HasEnoughMountain(Map map)
        {
			int num = 0;
			foreach (var cell in map.AllCells)
            {
				if (map.roofGrid.RoofAt(cell) == RoofDefOf.RoofRockThick)
                {
					num++;
                }
				if (num >= 100)
                {
					return true;
                }
            }
			return false;
        }
        protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_ChildOfMountain))
            {
				if (p.Map != null && HasEnoughMountainCheck(p.Map))
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
