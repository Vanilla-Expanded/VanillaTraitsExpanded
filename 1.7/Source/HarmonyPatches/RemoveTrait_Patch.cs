using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaTraitsExpanded;

[HarmonyPatch(typeof(TraitSet))]
[HarmonyPatch(nameof(TraitSet.RemoveTrait))]
public class RemoveTrait_Patch
{
    private static void Postfix(Pawn ___pawn)
    {
        try
        {
            if (___pawn.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowWorkSpeed) is Hediff_ForcedWork forcedWork)
                forcedWork.RecacheData();
        }
        catch (Exception ex)
        {
            Log.Error($"Exception checking traits in {___pawn}: {ex}");
        }
    }
}