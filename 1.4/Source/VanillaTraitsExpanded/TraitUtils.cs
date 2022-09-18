using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    [StaticConstructorOnStartup]
    public static class TraitUtils
    {
        public static bool HasTrait(this Pawn pawn, TraitDef traitDef)
        {
            if (traitDef != null && (pawn?.story?.traits?.HasTrait(traitDef) ?? false))
            {
                return true;
            }
            return false;
        }

        public static void TryGiveThought(this Pawn pawn, ThoughtDef thoughtDef)
        {
            pawn.needs?.mood?.thoughts.memories.TryGainMemory(thoughtDef);
        }

        public static void MakeFlee(Pawn pawn, Thing danger, int radius, List<Thing> dangers)
        {
            Job job = null;
            IntVec3 intVec;
            if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
            {
                intVec = pawn.CurJob.targetA.Cell;
            }
            else
            {
                intVec = CellFinderLoose.GetFleeDest(pawn, dangers, 24f);
            }

            if (intVec == pawn.Position)
            {
                intVec = GenRadial.RadialCellsAround(pawn.Position, radius, radius * 2).RandomElement();
            }
            if (intVec != pawn.Position)
            {
                job = JobMaker.MakeJob(JobDefOf.Flee, intVec, danger);
            }
            if (job != null)
            {
                //Log.Message(pawn + " flee");
                pawn.jobs.TryTakeOrderedJob(job);
            }
        }

        public static void MakeExit(Pawn pawn)
        {
            bool flag = (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (!pawn.CanReachMapEdge()) 
                    || (pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, countDormantPawnsAsHostile: true));
            if (!TryFindGoodExitDest(pawn, flag, out IntVec3 dest))
            {
                return;
            }
            if (flag)
            {
                using (PawnPath path = pawn.Map.pathFinder.FindPath(pawn.Position, dest, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings)))
                {
                    IntVec3 cellBefore;
                    Thing thing = path.FirstBlockingBuilding(out cellBefore, pawn);
                    if (thing != null)
                    {
                        Job job = DigUtility.PassBlockerJob(pawn, thing, cellBefore, canMineMineables: true, canMineNonMineables: true);
                        if (job != null)
                        {
                            pawn.jobs.TryTakeOrderedJob(job);
                            return;
                        }
                    }
                }
            }

            Job job2 = JobMaker.MakeJob(JobDefOf.Goto, dest);
            job2.exitMapOnArrival = true;
            job2.locomotionUrgency = LocomotionUrgency.Jog;
            job2.expiryInterval = 1000;
            job2.canBashDoors = true;
            pawn.jobs.TryTakeOrderedJob(job2);
        }

        private static bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
        {
            TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
            return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
        }
    }
}

