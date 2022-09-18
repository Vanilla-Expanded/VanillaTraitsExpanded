using RimWorld;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_NoDedicatedLab : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Prodigy))
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
				if (!HasLab(map))
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}

		public bool HasLab(Map map)
        {
			foreach (var room in map.regionGrid.allRooms)
            {
				if (room.Role == RoomRoleDefOf.Laboratory)
                {
					return true;
                }
            }
			return false;
		}
	}
}
