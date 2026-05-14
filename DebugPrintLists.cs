
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient;


class DebugPrintLists
{
    public static void PrintAllItems(bool bOnlyTreasure)
    {
        Plugin.Logger.LogInfo($"Time to print the item database baby !!!");
        List<Item> items = DialogueManager.instance.masterDatabase.items;
        foreach (Item item in items)
        {
            Item slot = DialogueManager.MasterDatabase.GetSlot(item, "TreasureSlot");
            if (bOnlyTreasure && slot == null)
            {
                if (item.IsFieldAssigned("TreasureMoney"))
                    Plugin.Logger.LogInfo($"Money Item: {item.Name}");
                continue;
            }
            Plugin.Logger.LogInfo($"Item: {item.Name} at slot {slot.Name}");
        }
    }
}