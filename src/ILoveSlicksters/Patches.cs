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
        // Egg chance patches
        public static void OnLoad()
        {

            ModUtil.RegisterForTranslation(typeof(StringsPatch));


            // Add the temperature modifier for decor oilfloater
            Type[] parameters_type = new Type[] { typeof(string), typeof(Tag), typeof(float), typeof(float), typeof(float), typeof(bool) };
            object[] paramaters = new object[] { "EthanolOilfloater", "EthanolOilfloaterEgg".ToTag(), 253.15f, 313.15f, PHO_TUNING.EGG_MODIFIER_PER_SECOND.NORMAL, false };

            CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create<CREATURES.EGG_CHANCE_MODIFIERS>().Method("CreateTemperatureModifier", parameters_type).GetValue<System.Action>(paramaters)
            );

            // Add the temperature modifier for molten oilfloater
            object[] paramaters2 = new object[] { "RobotOilfloater", "RobotOilfloaterEgg".ToTag(), 523.15f, 743.15f, PHO_TUNING.EGG_MODIFIER_PER_SECOND.NORMAL, false };

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
                PHO_TUNING.CreateLightModifier("LeafyOilfloater", "LeafyOilfloaterEgg".ToTag(), PHO_TUNING.EGG_MODIFIER_PER_SECOND.NORMAL, false)
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
    // Kg eaten patch
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    public class KG_Eaten_Patch
    {
        public static void OnLoad()
        {
            Traverse.Create<OilFloaterConfig>().Field<float>("KG_ORE_EATEN_PER_CYCLE").Value = PHO_TUNING.OILFLOATER.KG_ORE_EATEN_PER_CYCLE.HIGH2;

            float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / PHO_TUNING.OILFLOATER.KG_ORE_EATEN_PER_CYCLE.HIGH2;

            Traverse.Create<OilFloaterConfig>().Field<float>("CALORIES_PER_KG_OF_ORE").Value = CALORIES_PER_KG_OF_ORE;
            Traverse.Create<OilFloaterHighTempConfig>().Field<float>("CALORIES_PER_KG_OF_ORE").Value = CALORIES_PER_KG_OF_ORE;



        }
    }


    // add the OwO effect
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    public class OwO_EffectPatch
    {
        public static void Postfix(Db __instance)
        {
            Effect OwO_Effect = new Effect("OwO_effect", " OwO Effect", "This duplicant saw something so cute that he can't think of anything else.", 300f, true, true, false, null, 10f);
            OwO_Effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, - DUPLICANTSTATS.QOL_STRESS.BELOW_EXPECTATIONS.HARD, "OwO Effect"));
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
