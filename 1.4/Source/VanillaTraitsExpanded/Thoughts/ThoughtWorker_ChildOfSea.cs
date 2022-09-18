using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MapCheck
    {
		public int lastTickCheck;
		public bool value;
    }
	public class ThoughtWorker_ChildOfSea : ThoughtWorker
	{
		public static HashSet<TerrainDef> SeaTerrain = new HashSet<TerrainDef>()
		{
			TerrainDefOf.WaterMovingChestDeep,
			TerrainDefOf.WaterMovingShallow,
			TerrainDefOf.WaterOceanDeep,
			TerrainDefOf.WaterOceanShallow,
		};

		public Dictionary<Map, MapCheck> mapChecks = new Dictionary<Map, MapCheck>();
        public bool HasEnoughSeaCheck(Map map)
        {
            if (mapChecks.TryGetValue(map, out MapCheck check))
            {
                if (GenTicks.TicksAbs < check.lastTickCheck + 2000)
                {
                    return check.value;
                }
            }

            bool value = HasEnoughSea(map); // every 2000 ticks
			mapChecks[map] = new MapCheck
            {
                lastTickCheck = GenTicks.TicksAbs,
                value = value
            };
            return value;
        }

		public bool HasEnoughSea(Map map)
        {
			int num = 0;
			foreach (var terrain in map.terrainGrid.topGrid)
            {
				if (SeaTerrain.Contains(terrain))
                {
					num++;
                }
				if (num >= 40)
                {
					return true;
                }
            }
			return false;
        }
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_ChildOfSea))
            {
				if (p.Map != null && HasEnoughSeaCheck(p.Map))
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
