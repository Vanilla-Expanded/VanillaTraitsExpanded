using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    public class TraitsManager : GameComponent
    {
        public static TraitsManager Instance;
        public TraitsManager()
		{
            Instance = this;
        }

		public TraitsManager(Game game)
		{
            Instance = this;
        }

        public Dictionary<Pawn, Job> forcedJobs = new Dictionary<Pawn, Job>();
        public HashSet<Pawn> perfectionistsWithJobsToStop = new HashSet<Pawn>();
        public HashSet<Pawn> cowards = new HashSet<Pawn>();
        public HashSet<Pawn> bigBoned = new HashSet<Pawn>();
        public HashSet<Pawn> snobs = new HashSet<Pawn>();
        public Dictionary<Pawn, int> madSurgeonsWithLastHarvestedTick = new Dictionary<Pawn, int>();
        public Dictionary<Pawn, int> wanderLustersWithLastMapExitedTick = new Dictionary<Pawn, int>();
        public Dictionary<Pawn, int> squeamishWithLastVomitedTick = new Dictionary<Pawn, int>();
        public Dictionary<Pawn, int> absentMindedWithLastDiscardedTick = new Dictionary<Pawn, int>();

        public void PreInit()
        {
            try
            {
                Instance = this;
                if (forcedJobs == null) forcedJobs = new Dictionary<Pawn, Job>();
                if (perfectionistsWithJobsToStop == null) perfectionistsWithJobsToStop = new HashSet<Pawn>();
                if (cowards == null) cowards = new HashSet<Pawn>();
                if (bigBoned == null) bigBoned = new HashSet<Pawn>();
                if (snobs == null) snobs = new HashSet<Pawn>();
                if (madSurgeonsWithLastHarvestedTick == null) madSurgeonsWithLastHarvestedTick = new Dictionary<Pawn, int>();
                if (wanderLustersWithLastMapExitedTick == null) wanderLustersWithLastMapExitedTick = new Dictionary<Pawn, int>();
                if (squeamishWithLastVomitedTick == null) squeamishWithLastVomitedTick = new Dictionary<Pawn, int>();
                if (absentMindedWithLastDiscardedTick == null) absentMindedWithLastDiscardedTick = new Dictionary<Pawn, int>();
                TryRemoveWrongPawns();

                foreach (var f in forcedJobs)
                {
                    if (!f.Key.HasTrait(VTEDefOf.VTE_AbsentMinded))
                    {
                        Log.Error("Pawn " + f.Key + " has no VTE_AbsentMinded trait and is in the wrong list of absent-minded pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var f in absentMindedWithLastDiscardedTick)
                {
                    if (!f.Key.HasTrait(VTEDefOf.VTE_AbsentMinded))
                    {
                        Log.Error("Pawn " + f.Key + " has no VTE_AbsentMinded trait and is in the wrong list of absent-minded pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var p in perfectionistsWithJobsToStop)
                {
                    if (!p.HasTrait(VTEDefOf.VTE_Perfectionist))
                    {
                        Log.Error("Pawn " + p + " has no VTE_Perfectionist trait and is in the wrong list of perfectionists pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var p in cowards)
                {
                    if (!p.HasTrait(VTEDefOf.VTE_Coward))
                    {
                        Log.Error("Pawn " + p + " has no VTE_Coward trait and is in the wrong list of coward pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var p in bigBoned)
                {
                    if (!p.HasTrait(VTEDefOf.VTE_BigBoned))
                    {
                        Log.Error("Pawn " + p + " has no VTE_BigBoned trait and is in the wrong list of big boned pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var p in snobs)
                {
                    if (!p.HasTrait(VTEDefOf.VTE_Snob))
                    {
                        Log.Error("Pawn " + p + " has no VTE_Snob trait and is in the wrong list of snob pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }

                foreach (var p in madSurgeonsWithLastHarvestedTick)
                {
                    if (!p.Key.HasTrait(VTEDefOf.VTE_MadSurgeon))
                    {
                        Log.Error("Pawn " + p.Key + " has no VTE_MadSurgeon trait and is in the wrong list of mad surgeons pawns. Please report it to Vanilla Traits Expanded mod page");
                    }
                }
            }

            catch
            {

            }

        }
        public override void StartedNewGame()
        {
            PreInit();
            base.StartedNewGame();
        }

        public override void LoadedGame()
        {
            PreInit();
            base.LoadedGame();
        }

        public void TryInterruptForcedJobs()
        {
            if (forcedJobs is null) PreInit();
            var keysToRemove = new List<Pawn>();
            foreach (var data in forcedJobs)
            {
                if (data.Key.Map != null)
                {
                    if (data.Key.CurJob == data.Value)
                    {
                        if ((absentMindedWithLastDiscardedTick.ContainsKey(data.Key)
                            && GenTicks.TicksAbs > absentMindedWithLastDiscardedTick[data.Key] + (GenDate.TicksPerHour * 2f)
                            || !absentMindedWithLastDiscardedTick.ContainsKey(data.Key)) && Rand.Chance(0.03f))
                        {
                            //Log.Message(data.Key + " - stops forced " + data.Key.CurJob + " due to absent-minded trait");
                            Messages.Message("VTE.PawnStopsForcedJob".Translate(data.Key.Named("PAWN")), data.Key, MessageTypeDefOf.SilentInput, historical: false);
                            data.Key.jobs.StopAll();
                            absentMindedWithLastDiscardedTick[data.Key] = GenTicks.TicksAbs;
                        }
                    }
                    else
                    {
                        keysToRemove.Add(data.Key);
                    }
                }
            }
            foreach (var key in keysToRemove)
            {
                forcedJobs.Remove(key);
            }
        }

        public void TryForceFleeCowards()
        {
            if (cowards is null) PreInit();
            foreach (var pawn in cowards)
            {
                if (pawn?.Map != null && !pawn.Downed && !pawn.Dead && Rand.Chance(0.1f))
                {
                    var enemies = pawn.Map.attackTargetsCache?.GetPotentialTargetsFor(pawn)?.Where(x => 
                        (x is Pawn pawnEnemy && !pawnEnemy.Dead && !pawnEnemy.Downed || !(x.Thing is Pawn) && x.Thing.DestroyedOrNull())
                        && x.Thing.Position.DistanceTo(pawn.Position) < 15f 
                        && GenSight.LineOfSight(x.Thing.Position, pawn.Position, pawn.Map))?.Select(y => y.Thing);
                    if (enemies?.Count() > 0)
                    {
                        if (pawn.Faction == Faction.OfPlayer)
                        {
                            TraitUtils.MakeFlee(pawn, enemies.OrderBy(x => x.Position.DistanceTo(pawn.Position)).First(), 15, enemies.ToList());
                            Messages.Message("VTE.PawnCowardlyFlees".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                        }
                        else if (pawn.Faction != null)
                        {
                            TraitUtils.MakeExit(pawn);
                            if (pawn.HostileTo(Faction.OfPlayer))
                            {
                                Messages.Message("VTE.PawnCowardlyExitMapHostile".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                            }
                            else if (pawn.Faction.RelationKindWith(Faction.OfPlayer) == FactionRelationKind.Ally)
                            {
                                Messages.Message("VTE.PawnCowardlyExitMapAlly".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                            }
                            else
                            {
                                Messages.Message("VTE.PawnCowardlyExitMapNeutral".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                            }
                        }
                    }
                }
            }
        }

        public void TryBreakChairsUnderBigBoneds()
        {
            if (bigBoned is null) PreInit();
            foreach (var pawn in bigBoned)
            {
                if (pawn?.Map != null && !pawn.pather.Moving && Rand.Chance(0.05f))
                {
                    var firstBuilding = pawn.Position.GetFirstBuilding(pawn.Map);
                    if (firstBuilding?.def?.building?.isSittable ?? false && !(firstBuilding is Building_Throne))
                    {
                        //if (latestChairsBreaks.ContainsKey(pawn.GetUniqueLoadID() + firstBuilding.GetUniqueLoadID()))
                        if (pawn.CurJobDef == JobDefOf.Ingest)
                        {
                            firstBuilding.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                            pawn.jobs.StopAll();
                            Messages.Message("VTE.PawnBreaksChairs".Translate(firstBuilding.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                        }
                        else if (pawn.CurJobDef == VTEDefOf.WatchTelevision)
                        {
                            var chairs = pawn.Position.GetFirstBuilding(pawn.Map);
                            Messages.Message("VTE.PawnBreaksChairs".Translate(chairs.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                            firstBuilding.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                            pawn.jobs.StopAll();
                        }
                    }
                    else if (pawn.jobs?.curDriver is JobDriver_SitFacingBuilding && pawn.CurJob?.targetB.Thing != null && !(pawn.CurJob?.targetB.Thing is Building_Throne))
                    {
                        Messages.Message("VTE.PawnBreaksChairs".Translate(pawn.CurJob.targetB.Thing.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                        pawn.CurJob.targetB.Thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                        pawn.jobs.StopAll();
                    }
                }
            }
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                TryInterruptForcedJobs();
            }
            if (Find.TickManager.TicksGame % 300 == 0)
            {
                TryForceFleeCowards();
            }
            if (Find.TickManager.TicksGame % 500 == 0)
            {
                TryBreakChairsUnderBigBoneds();
            }
            if (Find.TickManager.TicksGame % 2000 == 0)
            {
                TryRemoveWrongPawns();
            }
            if (perfectionistsWithJobsToStop.Count > 0)
            {
                foreach (var pawn in perfectionistsWithJobsToStop)
                {
                    pawn.jobs.StopAll();
                    if (pawn.HasTrait(VTEDefOf.VTE_Perfectionist))
                    {
                        pawn.TryGiveThought(VTEDefOf.VTE_CouldNotFinishItem);
                    }
                }
                perfectionistsWithJobsToStop.Clear();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref forcedJobs, "forcedJobs", LookMode.Reference, LookMode.Reference, ref pawnKeys, ref jobValues);
            Scribe_Collections.Look(ref madSurgeonsWithLastHarvestedTick, "madSurgeonsWithLastHarvestedTick", LookMode.Reference, LookMode.Value, ref pawnKeys2, ref tickValues);
            Scribe_Collections.Look(ref wanderLustersWithLastMapExitedTick, "wanderLustersWithLastMapExitedTick", LookMode.Reference, LookMode.Value, ref pawnKeys3, ref tickValues1);
            Scribe_Collections.Look(ref squeamishWithLastVomitedTick, "squeamishWithLastVomitedTick", LookMode.Reference, LookMode.Value, ref pawnKeys4, ref tickValues2);
            Scribe_Collections.Look(ref absentMindedWithLastDiscardedTick, "absentMindedWithLastDiscardedTick", LookMode.Reference, LookMode.Value, ref pawnKeys5, ref tickValues3);
            Scribe_Collections.Look(ref perfectionistsWithJobsToStop, "perfectionistsWithJobsToStop", LookMode.Reference);
            Scribe_Collections.Look(ref cowards, "cowards", LookMode.Reference);
            Scribe_Collections.Look(ref snobs, "snobs", LookMode.Reference);
            Scribe_Collections.Look(ref bigBoned, "bigBoned", LookMode.Reference);
        }

        public void RemoveDestroyedPawn(Pawn key)
        {
            forcedJobs.RemoveAll(x => x.Key == key);
            madSurgeonsWithLastHarvestedTick.RemoveAll(x => x.Key == key);
            wanderLustersWithLastMapExitedTick.RemoveAll(x => x.Key == key);
            perfectionistsWithJobsToStop.RemoveWhere(x => x == key);
            cowards.RemoveWhere(x => x == key);
            snobs.RemoveWhere(x => x == key);
            bigBoned.RemoveWhere(x => x == key);
            squeamishWithLastVomitedTick.RemoveAll(x => x.Key == key);
            absentMindedWithLastDiscardedTick.RemoveAll(x => x.Key == key);
        }

        public void TryRemoveWrongPawns()
        {
            forcedJobs.RemoveAll(x => !x.Key.HasTrait(VTEDefOf.VTE_AbsentMinded));
            absentMindedWithLastDiscardedTick.RemoveAll(x => !x.Key.HasTrait(VTEDefOf.VTE_AbsentMinded));
            madSurgeonsWithLastHarvestedTick.RemoveAll(x => !x.Key.HasTrait(VTEDefOf.VTE_MadSurgeon));
            wanderLustersWithLastMapExitedTick.RemoveAll(x => !x.Key.HasTrait(VTEDefOf.VTE_Wanderlust));
            perfectionistsWithJobsToStop.RemoveWhere(x => !x.HasTrait(VTEDefOf.VTE_Perfectionist));
            cowards.RemoveWhere(x => !x.HasTrait(VTEDefOf.VTE_Coward));
            snobs.RemoveWhere(x => !x.HasTrait(VTEDefOf.VTE_Snob));
            bigBoned.RemoveWhere(x => !x.HasTrait(VTEDefOf.VTE_BigBoned));
            squeamishWithLastVomitedTick.RemoveAll(x => !x.Key.HasTrait(VTEDefOf.VTE_Squeamish));
        }

        private List<Pawn> pawnKeys = new List<Pawn>();
        private List<Job> jobValues = new List<Job>();


        private List<Pawn> pawnKeys2 = new List<Pawn>();
        private List<int> tickValues = new List<int>();

        private List<Pawn> pawnKeys3 = new List<Pawn>();
        private List<int> tickValues1 = new List<int>();

        private List<Pawn> pawnKeys4 = new List<Pawn>();
        private List<int> tickValues2 = new List<int>();

        private List<Pawn> pawnKeys5 = new List<Pawn>();
        private List<int> tickValues3 = new List<int>();
    }
}
