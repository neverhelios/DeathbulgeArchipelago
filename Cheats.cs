using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArticyEnums;
using Combat;
using Core;
using HarmonyLib;
using PixelCrushers.DialogueSystem;


namespace DeathbulgeArchipelagoClient;

class Cheats_Patch
{
    static readonly string[] maxedStats = ["Strength", "Toughness", "Spirit", "Resistance", "Speed", "Focus", "LifeRegen", "SpecialHypeRegen"];

    // [HarmonyPatch(typeof(StatDamageCalculator))]
    // [HarmonyPatch("RecomputeStat", [typeof(Combatant), typeof(string), typeof(int), typeof(int), typeof(StatUsageList)])]
    // [HarmonyPostfix]
    // static void Postfix_RecomputeStat_SetMaxStats(Combatant comb, string statName, int min, int max, StatUsageList statUsageOverride)
    // {
    //     if (maxedStats.Contains(statName))
    //         comb.Current.SetStat(statName, max);
    // }

    static readonly FieldInfo statValuesFieldInfo = typeof(Stats).GetField("statValues", BindingFlags.NonPublic | BindingFlags.Instance);
    [HarmonyPatch(typeof(PartyCharacter))]
    [HarmonyPatch("RecomputeStats", [typeof(Combatant), typeof(Item), typeof(List<Item>), typeof(ActionBar), typeof(bool)])]
    [HarmonyPostfix]
    static void Postfix_RecomputeStats_SetMaxStats(Combatant comb, Item classObj, List<Item> patches, ActionBar actionBar, bool isCounter)
    {
        Stats statLayer = new Stats();
        Dictionary<string, float> statValues = (Dictionary<string, float>)statValuesFieldInfo.GetValue(statLayer);
        statValues["MaxLife"] = 1000f;
        statValues["MaxHype"] = 1000f;
        statValues["Focus"] = 1000f;
        statValues["Spirit"] = 1000f;
        statValues["Resistance"] = 1000f;
        statValues["Strength"] = 1000f;
        statValues["Toughness"] = 1000f;
        statValues["Speed"] = 1000f;
        // statValues["IncomingDamage"] = 1000f;
        // statValues["IncomingNoiseDamage"] = 1000f;
        // statValues["IncomingMelodyDamage"] = 1000f;
        // statValues["IncomingHealing"] = 1000f;
        // statValues["OutgoingDamage"] = 1000f;
        // statValues["OutgoingNoiseDamage"] = 1000f;
        // statValues["OutgoingMelodyDamage"] = 1000f;
        // statValues["OutgoingBeatDamage"] = 1000f;
        // statValues["OutgoingModDamage"] = 1000f;
        // statValues["OutgoingMerchDamage"] = 1000f;
        // statValues["OutgoingHealing"] = 1000f;
        // statValues["OutgoingBeatHealing"] = 1000f;
        // statValues["OutgoingModHealing"] = 1000f;
        // statValues["OutgoingMerchHealing"] = 1000f;
        statValues["LifeRegen"] = 1000f;
        statValues["SpecialHypeRegen"] = 1000f;
        statLayer.StatUsage = StatUsageList.Additive;
        comb.AddStatsLayer(statLayer);

        CommonObjects.GetDamageCalc().RecomputeCurrent(comb, 1000);
    }

    // Infinite damage
    [HarmonyPatch(typeof(WorldCombatant))]
    [HarmonyPatch("Damage")]
    [HarmonyPrefix]
    static void InfinitePower(ref int damage)
    {
        damage = 100000;
    }

}