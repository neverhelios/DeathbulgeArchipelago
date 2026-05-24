using System.Collections.Generic;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Models;
using Core;
using Field;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

namespace DeathbulgeArchipelagoClient;

class ItemDispatcher
{
    public static bool bIsFieldLoaded { get; private set; } = false;
    private static readonly Queue<ItemInfo> itemsToDispatch = new();

    public static long player;


    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Plugin.Logger.LogInfo($"Scene loaded: {scene.name} {mode}");
        if (scene.name == "Field")
            SetFieldLoadedAsync(3000);
    }

    private static async void SetFieldLoadedAsync(int time)
    {
        await Task.Delay(time);
        bIsFieldLoaded = true;
    }

    public static void OnSceneUnloaded(Scene scene)
    {
        Plugin.Logger.LogInfo($"Scene unloaded: {scene.name}");
        if (scene.name == "Field")
        {
            bIsFieldLoaded = false;

            foreach (ItemInfo itemInfo in ArchipelagoManager.instance.currSession?.Items?.AllItemsReceived)
            {
                // TODO: Avoid my items
                AddDispatchedItem(itemInfo);
            }
        }
    }

    public static void DispatchItems()
    {
        if (bIsFieldLoaded)
        {
            while (itemsToDispatch.Count > 0)
            {
                TreasureUI treasureUI = CommonObjects.GetTreasureUI();

                if (!treasureUI.window.activeInHierarchy)
                {
                    ItemInfo itemReceived = itemsToDispatch.Dequeue();
                    // TODO: Special treatement for external money
                    Item item = DialogueManager.MasterDatabase.GetItem(Items.GetTreasureFromItemName(itemReceived.ItemName));
                    Item slot = DialogueManager.MasterDatabase.GetSlot(item, "TreasureSlot");
                    if (slot != null)
                    {
                        if (CoreHelper.HasItem(slot))
                            Plugin.Logger.LogInfo($"Already have the item {slot.Name}");
                        else
                            treasureUI.Show(item);
                    }
                    else
                    {
                        // TODO: Special treatement for external money
                        Plugin.Logger.LogInfo($"Argent, argent, argent");
                        treasureUI.Show(item);
                    }

                }
                else
                {
                    // Already printing something, we'll show the new item after
                    break;
                }


            }
        }
    }

    public static void AddDispatchedItem(ItemInfo itemInfo)
    {
        if (itemInfo != null)
        {
            Plugin.Logger.LogInfo($"Got item {itemInfo.ItemName} from {itemInfo.LocationDisplayName} (game {itemInfo.LocationGame})");
            if (itemInfo.Player.Slot == player)
                Plugin.Logger.LogInfo($"Oh it's mine ! Skipping this one :)");
            else
            {
                //List item
                itemsToDispatch.Enqueue(itemInfo);
            }

        }
    }
}