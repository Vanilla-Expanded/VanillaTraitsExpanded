using RimWorld;
using UnityEngine;

namespace VanillaTraitsExpanded
{
	public class Thought_BondedAnimalMasterHater : Thought_Situational
	{
		private const int MaxAnimals = 3;
		protected override float BaseMoodOffset => base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMasterHater)def.Worker).GetAnimalsCount(pawn), 3);
	}
}
