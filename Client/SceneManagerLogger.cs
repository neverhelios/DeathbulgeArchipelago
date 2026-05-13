using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;


namespace DeathbulgeArchipelagoClient;

class SceneManagerLogger
{
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Plugin.Logger.LogInfo($"Scene loaded: {scene.name} {mode}");
        Plugin.Logger.LogInfo($"Scene active: {SceneManager.GetActiveScene().name}\n\n");

        foreach(GameObject rootObj in scene.GetRootGameObjects())
        {
            Plugin.Logger.LogInfo(rootObj + "\n\n");
            PrintChildrensRecursive(rootObj.transform, "+", false);
        }
        Plugin.Logger.LogInfo("-----------------------------------------------------------------");
    }

    public static void OnSceneUnloaded(Scene scene)
    {
        Plugin.Logger.LogInfo($"Scene unloaded: {scene.name}");
    }

    public static void OnActiveSceneChanged(Scene sceneStart, Scene sceneFinished)
    {
        Plugin.Logger.LogInfo($"Scene changed from {sceneStart.name} to {sceneFinished.name}");
    }

    private static void PrintChildrensRecursive(Transform transform, string prefix, bool bPrintComponent)
    {
        if(bPrintComponent)
        {
            GameObject curr_object = transform.gameObject;
            for (int i = 0; i < curr_object.GetComponentCount(); i++)
            {
                var currComponent = curr_object.GetComponentAtIndex(i);
                Plugin.Logger.LogInfo($"{prefix}-> Component[{i}]: {currComponent.GetType()}");
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currChild = transform.GetChild(i);
            Plugin.Logger.LogInfo($"{prefix} Child[{i}]: {currChild.gameObject}");
            PrintChildrensRecursive(currChild, $"{prefix}+", bPrintComponent);
        }
    }
}