using Field;
using HarmonyLib;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;

public class DialogueCatcher : MonoBehaviour
{
    [HarmonyPatch(typeof(DialogueSystemController))]
    [HarmonyPatch("StartConversation", [typeof(string), typeof(int)])]
    [HarmonyPrefix]
    static void Prefix_StartConversation(string title, int initialDialogueEntryID)
    {
        if (Plugin.logDialogueConfig.Value)
            Plugin.Logger.LogInfo($"######################## START DIALOGUE CONVERSATION STATE {title}");
    }

    [HarmonyPatch(typeof(MapWarp))]
    [HarmonyPatch("PrimeWarp")]
    [HarmonyPostfix]
    static void PrimeWarpLogger()
    {
        if (Plugin.logWarpConfig.Value)
        {
            Plugin.Logger.LogInfo($"Je me prime le warp");
            Plugin.Logger.LogInfo($"-> WarpDestinationArea: {DialogueLua.GetVariable("WarpDestinationArea").AsString}");
            Plugin.Logger.LogInfo($"-> WarpDestinationMap: {DialogueLua.GetVariable("Common.WarpDestinationMap").AsString}");
            Plugin.Logger.LogInfo($"-> WarpDestinationObject: {DialogueLua.GetVariable("Common.WarpDestinationObject").AsString}");
            Plugin.Logger.LogInfo($"-> WarpChangeFacing: {DialogueLua.GetVariable("Common.WarpChangeFacing").AsString}");
            Plugin.Logger.LogInfo($"-> WarpFacing: {DialogueLua.GetVariable("Common.WarpFacing").AsString}");
            Plugin.Logger.LogInfo($"-> WarpIndoors: {DialogueLua.GetVariable("Common.WarpIndoors").AsString}");
            Plugin.Logger.LogInfo($"-> WarpIndoorObject: {DialogueLua.GetVariable("Common.WarpIndoorObject").AsString}");
            Plugin.Logger.LogInfo($"-> WarpIndoorRoomObject: {DialogueLua.GetVariable("Common.WarpIndoorRoomObject").AsString}");
            Plugin.Logger.LogInfo($"-> WarpDestinationLayer: {DialogueLua.GetVariable("Common.WarpDestinationLayer").AsString}");
            Plugin.Logger.LogInfo($"-> WarpDestinationSortLayer: {DialogueLua.GetVariable("Common.WarpDestinationSortLayer").AsString}");
            Plugin.Logger.LogInfo($"-> WarpFadeOut: {DialogueLua.GetVariable("Common.WarpFadeOut").AsString}");
            Plugin.Logger.LogInfo($"-> WarpAutosave: {DialogueLua.GetVariable("Common.WarpAutosave").AsString}");
            Plugin.Logger.LogInfo($"-> LastWarpDestination: {DialogueLua.GetVariable("Common.LastWarpDestination").AsString}");
            Plugin.Logger.LogInfo($"-> CurrentArea: {DialogueLua.GetVariable("Common.CurrentArea").AsString}");
            Plugin.Logger.LogInfo($"-> CurrentMap: {DialogueLua.GetVariable("Common.CurrentMap").AsString}");
            // CommonObjects.GetFieldMain().mapLoader.PreviousArea = DialogueLua.GetVariable("Common.CurrentArea").AsString;
        }
    }

    [HarmonyPatch(typeof(ConversationModel))]
    [HarmonyPatch("GetState", [typeof(DialogueEntry), typeof(bool), typeof(bool), typeof(bool)])]
    [HarmonyPrefix]
    static void Prefix_GetState(DialogueEntry entry, bool includeLinks, bool stopAtFirstValid = false, bool skipExecution = false)
    {
        if (Plugin.logDialogueConfig.Value)
            Plugin.Logger.LogInfo($"[CONVERSATION MODEL STATE {entry.conversationID}|{entry.id}]");
    }

    [HarmonyPatch(typeof(ConversationState))]
    [HarmonyPatch(MethodType.Constructor, [typeof(Subtitle), typeof(Response[]), typeof(Response[]), typeof(bool)])]
    [HarmonyPrefix]
    static bool Prefix_ConversationState(ref Subtitle subtitle, Response[] npcResponses, Response[] pcResponses, bool isGroup = false)
    {

        if (Plugin.logDialogueConfig.Value)
        {
            Plugin.Logger.LogInfo($"- THE STATE WILL SHOW \"{subtitle.dialogueEntry.subtitleText}\" -");
            Plugin.Logger.LogInfo($"- AND HAVE {npcResponses.Length} NPC Responses AND  {pcResponses.Length} PC Responses -");
            foreach (var response in npcResponses)
            {
                Plugin.Logger.LogInfo($"- NPC DESTINATION ENTRY IS {response.destinationEntry.conversationID}|{response.destinationEntry.id}");
            }
            foreach (var response in pcResponses)
            {
                Plugin.Logger.LogInfo($"- PC DESTINATION ENTRY IS {response.destinationEntry.conversationID}|{response.destinationEntry.id}");
            }
        }

        // Entry manipulation
        if (subtitle.dialogueEntry.conversationID == 571 && subtitle.dialogueEntry.id == 10)
        {
            subtitle.formattedText.text = "Well the door is open, why not get out of the bus ?";
        }


        return true;
    }

    static readonly FieldInfo databaseFieldInfo = typeof(ConversationModel).GetField("m_database", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo evaluateLinksMethodInfo = typeof(ConversationModel).GetMethod("EvaluateLinksAtPriority", BindingFlags.NonPublic | BindingFlags.Instance);
    [HarmonyPatch(typeof(ConversationModel))]
    [HarmonyPatch("EvaluateLinksAtPriority")]
    [HarmonyPrefix]
    static bool Prefix_EvaluateLinks(ConversationModel __instance, ConditionPriority priority, DialogueEntry entry, List<Response> npcResponses, List<Response> pcResponses, List<DialogueEntry> visited)
    {
        bool bHasLinkPossible = false;
        foreach (var link in entry.outgoingLinks)
        {
            if (link.priority == priority)
            {
                bHasLinkPossible = true;
            }
        }
        if (bHasLinkPossible)
        {
            // TODO: Do only one if by subscribing to a force link dictionnary ad searching the entries here
            // Link manipulation
            if (entry.conversationID == 571 && entry.id == 4)
            {
                Plugin.Logger.LogInfo($"____________________ I FORCE YOU YOU NOT SPEAK TO COALED ____________________");
                ForceLink(__instance, entry.outgoingLinks[0], npcResponses, pcResponses, visited);
                return false;
            }

            if (entry.conversationID == 571 && entry.id == 10)
            {
                Plugin.Logger.LogInfo($"____________________ GET OUT OF MY BUS ____________________");
                WarpManager.CustomPrimeWarp("BopsteadBusStop");
                ForceLink(__instance, 768, 1, npcResponses, pcResponses, visited);
                return false;
            }


            if (entry.conversationID == 578 && entry.id == 46)
            {
                // Skip link 38
                Plugin.Logger.LogInfo($"____________________ DO NOT CLOSE THE BUS UPPER DOOR ____________________");
                ForceLink(__instance, 578, 34, npcResponses, pcResponses, visited);
                return false;
            }

            if (Plugin.logDialogueConfig.Value)
            {
                Plugin.Logger.LogInfo($"");
                Plugin.Logger.LogInfo($">>>>>>>>>>>>>>>>>>>>>>>> CONVERSATION LINE {entry.conversationID}|{entry.id}");
                if (entry.DialogueText != "")
                    Plugin.Logger.LogInfo($"Text: {entry.DialogueText}");
                if (entry.Sequence != "")
                    Plugin.Logger.LogInfo($"Sequence: {entry.Sequence}");
                if (entry.userScript != "")
                    Plugin.Logger.LogInfo($"Lua: {entry.userScript}\n");
                if (entry.conditionsString != "")
                    Plugin.Logger.LogInfo($"Condition: {entry.conditionsString}\n");
                Plugin.Logger.LogInfo($"IsGroup: {entry.isGroup} | IsRoot: {entry.isRoot}\n");

                foreach (var link in entry.outgoingLinks)
                {
                    if (link.priority == priority)
                    {
                        Plugin.Logger.LogInfo($"[Evaluate link] for {entry.conversationID}|{entry.id}");
                        DialogueEntry evaluatedDialogueEntry = ((DialogueDatabase)databaseFieldInfo.GetValue(__instance)).GetDialogueEntry(link);
                        if (evaluatedDialogueEntry.conditionsString != "" || evaluatedDialogueEntry.falseConditionAction != "")
                            Plugin.Logger.LogInfo($"Dialogue conditions: {evaluatedDialogueEntry.conditionsString} -> {evaluatedDialogueEntry.falseConditionAction}");
                        Plugin.Logger.LogInfo($"Link: {link.originConversationID}|{link.originDialogueID} -> {link.destinationConversationID}|{link.destinationDialogueID}");
                    }
                }
            }
        }
        return true;
    }

    static private void ForceLink(ConversationModel __instance, int conversationID, int dialogueEntryId, List<Response> npcResponses, List<Response> pcResponses, List<DialogueEntry> visited)
    {
        Link tpOutofBusLink = new(0, 0, conversationID, dialogueEntryId);
        ForceLink(__instance, tpOutofBusLink, npcResponses, pcResponses, visited);
    }

    static private void ForceLink(ConversationModel __instance, Link link, List<Response> npcResponses, List<Response> pcResponses, List<DialogueEntry> visited)
    {
        DialogueEntry evaluatedDialogueEntry = ((DialogueDatabase)databaseFieldInfo.GetValue(__instance)).GetDialogueEntry(link);
        Lua.Run(evaluatedDialogueEntry.userScript, DialogueDebug.logInfo, true);
        evaluatedDialogueEntry.onExecute.Invoke();
        for (int j = 4; j >= 0; j--)
        {
            int num = npcResponses.Count + pcResponses.Count;
            evaluateLinksMethodInfo.Invoke(__instance, [(ConditionPriority)j, evaluatedDialogueEntry, npcResponses, pcResponses, visited, false]);

            if (npcResponses.Count + pcResponses.Count > num)
            {
                break;
            }
        }
    }

}

// Legacy logger
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
                if (entry.Sequence.ToString() == "Continue()" && entry.userScript == "" && entry.DialogueText == "" && entry.conditionsString == "")
                {
                    Plugin.Logger.LogInfo($"-- Entry {entry.id} -> Empty");
                }
                else
                {
                    Plugin.Logger.LogInfo($"-- Entry {entry.id}: ");
                    if (entry.DialogueText != "")
                        Plugin.Logger.LogInfo($"Text: {entry.DialogueText}");
                    if (entry.Sequence != "")
                        Plugin.Logger.LogInfo($"Sequence: {entry.Sequence}");
                    if (entry.userScript != "")
                        Plugin.Logger.LogInfo($"Lua: {entry.userScript}\n");
                    if (entry.conditionsString != "")
                        Plugin.Logger.LogInfo($"Condition: {entry.conditionsString}\n");
                    foreach (var link in entry.outgoingLinks)
                    {
                        Plugin.Logger.LogInfo($"Link: {link.originConversationID}|{link.originDialogueID} -> {link.destinationConversationID}|{link.destinationDialogueID}");
                    }

                }
            }
        }
    }
}