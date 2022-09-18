using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalState_Kleptomaniac : MentalState
	{
        public override RandomSocialMode SocialModeMax()
        {
            return 0f;
        }
    }
}
