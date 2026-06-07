using DeathbulgeArchipelagoClient;
using HarmonyLib;
using PixelCrushers.DialogueSystem;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;

public class PlacedItemCatcher : MonoBehaviour
{
    [HarmonyPatch(typeof(PersistentActiveData))]
    [HarmonyPatch("Check")]
    [HarmonyPrefix]
    static bool CatchPersistentActiveDataCheck(PersistentActiveData __instance)
    {
        string instancePath = Utils.GetHierarchyPath(__instance.transform);
        // Plugin.Logger.LogInfo($"Persistent data of {instancePath}");
        if (instancePath == "MapRoot/MapGraphics/LowerDecks/Interior (Floor 3-1)/Fancydoor-Controller")
        {
            __instance.target.SetActive(false);
            return false;
        }
        if (instancePath == "MapRoot/MapGraphics/SaevaTent/SaevaTent-Interior/pNPC-Trista-Tent")
        {
            // Remove the briff knocked out condition
            __instance.condition.luaConditions = ["Variable[\"Hoho.BriffTentPrimed\"] == false"];
            __instance.target.SetActive(__instance.condition.IsTrue(null));
            return false;
        }

        return true;
    }
}