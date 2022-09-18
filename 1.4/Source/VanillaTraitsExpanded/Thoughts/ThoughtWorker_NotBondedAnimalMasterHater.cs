using RimWorld;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_NotBondedAnimalMasterHater : ThoughtWorker_BondedAnimalMasterHater
	{
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p;
		}
	}
}
