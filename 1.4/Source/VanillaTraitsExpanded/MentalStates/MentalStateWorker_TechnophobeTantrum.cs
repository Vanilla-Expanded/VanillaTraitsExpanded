using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalStateWorker_TechnophobeTantrum : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!pawn.HasTrait(VTEDefOf.VTE_Technophobe))
            {
				return false;
            }
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, tmpThings, GetCustomValidator());
			bool result = tmpThings.Any();
			tmpThings.Clear();
			return result;
		}

		public Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => x.def.techLevel > RimWorld.TechLevel.Medieval;
		}
	}
}
