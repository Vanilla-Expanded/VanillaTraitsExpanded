using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class Thought_BondedAnimalMasterHater : Thought_Situational
	{
		private const int MaxAnimals = 3;
		protected override float BaseMoodOffset => base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMasterHater)def.Worker).GetAnimalsCount(pawn), 3);
	}
}
