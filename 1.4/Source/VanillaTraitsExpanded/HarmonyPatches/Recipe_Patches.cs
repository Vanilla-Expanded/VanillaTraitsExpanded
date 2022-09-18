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
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    internal static class MakeRecipeProducts_Patch
    {
        private static void Postfix(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver)
        {
            if (worker.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
                if (recipeDef == DefDatabase<RecipeDef>.GetNamed("ButcherCorpseFlesh") && ingredients != null && ingredients.Where(x => x is Corpse corpse).Any())
                {
                    TraitsManager.Instance.madSurgeonsWithLastHarvestedTick[worker] = GenTicks.TicksAbs;
                    worker.TryGiveThought(VTEDefOf.VTE_HarvestedOrgans);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    internal static class TakeDamage_Patch
    {
        public static bool dirty = false;
        private static void Postfix()
        {
            dirty = true;
        }
    }
    
    [HarmonyPatch(typeof(Recipe_RemoveBodyPart), "ApplyOnPawn")]
    internal static class ApplyOnPawn_Patch
    {
        private static void Prefix()
        {
            TakeDamage_Patch.dirty = false;
        }
        private static void Postfix(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
                if (TakeDamage_Patch.dirty)
                {
                    TraitsManager.Instance.madSurgeonsWithLastHarvestedTick[billDoer] = GenTicks.TicksAbs;
                    billDoer.TryGiveThought(VTEDefOf.VTE_HarvestedOrgans);
                }
            }
            TakeDamage_Patch.dirty = false;
        }
    }
}
