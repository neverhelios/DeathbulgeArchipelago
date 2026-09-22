using System.Reflection;
using BepInEx.Logging;
using Core;
using Field.UI;
using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient.ItemsManagement;

class ShopManager
{

    static readonly FieldInfo currentSaleItemFieldInfo = typeof(ShopsMenuUI).GetField("currentSaleItem", BindingFlags.NonPublic | BindingFlags.Instance);
    // Send checks for classic treasures
    [HarmonyPatch(typeof(ShopsMenuUI))]
    [HarmonyPatch("PurchaseCurrentItem")]
    [HarmonyPrefix]
    static bool Prefix_PurchaseCurrentItem(ShopsMenuUI __instance)
    {
        Item purchasedItem = (Item)currentSaleItemFieldInfo.GetValue(__instance);

        Plugin.Logger.LogInfo($"Item: {purchasedItem.Name} (ID: {purchasedItem.id})");
        foreach (var field in purchasedItem.fields)
        {
            Plugin.Logger.LogInfo($"   {field.title} = {field.value}");
        }

        string locationString = $"{purchasedItem.Name} Shop Item";

        string addedItem = Items.SendCheckAndGetItem(locationString);
        if (ArchipelagoManager.instance.IsLocalLocation(locationString))
        {
            Item item = DialogueManager.MasterDatabase.GetItem(addedItem);
            CoreHelper.AddItem(item);
        }
        return true;
    }

}