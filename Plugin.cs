using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

using Field;
using System.Threading;
using Core;

namespace DeathbulgeArchipelagoClient;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private static ConfigEntry<bool> logSceneLoadedConfig;
    private static ConfigEntry<bool> logDialogueConfig;
    private static ConfigEntry<bool> logLuaShortcutsConfig;
    internal static ConfigEntry<bool> logLuaCommmandsInterceptedConfig;
    private static ConfigEntry<bool> enableCheatsConfig;
    internal static ConfigEntry<bool> fullStatsConfig;
    internal static ConfigEntry<bool> ohkoConfig;

    private static readonly Queue<ItemInfo> itemsToDispatch = new();

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        logSceneLoadedConfig = Config.Bind("Debug.Logging", "LogScenesLoaded", true, "For developpement purposes");
        logDialogueConfig = Config.Bind("Debug.Logging", "LogDialogue", true, "For developpement purposes");
        logLuaShortcutsConfig = Config.Bind("Debug.Logging", "LogLuaShortcuts", true, "For developpement purposes");
        logLuaCommmandsInterceptedConfig = Config.Bind("Debug.Logging", "LogLuaCommandsIntecepted", true, "For developpement purposes");
        enableCheatsConfig = Config.Bind("Debug.Cheat", "Enable", true, "Enable cheats (For debugging right ?)");
        fullStatsConfig = Config.Bind("Debug.Cheat", "FullStats", true, "All Stats at maximum for fast fights");
        ohkoConfig = Config.Bind("Debug.Cheat", "OHKO", true, "Every damage is a One Hit KO (Ennemies included I'm too lazy this is for debugging)");

        this.gameObject.AddComponent<ArchipelagoManager>();
        ArchipelagoManager.instance = gameObject.GetComponent<ArchipelagoManager>();

        ArchipelagoManager.instance.CreateSession("localhost", "DeathbulgeTest");


        if (logSceneLoadedConfig.Value)
        {
            SceneManager.sceneLoaded += SceneManagerLogger.OnSceneLoadedLog;
            SceneManager.activeSceneChanged += SceneManagerLogger.OnActiveSceneChanged;
        }
        SceneManager.sceneLoaded += SceneManagerLogger.OnSceneLoaded;
        SceneManager.sceneUnloaded += SceneManagerLogger.OnSceneUnloaded;


        if (logDialogueConfig.Value)
            Harmony.CreateAndPatchAll(typeof(DialogueTriggerLogger_Patch));

        if (enableCheatsConfig.Value)
            Harmony.CreateAndPatchAll(typeof(Cheats_Patch));

        if (logLuaShortcutsConfig.Value)
            Harmony.CreateAndPatchAll(typeof(PrintAllSequencer_Patch));

        Harmony.CreateAndPatchAll(typeof(TreasureManager_Patch));

        // StartCoroutine(DumpDatabaseWhenReady());

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} has finished patching!");

        Logger.LogInfo($"PATH Plugin {Paths.PluginPath}Deathbulge/DeathbulgeArchipelago/ressources");
    }

    void Update()
    {
        if (SceneManagerLogger.bIsFieldLoaded)
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
        itemsToDispatch.Enqueue(itemInfo);
    }
}
