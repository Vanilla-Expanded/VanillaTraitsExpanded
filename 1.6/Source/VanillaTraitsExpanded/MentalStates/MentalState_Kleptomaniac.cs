using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalState_Kleptomaniac : MentalState
    {
        public const int StealingCooldown = 850;
        public int nextStealTick = -1;

        public override RandomSocialMode SocialModeMax()
        {
            return 0f;
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref nextStealTick, nameof(nextStealTick), -1);
        }
    }
}
