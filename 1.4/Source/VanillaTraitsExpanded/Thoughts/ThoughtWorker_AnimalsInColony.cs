using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MapPawns
	{
		public MapPawns(List<Pawn> pawns)
		{
			this.pawns = pawns;
			this.lastTickCheck = GenTicks.TicksAbs;
		}
		public List<Pawn> pawns;
		public int lastTickCheck;
	}
	public class ThoughtWorker_AnimalsInColony : ThoughtWorker
	{
		public static Dictionary<Map, MapPawns> mapPawns = new Dictionary<Map, MapPawns>();
		public static List<Pawn> GetAllAnimals(Map map, Faction faction)
		{
			if (mapPawns.TryGetValue(map, out MapPawns mapPawns2))
			{
				if (GenTicks.TicksAbs > mapPawns2.lastTickCheck + 60)
				{
					mapPawns2.pawns = map.mapPawns.AllPawns.Where(x => x.RaceProps.Animal && x.Faction == faction).ToList();
					mapPawns2.lastTickCheck = GenTicks.TicksAbs;
				}
				return mapPawns2.pawns;
			}
			else
			{
				var pawns = map.mapPawns.AllPawns.Where(x => x.RaceProps.Animal && x.Faction == faction).ToList();
				mapPawns[map] = new MapPawns(pawns);
				return pawns;
			}
		}
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Menagerist))
            {
				Map map = null;
				if (p.ownership?.OwnedBed?.Map != null)
				{
					map = p.ownership.OwnedBed.Map;
				}
				else
				{
					map = p.Map;
				}
				if (map != null)
				{
					var animalCount = GetAllAnimals(map, p.Faction).Count();
					if (animalCount <= 15)
					{
						return ThoughtState.ActiveAtStage(animalCount);
					}
					else
					{
						return ThoughtState.ActiveAtStage(15);
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
