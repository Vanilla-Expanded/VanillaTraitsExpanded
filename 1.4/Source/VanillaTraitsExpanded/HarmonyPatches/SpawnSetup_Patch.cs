using HarmonyLib;
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
    [HarmonyPatch(typeof(TraitSet))]
    [HarmonyPatch("GainTrait")]
    public static class GainTrait_Patch
    {
        private static void Postfix(Pawn ___pawn)
        {
            SpawnSetup_Patch.AddPawn(___pawn);
        }
    }

    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("SpawnSetup")]

    public static class SpawnSetup_Patch
    {
        private static void Postfix(Pawn __instance)
        {
            AddPawn(__instance);
        }
        public static void AddPawn(Pawn __instance)
        {
            try
            {
                if (__instance.HasTrait(VTEDefOf.VTE_Coward))
                {
                    TraitsManager.Instance.cowards.Add(__instance);
                }
                if (__instance.HasTrait(VTEDefOf.VTE_BigBoned))
                {
                    TraitsManager.Instance.bigBoned.Add(__instance);
                }

                if (__instance.HasTrait(VTEDefOf.VTE_Submissive))
                {
                    if (__instance.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowWorkSpeed) == null)
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.VTE_SlowWorkSpeed, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_Snob))
                {
                    TraitsManager.Instance.snobs.Add(__instance);
                }

                if (__instance.HasTrait(VTEDefOf.VTE_MadSurgeon))
                {
                    if (TraitsManager.Instance.madSurgeonsWithLastHarvestedTick == null)
                        TraitsManager.Instance.madSurgeonsWithLastHarvestedTick = new Dictionary<Pawn, int>();
                    if (!TraitsManager.Instance.madSurgeonsWithLastHarvestedTick.ContainsKey(__instance))
                    {
                        TraitsManager.Instance.madSurgeonsWithLastHarvestedTick[__instance] = GenTicks.TicksAbs;
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_Stoner))
                {
                    if (!__instance.health.hediffSet.HasHediff(VTEDefOf.SmokeleafAddiction))
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.SmokeleafAddiction, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_Lush))
                {
                    if (!__instance.health.hediffSet.HasHediff(VTEDefOf.AlcoholAddiction))
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.AlcoholAddiction, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_Wanderlust))
                {
                    if (TraitsManager.Instance.wanderLustersWithLastMapExitedTick == null) TraitsManager.Instance.wanderLustersWithLastMapExitedTick = new Dictionary<Pawn, int>();
                    if (!TraitsManager.Instance.wanderLustersWithLastMapExitedTick.ContainsKey(__instance))
                    {
                        TraitsManager.Instance.wanderLustersWithLastMapExitedTick[__instance] = GenTicks.TicksAbs;
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_Insomniac))
                {
                    if (__instance.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_RestSlowFallFactor) == null)
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.VTE_RestSlowFallFactor, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_ThickSkinned))
                {
                    if (__instance.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_SlowerBleedingRate) == null)
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.VTE_SlowerBleedingRate, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
                if (__instance.HasTrait(VTEDefOf.VTE_ThinSkinned))
                {
                    if (__instance.health.hediffSet.GetFirstHediffOfDef(VTEDefOf.VTE_HigherBleedingRate) == null)
                    {
                        var hediff = HediffMaker.MakeHediff(VTEDefOf.VTE_HigherBleedingRate, __instance);
                        __instance.health.AddHediff(hediff);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Exception checking traits in {__instance}: {ex}");
            }
        }
    }
}
