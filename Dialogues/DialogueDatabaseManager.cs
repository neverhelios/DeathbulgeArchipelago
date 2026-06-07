using System;
using System.Collections.Generic;
using System.Reflection;
using DeathbulgeArchipelagoClient.DebugClasses;
using HarmonyLib;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DeathbulgeArchipelagoClient.Dialogues;

class DialogueDatabaseManager
{

    static int startingConvOffset = -1;
    static int currentConvOffset = 0;

    static readonly Type dialogueDatabaseTypeInfo = typeof(DialogueDatabase);
    static readonly FieldInfo conversationTitleCacheFieldInfo = dialogueDatabaseTypeInfo.GetField("conversationTitleCache", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo addAssetsMethodInfo = dialogueDatabaseTypeInfo.GetMethod("AddAssets", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo addAssetsConversationMethodInfo = addAssetsMethodInfo.MakeGenericMethod(typeof(Conversation));
    [HarmonyPatch(typeof(DialogueDatabase))]
    [HarmonyPatch("Add")]
    [HarmonyPostfix]
    static void CatchDialogueDatabaseAdding(DialogueDatabase __instance)
    {
        List<Conversation> listConversations = new();

        // TODO: MAKE THIS MORE CLEAN
        Conversation getOutOfLabConv = new();
        getOutOfLabConv.fields = new List<PixelCrushers.DialogueSystem.Field>();
        getOutOfLabConv.Title = "Modded Dialogue/The Lab/The Loser Lounge/Loser couch - Get out of lab edition";
        getOutOfLabConv.id = GetHighestAvailableConversationID(__instance);

        getOutOfLabConv.dialogueEntries.Add(CreateDialogueEntry(__instance, 630, 6, getOutOfLabConv.id, 0, "NO ! LET ME GET THE FUCK OUT OF HERE", false, [new Link(getOutOfLabConv.id, 0, getOutOfLabConv.id, 1)]));
        getOutOfLabConv.dialogueEntries.Add(CreateDialogueEntry(__instance, 630, 6, getOutOfLabConv.id, 1, "Hello World 2 le retour", true, [new Link(getOutOfLabConv.id, 1, 768, 1)]));

        listConversations.Add(getOutOfLabConv);

        DebugPrintLists.PrintConversation(getOutOfLabConv, "  ", false);

        addAssetsConversationMethodInfo.Invoke(__instance, [__instance.conversations, listConversations, conversationTitleCacheFieldInfo.GetValue(__instance)]);
    }

    public static int GetModdedConvsBaseId()
    {
        return startingConvOffset + 1;
    }

    private static int GetHighestAvailableConversationID(DialogueDatabase dialogueDatabase)
    {
        if (startingConvOffset == -1)
        {
            int num = 0;
            foreach (Conversation dialogueEntry in dialogueDatabase.conversations)
            {
                num = Mathf.Max(dialogueEntry.id, num);
            }
            startingConvOffset = num;
        }
        currentConvOffset++;
        return startingConvOffset + currentConvOffset;
    }

    [HarmonyPatch(typeof(DialogueDatabase))]
    [HarmonyPatch("Clear")]
    [HarmonyPrefix]
    static bool CatchDialogueDatabaseClear(DialogueDatabase __instance)
    {
        Plugin.Logger.LogInfo("OOooo Clear dialogues oooOOO");
        startingConvOffset = -1;
        currentConvOffset = 0;
        return true;
    }

    public static DialogueEntry CreateDialogueEntry(DialogueDatabase currentDatabase, int sourceConversationID, int SourceDialogueID, int conversationID, int dialogueID, string dialogueText, bool isGroup, List<Link> outgoingLinks)
    {
        // I use this entry as a base, this might or might not be temporary
        DialogueEntry dialogueEntry = new(currentDatabase.GetDialogueEntry(sourceConversationID, SourceDialogueID))
        {
            conversationID = conversationID,
            id = dialogueID,
            isGroup = isGroup,
            outgoingLinks = outgoingLinks,
            DialogueText = dialogueText,
        };

        return dialogueEntry;
    }
}