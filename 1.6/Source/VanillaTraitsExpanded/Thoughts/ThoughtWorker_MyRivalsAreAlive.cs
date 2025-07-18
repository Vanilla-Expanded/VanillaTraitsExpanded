﻿using RimWorld;
using Verse;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_MyRivalsAreAlive : ThoughtWorker
	{
		public bool RivalsAreAlive(Pawn p)
        {
			foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
            {
				if (pawn.RaceProps.IsFlesh && !pawn.Dead && !pawn.Destroyed)
				{
					int num = p.relations.OpinionOf(pawn);
					if (num < -20)
					{
						return true;
					}
				}
			}
			return false;
        }
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Vengeful) && RivalsAreAlive(p))
            {
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
