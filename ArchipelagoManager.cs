using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Field;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DeathbulgeArchipelagoClient;

class ArchipelagoManager : MonoBehaviour
{
    public static ArchipelagoManager instance = null;

    public ArchipelagoSession currSession = null;
    public LoginSuccessful logInfos;

    private long player;

    private class LocationData
    {
        public string itemName { get; set; }
        public long itemId { get; set; }
        public long player { get; set; }
        public string playerName { get; set; }
        public ItemFlags itemFlags { get; set; }
        public long id { get; set; }
    }

    private Dictionary<string, LocationData> locations = new();


    public void CreateSession(string server, string user)
    {
        currSession = ArchipelagoSessionFactory.CreateSession(server, 38281);

        // Must go BEFORE a successful connection attempt
        currSession.Items.ItemReceived += (receivedItemsHelper) =>
        {
            ItemInfo itemReceived = receivedItemsHelper.PeekItem();
            ItemDispatcher.AddDispatchedItem(itemReceived);
            receivedItemsHelper.DequeueItem();
        };

        LoginResult result;
        try
        {
            result = currSession.TryConnectAndLogin("Deathbulge", user, ItemsHandlingFlags.AllItems);
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
            currSession = null;
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string errorMessage = $"Failed to Connect to {server} as {user}:";
            foreach (string error in failure.Errors)
            {
                errorMessage += $"\n    {error}";
            }
            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                errorMessage += $"\n    {error}";
            }
            Plugin.Logger.LogError(errorMessage);
        }
        logInfos = (LoginSuccessful)result;
        player = logInfos.Slot;
        ItemDispatcher.player = logInfos.Slot;

        GetOwnLocationData().Wait();
    }

    public bool IsLocalLocation(string location)
    {
        return locations[location].player == player;
    }

    public string GetLocationItem(string location)
    {
        return locations[location].itemName;
    }

    public string GetPlayerName(string location)
    {
        return locations[location].playerName;
    }

    public ItemFlags GetItemFlags(string location)
    {
        return locations[location].itemFlags;
    }

    public async Task GetOwnLocationData()
    {
        locations.Clear();
        long[] allLocations = currSession.Locations.AllLocations.ToArray();
        Dictionary<long, ScoutedItemInfo> scoutedLocations = currSession.Locations.ScoutLocationsAsync(allLocations).Result;
        foreach (long location in allLocations)
        {
            if (!scoutedLocations.ContainsKey(location))
            {
                Plugin.Logger.LogError($"Location {location} couldn't be scouted");
                continue;
            }

            ScoutedItemInfo scoutedItem = scoutedLocations[location];

            LocationData locationData = new LocationData();
            if (scoutedItem == null || String.IsNullOrEmpty(scoutedItem.ItemName))
            {
                locationData.player = 0;
                locationData.playerName = "No one";
                locationData.itemFlags = ItemFlags.None;
                locationData.itemId = 0;
                if (String.IsNullOrEmpty(scoutedItem.ItemName))
                {
                    Debug.LogError($"Player:{scoutedItem.Player.Name} Game:{scoutedItem.Player.Game}, ItemId:{scoutedItem.ItemId}");
                    locationData.player = scoutedItem.Player;
                    locationData.playerName = scoutedItem.Player.Name;
                    locationData.itemFlags = scoutedItem.Flags;
                    locationData.itemId = scoutedItem.ItemId;
                }
                else
                    Debug.LogError($"There is no Data for this Location {location}");
                locationData.id = location;
                locationData.itemName = "No item name";
            }
            else
            {
                locationData.itemName = scoutedItem.ItemName;
                locationData.itemId = scoutedItem.ItemId;
                locationData.playerName = scoutedItem.Player.Name;
                locationData.player = scoutedItem.Player;
                locationData.itemFlags = scoutedItem.Flags;
                locationData.id = location;
            }
            locations.Add(currSession.Locations.GetLocationNameFromId(location, "Deathbulge"), locationData);
        }
    }
}