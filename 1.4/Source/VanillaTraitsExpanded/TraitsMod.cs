using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    class TraitsMod : Mod
    {
        public static TraitsSettings settings;
        public TraitsMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<TraitsSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            var traits = DefDatabase<TraitDef>.AllDefsListForReading;
            foreach (var trait in traits)
            {
                if (settings.traitStates == null) settings.traitStates = new Dictionary<string, bool>();
                if (!settings.traitStates.ContainsKey(trait.defName))
                {
                    settings.traitStates[trait.defName] = true;
                }
            }
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Vanilla Traits Expanded";
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            DefsRemover.DoDefsRemoval();
        }
    }
    [StaticConstructorOnStartup]
    public static class DefsRemover
    {
        static DefsRemover()
        {
            DoDefsRemoval();
        }
        public static void RemoveDef(TraitDef def)
        {
            try
            {
                if (DefDatabase<TraitDef>.AllDefsListForReading.Contains(def))
                {
                    MethodInfo methodInfo = AccessTools.Method(typeof(DefDatabase<TraitDef>), "Remove", null, null);
                    methodInfo.Invoke(null, new object[]
                    {
                        def
                    });
                }
            }
            catch { };
        }
        public static void DoDefsRemoval()
        {
            foreach (var traitState in TraitsMod.settings.traitStates)
            {
                if (!traitState.Value)
                {
                    var defToRemove = DefDatabase<TraitDef>.GetNamedSilentFail(traitState.Key);
                    if (defToRemove != null)
                    {
                        RemoveDef(defToRemove);
                    }
                }
            }
        }
    }
}