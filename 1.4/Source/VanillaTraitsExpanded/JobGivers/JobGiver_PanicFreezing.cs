using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class JobGiver_PanicFreezing : ThinkNode_JobGiver
	{
        protected override Job TryGiveJob(Pawn pawn)
        {
            var mentalState = pawn.MentalState as MentalState_PanicFreezing;
            if (mentalState == null)
            {
                return null;
            }
            var cells = pawn.ownership?.OwnedRoom?.Cells;
            if (cells?.Count() > 0)
            {
                if (!cells.Contains(pawn.Position))
                {
                    Job job = JobMaker.MakeJob(JobDefOf.Flee, cells.Where(x => x.Walkable(pawn.Map)).RandomElement());
                    job.locomotionUrgency = LocomotionUrgency.Sprint;
                    return job;
                }
                else
                {
                    Job job = JobMaker.MakeJob(JobDefOf.Wait, 60);
                    return job;
                }
            }
            else
            {
                Job job = JobMaker.MakeJob(JobDefOf.Wait, 60);
            }
            return null;
        }
	}
}
