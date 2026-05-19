using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using HarmonyLib;

namespace DeathbulgeArchipelagoClient;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private static ConfigEntry<bool> logSceneLoadedConfig;
    private static ConfigEntry<bool> logDialogueConfig;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        logSceneLoadedConfig = Config.Bind("Debug.Logging", "LogScenesLoaded", true, "For developpement purposes");
        logDialogueConfig = Config.Bind("Debug.Logging", "LogDialogue", true, "For developpement purposes");

        if (logSceneLoadedConfig.Value)
        {
            SceneManager.sceneLoaded += SceneManagerLogger.OnSceneLoaded;
            SceneManager.sceneUnloaded += SceneManagerLogger.OnSceneUnloaded;
            SceneManager.activeSceneChanged += SceneManagerLogger.OnActiveSceneChanged;
        }

        if (logDialogueConfig.Value)
            Harmony.CreateAndPatchAll(typeof(DialogueTrigger_Patch));

        // StartCoroutine(DumpDatabaseWhenReady());

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} has finished patching!");
    }
}
