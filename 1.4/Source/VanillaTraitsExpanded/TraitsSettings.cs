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
    class TraitsSettings : ModSettings
    {
        public Dictionary<string, bool> traitStates = new Dictionary<string, bool>();
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref traitStates, "traitStates", LookMode.Value, LookMode.Value, ref traitKeys, ref boolValues);
        }

        private List<string> traitKeys;
        private List<bool> boolValues;

        public void DoSettingsWindowContents(Rect inRect)
        {
            var keys = traitStates.Keys.ToList().OrderByDescending(x => x).ToList();
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect rect2 = new Rect(0f, 0f, inRect.width - 30f, keys.Count * 24);
            Widgets.BeginScrollView(rect, ref scrollPosition, rect2, true);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(rect2);
            for (int num = keys.Count - 1; num >= 0; num--)
            {
                var test = traitStates[keys[num]];
                listingStandard.CheckboxLabeled(keys[num], ref test);
                traitStates[keys[num]] = test;
            }
            listingStandard.End();
            Widgets.EndScrollView();
            base.Write();
        }
        private static Vector2 scrollPosition = Vector2.zero;

    }
}

