using Verse;

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
