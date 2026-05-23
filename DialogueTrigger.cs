using HarmonyLib;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;

class DialogueTriggerLogger_Patch
{
    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnConversationStart")]
    [HarmonyPrefix]
    static void Prefix_ConvStart(Transform actor)
    {
        if (actor)
            Plugin.Logger.LogInfo($"Conversation starts on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("Fire")]
    [HarmonyPrefix]
    static void Prefix_Fire(Transform actor)
    {
        if (actor)
            Plugin.Logger.LogInfo($"Fire event on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnUse", [typeof(Transform)])]
    [HarmonyPrefix]
    static void Prefix_Use_Transform(Transform actor)
    {
        if (actor)
            Plugin.Logger.LogInfo($"On use on {actor.gameObject}");
    }

    [HarmonyPatch(typeof(DialogueSystemTrigger))]
    [HarmonyPatch("OnUse", [typeof(string)])]
    [HarmonyPrefix]
    static void Prefix_Use_Str(string message)
    {
        Plugin.Logger.LogInfo($"On use with message {message}");
    }


    [HarmonyPatch(typeof(DialogueManager))]
    [HarmonyPatch("StartConversation", [typeof(string), typeof(int), typeof(List<Transform>), typeof(Transform)])]
    [HarmonyPrefix]
    static void Prefix_StartConversation(string title, int initialDialogueEntryID, List<Transform> objectRefs, Transform self)
    {
        Plugin.Logger.LogInfo($"----> Dialogue {title}; Number {initialDialogueEntryID}; Launched by {self}, With ref List:");
        foreach (Transform curr_object in objectRefs)
        {
            Plugin.Logger.LogInfo($"      {curr_object}");
        }
        Conversation conversation = DialogueManager.instance.masterDatabase.GetConversation(title);
        PrintDialogueConveration(conversation);
    }

    static void PrintDialogueConveration(Conversation conversation)
    {
        if (conversation != null)
        {
            Plugin.Logger.LogInfo($"Conversation: {conversation.Title}");

            foreach (var entry in conversation.dialogueEntries)
            {
                if (entry.Sequence.ToString() == "Continue()" && entry.userScript == "")
                {
                    Plugin.Logger.LogInfo($"Entry {entry.id} -> Empty");
                }
                else
                {
                    Plugin.Logger.LogInfo($"Entry {entry.id}: ");
                    if (entry.DialogueText != "")
                        Plugin.Logger.LogInfo($"Text: {entry.DialogueText}");
                    if (entry.Sequence != "")
                        Plugin.Logger.LogInfo($"Sequence: {entry.Sequence}");
                    if (entry.userScript != "")
                        Plugin.Logger.LogInfo($"Lua: {entry.userScript}\n");
                }
            }
        }
    }
}