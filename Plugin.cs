using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

using Field;

namespace DeathbulgeArchipelagoClient;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private static ConfigEntry<bool> logSceneLoadedConfig;
    private static ConfigEntry<bool> logDialogueConfig;

    private static readonly Queue<ItemInfo> itemsToDispatch = new();

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        logSceneLoadedConfig = Config.Bind("Debug.Logging", "LogScenesLoaded", true, "For developpement purposes");
        logDialogueConfig = Config.Bind("Debug.Logging", "LogDialogue", true, "For developpement purposes");

        this.gameObject.AddComponent<ArchipelagoManager>();
        ArchipelagoManager.instance = gameObject.GetComponent<ArchipelagoManager>();

        ArchipelagoManager.instance.CreateSession("localhost", "DeathbulgeTest");


        if (logSceneLoadedConfig.Value)
        {
            SceneManager.sceneLoaded += SceneManagerLogger.OnSceneLoaded;
            SceneManager.sceneUnloaded += SceneManagerLogger.OnSceneUnloaded;
            SceneManager.activeSceneChanged += SceneManagerLogger.OnActiveSceneChanged;
        }

        if (logDialogueConfig.Value)
            Harmony.CreateAndPatchAll(typeof(DialogueTriggerLogger_Patch));

        Harmony.CreateAndPatchAll(typeof(DialogueTrigger_Patch));

        // StartCoroutine(DumpDatabaseWhenReady());

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} has finished patching!");
    }

    void Update()
    {
        // if(bCanGetItems)
        while (itemsToDispatch.Count > 0)
        {
            ItemInfo itemReceived = itemsToDispatch.Dequeue();
            Plugin.Logger.LogInfo($"Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Item item = DialogueManager.MasterDatabase.GetItem(Items.GetTreasureFromItemName(itemReceived.ItemName));
            TreasureUI treasureUI = CommonObjects.GetTreasureUI();
            treasureUI.Show(item);
        }
    }

    public static void AddDispatchedItem(ItemInfo itemInfo)
    {
        itemsToDispatch.Enqueue(itemInfo);
    }
}
