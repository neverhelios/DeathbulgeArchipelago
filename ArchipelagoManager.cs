using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;

namespace DeathbulgeArchipelagoClient;

class ArchipelagoManager : MonoBehaviour
{
    public static ArchipelagoManager instance = null;

    public ArchipelagoSession currSession = null;
    public LoginSuccessful logInfos;


    public void CreateSession(string server, string user)
    {
        currSession = ArchipelagoSessionFactory.CreateSession(server, 38281);

        // Must go BEFORE a successful connection attempt
        currSession.Items.ItemReceived += (receivedItemsHelper) =>
        {
            ItemInfo itemReceived = receivedItemsHelper.PeekItem();

            if (itemReceived != null)
            {
                Plugin.Logger.LogInfo($"Got item {itemReceived.ItemName} from {itemReceived.LocationDisplayName} (game {itemReceived.LocationGame})");
            }

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

    }
}