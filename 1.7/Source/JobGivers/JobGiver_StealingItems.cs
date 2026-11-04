using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    public class JobGiver_StealingItems : ThinkNode_JobGiver
    {
        public Pawn GetCandidateToSteal(Pawn thief, List<Pawn> pawns)
        {
            foreach (var p in pawns)
            {
                if (p.CurJobDef != JobDefOf.Goto || p.CurJob.targetA.Cell.DistanceToSquared(thief.Position) < p.Position.DistanceToSquared(thief.Position))
                {
                    return p;
                }
            }
            return null;
        }
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (Rand.Chance(0.5f))
            {
                return null;
            }
            if (pawn.MentalState is not MentalState_Kleptomaniac kleptomaniac || kleptomaniac.nextStealTick > Find.TickManager.TicksGame)
            {
                return null;
            }
            var pawnsCandidates = pawn.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike && x.Position.IsValid && x.Faction != pawn.Faction && !x.HostileTo(pawn)).ToList();
            if (pawnsCandidates.Count > 0)
            {
                var victim = GetCandidateToSteal(pawn, pawnsCandidates.OrderBy(x => x.Position.DistanceToSquared(pawn.Position)).ToList());
                if (victim != null)
                {
                    kleptomaniac.nextStealTick = Find.TickManager.TicksGame + MentalState_Kleptomaniac.StealingCooldown;
                    //Log.Message(pawn + " trying to steal item from " + victim + " in " + victim.positionInt);
                    return JobMaker.MakeJob(VTEDefOf.VTE_StealItems, victim);
                }
            }
            //Log.Message(pawn + " can't nothing steal");
            return null;
        }
    }
}

