using RimWorld;
using Verse;

namespace VanillaTraitsExpanded;

public class Hediff_ForcedWork : Hediff
{
    private const int DefaultLastForcedWorkTick = -1000000;

    private bool suppressed = false;
    private bool activeWhileForcedWork = true;
    private int nextForcedWorkTickEnd = DefaultLastForcedWorkTick;
    // RecacheData can run while the hediff list is being iterated (e.g. Notify_Spawned), so removal is left to ShouldRemove.
    private bool pendingRemoval = false;

    public override int CurStageIndex => pendingRemoval || suppressed || (Find.TickManager.TicksGame < nextForcedWorkTickEnd) != activeWhileForcedWork ? 0 : 1;

    public override bool ShouldRemove => pendingRemoval || base.ShouldRemove;

    public override void TickInterval(int delta)
    {
        base.TickInterval(delta);

        // Re-check before the health tracker acts on ShouldRemove, in case a trait was gained since.
        if (pendingRemoval)
            RecacheData();

        if (suppressed)
            nextForcedWorkTickEnd = DefaultLastForcedWorkTick;
        else if (pawn is { CurJob: not null, CurJob.playerForced: true, Drafted: false })
            nextForcedWorkTickEnd = Find.TickManager.TicksGame + GenDate.TicksPerHour;

        // Shouldn't be needed... But exists for extra safety.
        if (pawn.IsHashIntervalTick(GenDate.TicksPerDay, delta))
            RecacheData();
    }

    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref nextForcedWorkTickEnd, nameof(nextForcedWorkTickEnd), DefaultLastForcedWorkTick);

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
            RecacheData();
    }

    public override void PostAdd(DamageInfo? dinfo)
    {
        base.PostAdd(dinfo);

        RecacheData();
    }

    public override void Notify_Spawned()
    {
        base.Notify_Spawned();

        RecacheData();
    }

    public void RecacheData()
    {
        if (pawn?.health?.hediffSet?.hediffs == null)
            return;

        if (pawn.story?.traits == null)
        {
            pendingRemoval = true;
            return;
        }

        pendingRemoval = false;

        var submissiveActive = false;
        var anySuppressed = false;
        var rebelActive = false;

        for (var i = 0; i < pawn.story.traits.allTraits.Count; ++i)
        {
            var trait = pawn.story.traits.allTraits[i];
            if (trait.def == VTEDefOf.VTE_Submissive)
            {
                if (trait.Suppressed)
                    anySuppressed = true;
                else
                    submissiveActive = true;
            }
            else if (trait.def == VTEDefOf.VTE_Rebel)
            {
                if (trait.Suppressed)
                    anySuppressed = true;
                else
                    rebelActive = true;
            }
        }

        suppressed = false;

        if (submissiveActive && rebelActive)
            suppressed = true;
        else if (submissiveActive)
            activeWhileForcedWork = false;
        else if (rebelActive)
            activeWhileForcedWork = true;
        else if (anySuppressed)
            suppressed = true;
        else
            pendingRemoval = true;
    }
}