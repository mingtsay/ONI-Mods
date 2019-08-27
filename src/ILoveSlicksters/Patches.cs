﻿using Harmony;
using System.Collections.Generic;
using TUNING;
using System;
using Klei.AI;
using UnityEngine;

namespace ILoveSlicksters
{

    class Patches
    {

        public static void OnLoad()
        {

            // Add the temperature modifier for decor oilfloater
            Type[] parameters_type = new Type[] { typeof(string), typeof(Tag), typeof(float), typeof(float), typeof(float), typeof(bool) };
            object[] paramaters = new object[] { "EthanolOilfloater", "EthanolOilfloaterEgg".ToTag(), 253.15f, 313.15f, CUSTOM_TUNINGS.EGG_MODIFIER_PER_SECOND.NORMAL, false };

            CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create<CREATURES.EGG_CHANCE_MODIFIERS>().Method("CreateTemperatureModifier", parameters_type).GetValue<System.Action>(paramaters)
            );

            // Add the temperature modifier for molten oilfloater
            object[] paramaters2 = new object[] { "RobotOilfloater", "RobotOilfloaterEgg".ToTag(), 393.15f, 543.15f, CUSTOM_TUNINGS.EGG_MODIFIER_PER_SECOND.NORMAL, false };

            CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create<CREATURES.EGG_CHANCE_MODIFIERS>().Method("CreateTemperatureModifier", parameters_type).GetValue<System.Action>(paramaters2)
            );



            // Add Leafy egg chance
            OilFloaterTuning.EGG_CHANCES_BASE.Add(
            new FertilityMonitor.BreedingChance
            {
                egg = "LeafyOilfloaterEgg".ToTag(),
                weight = 0.02f
            });
            // Add Robot egg chance
            OilFloaterTuning.EGG_CHANCES_HIGHTEMP.Add(
            new FertilityMonitor.BreedingChance
            {
                egg = "RobotOilfloaterEgg".ToTag(),
                weight = 0.02f
            });
            // Add the light modifier for leafy oilfloater
            CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                CUSTOM_TUNINGS.CreateLightModifier("LeafyOilfloater", "LeafyOilfloaterEgg".ToTag(), CUSTOM_TUNINGS.EGG_MODIFIER_PER_SECOND.NORMAL, false)
            );



            // replace the molten slicker egg chance to ethanol egg chance
            OilFloaterTuning.EGG_CHANCES_DECOR[1] = new FertilityMonitor.BreedingChance
            {
                egg = "EthanolOilfloaterEgg".ToTag(),
                weight = 0.02f
            };
            // Add the OwO slicker egg chance
            OilFloaterTuning.EGG_CHANCES_DECOR.Add(new FertilityMonitor.BreedingChance
            {
                egg = "OwO_OilfloaterEgg".ToTag(),
                weight = 0.20f
            });

        }
    }

    // add the OwO effect
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    class OwO_EffectPatch
    {
        public static void Postfix(Db __instance)
        {
            Effect OwO_Effect = new Effect("OwO_effect", " OwO Effect", "This duplicant saw something so cute that he can't think of anything else.", 300f, true, true, false, null, 10f);
            OwO_Effect.Add(new AttributeModifier("StressDelta", - DUPLICANTSTATS.QOL_STRESS.BELOW_EXPECTATIONS.HARD * 2, "" ));
            __instance.effects.Add(OwO_Effect);

        }
    }

    // add Light sensibility for all slickers
    [HarmonyPatch(typeof(BaseOilFloaterConfig))]
    [HarmonyPatch("BaseOilFloater")]
    public class AddLightSensibilityPatch
    {
        public static void Postfix(GameObject __result)
        {
            __result.AddOrGet<LightVulnerable>();
        }
    }
}