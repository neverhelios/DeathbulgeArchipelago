using HarmonyLib;
using PixelCrushers.DialogueSystem;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;
class DialogueTrigger_Patch
{
    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnConversationStart")]
    [HarmonyPrefix]
    static void Prefix_ConvStart(Transform actor)
    {
        if(actor)
            Plugin.Logger.LogInfo($"Conversation starts on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("Fire")]
    [HarmonyPrefix]
    static void Prefix_Fire(Transform actor)
    {
        if(actor)
            Plugin.Logger.LogInfo($"Fire event on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnUse", [typeof(Transform)])]
    [HarmonyPrefix]
    static void Prefix_Use_Transform(Transform actor)
    {
        if(actor)
            Plugin.Logger.LogInfo($"On use on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnUse", [typeof(string)])]
    [HarmonyPrefix]
    static void Prefix_Use_Str(string message)
    {
        Plugin.Logger.LogInfo($"On use with message {message}");
    }


    [HarmonyPatch(typeof(DialogueSystemController))]
    [HarmonyPatch("StartConversation", [typeof(string), typeof(int)])]
    [HarmonyPrefix]
    static void Prefix_StartConversation(string title, int initialDialogueEntryID)
    {
        Plugin.Logger.LogInfo($"----> Dialogue {title}; Number {initialDialogueEntryID}");
    }

}