﻿using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_NoDedicatedLab : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Prodigy))
            {
				Map map;
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
			foreach (var room in map.regionGrid.AllRooms)
            {
				if (room.Role == VTEDefOf.Laboratory)
                {
					return true;
                }
            }
			return false;
		}
	}
}
