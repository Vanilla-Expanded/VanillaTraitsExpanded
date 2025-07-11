﻿using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalStateWorker_Kleptomaniac : MentalStateWorker
	{
		public override bool StateCanOccur(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && Rand.Chance(0.5f) && pawn.HasTrait(VTEDefOf.VTE_Kleptomaniac) && pawn.Map.mapPawns.AllPawns.Any
				(x => !x.Dead && x.Spawned && x.Position.IsValid && x.RaceProps.Humanlike && x.Faction != pawn.Faction && !x.HostileTo(pawn));
		}
	}
}
