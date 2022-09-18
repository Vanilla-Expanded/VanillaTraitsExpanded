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
    [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState")]
    public class Patch_TryStartMentalState
    {
        private static bool Prefix(Pawn ___pawn, ref bool __result, MentalStateDef stateDef, string reason = null, bool forceWake = false,
            bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false)
        {
            if (___pawn.HasTrait(VTEDefOf.VTE_Schizoid) && !stateDef.IsExtreme)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
