using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using static Verse.DamageWorker;

namespace VanillaTraitsExpanded
{
    [HarmonyPatch(typeof(DamageWorker_AddInjury), "ApplyDamageToPart")]
    public static class Patch_ApplyDamageToPart
    {
        public static void Prefix(ref DamageInfo dinfo, Pawn pawn, DamageResult result)
        {
            if (dinfo.Instigator is Pawn instigator && instigator.HasTrait(VTEDefOf.VTE_DrunkenMaster) && (dinfo.Weapon == null || dinfo.Weapon == ThingDefOf.Human || dinfo.Weapon.IsMeleeWeapon))
            {
                var alcoholHediff = GetAlcoholHediff(instigator);
                if (alcoholHediff != null)
                {
                    var newDamAmount = dinfo.Amount * (1 + (alcoholHediff.CurStageIndex / 2f));
                    //Log.Message(instigator + " has Drunken Master trait, melee damage is increased - old value: " + dinfo.Amount + " - new value: " + newDamAmount);
                    dinfo.SetAmount(newDamAmount);
                }
            }
        }

        public static Hediff GetAlcoholHediff(Pawn pawn)
        {
            if (pawn.health?.hediffSet?.hediffs != null)
            {
                foreach (var hediff in pawn.health.hediffSet.hediffs)
                {
                    if (hediff is Hediff_Alcohol || hediff.TryGetComp<HediffComp_Effecter>()?.Props.stateEffecter == VTEDefOf.Drunk)
                    {
                        return hediff;
                    }
                }
            }

            return null;
        }
    }
}
