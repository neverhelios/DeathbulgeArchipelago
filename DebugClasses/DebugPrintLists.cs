using BepInEx;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient.DebugClasses;


class DebugPrintLists
{
    public static void PrintAllItems(bool bOnlyTreasure)
    {
        Plugin.Logger.LogInfo($"Time to print the item database baby !!!");
        List<Item> items = DialogueManager.instance.masterDatabase.items;
        foreach (Item item in items)
        {
            Item slot = DialogueManager.MasterDatabase.GetSlot(item, "TreasureSlot");
            if (bOnlyTreasure && slot == null)
            {
                // if (item.IsFieldAssigned("TreasureMoney"))
                // Plugin.Logger.LogInfo($"Money Item: {item.Name}");
                // Plugin.Logger.LogInfo($"LocationItem(\"{item.Name}\", \"Treasure Money({item.LookupInt("TreasureMoney")})\")");
                continue;
            }
            // Plugin.Logger.LogInfo($"Item: {item.Name} at slot {slot.Name}");
            // Plugin.Logger.LogInfo($"LocationItem(\"{item.Name}\", \"{slot.Name}\")");
            Plugin.Logger.LogInfo($"{{\"{slot.Name}\",\n'count': 1,\n'classification': ItemClassification.useful}},");
        }
    }

    static bool dumped = false;

    public static IEnumerator DumpDatabaseWhenReady()
    {
        while (DialogueManager.MasterDatabase == null)
            yield return null;

        if (dumped) yield break;
        dumped = true;

        var db = DialogueManager.MasterDatabase;

        var sb = new StringBuilder();

        sb.AppendLine("===== DIALOGUE DATABASE DUMP =====");

        // --- ITEMS ---
        sb.AppendLine("\n---- ITEMS ----");
        foreach (var item in db.items)
        {
            sb.AppendLine($"Item: {item.Name} (ID: {item.id})");

            foreach (var field in item.fields)
            {
                sb.AppendLine($"   {field.title} = {field.value}");
            }
        }

        // --- CONVERSATIONS ---
        sb.AppendLine("\n---- CONVERSATIONS ----");
        foreach (var conv in db.conversations)
        {
            sb.Append(StringifyConversation(conv, "  "));
        }

        // --- ACTORS ---
        sb.AppendLine("\n---- ACTORS ----");
        foreach (var actor in db.actors)
        {
            sb.AppendLine($"Actor: {actor.Name} (ID: {actor.id})");
        }

        // --- VARIABLES ---
        sb.AppendLine("\n---- VARIABLES ----");
        foreach (var variable in db.variables)
        {
            sb.AppendLine($"Variable: {variable.Name} = {variable.InitialValue}");
        }

        sb.AppendLine("\n===== END OF DUMP =====");

        // Chemin du fichier
        string path = Path.Combine(Paths.BepInExRootPath, "dialogue_db_dump.txt");

        File.WriteAllText(path, sb.ToString());

        Plugin.Logger.LogInfo($"Database dump écrit ici: {path}");
    }

    public static void PrintConversation(Conversation conv, string prefix, bool separateLines)
    {
        string conversationString = StringifyConversation(conv, prefix);
        if (!separateLines)
        {
            Plugin.Logger.LogInfo(conversationString);
            return;
        }

        string[] conversationEntryLines = conversationString.Split("\n");
        foreach (var line in conversationEntryLines)
            Plugin.Logger.LogInfo(line);
    }

    public static string StringifyConversation(Conversation conv, string prefix)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Conversation: {conv.Title} (ID: {conv.id})");
        foreach (var entry in conv.dialogueEntries)
        {
            sb.Append(StringifyDialogueEntry(entry, prefix));
        }
        return sb.ToString();
    }

    public static void PrintDialogueEntry(DialogueEntry entry, string prefix, bool separateLines)
    {
        string dialogueEntryString = StringifyDialogueEntry(entry, prefix);
        if (!separateLines)
        {
            Plugin.Logger.LogInfo(dialogueEntryString);
            return;
        }

        string[] dialogueEntryLines = dialogueEntryString.Split("\n");
        foreach (var line in dialogueEntryLines)
            Plugin.Logger.LogInfo(line);
    }

    public static string StringifyDialogueEntry(DialogueEntry entry, string prefix)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{prefix}-- Entry ID: {entry.id} --");
        sb.AppendLine($"{prefix}  Subtitle Text: {entry.subtitleText}");
        sb.AppendLine($"{prefix}  Sequence: {entry.Sequence}");
        sb.AppendLine($"{prefix}  Lua: {entry.userScript}");
        sb.AppendLine($"{prefix}  Conditions: {entry.conditionsString}");
        sb.AppendLine($"{prefix}  False Cond Action: {entry.falseConditionAction}");
        sb.AppendLine($"{prefix}  Is Group: {entry.isGroup} | Is Root: {entry.isRoot}");
        sb.AppendLine($"{prefix}  Actor ID: {entry.ActorID}");
        foreach (var link in entry.outgoingLinks)
        {
            sb.AppendLine($"{prefix}Link: {link.originConversationID}|{link.originDialogueID} -> {link.destinationConversationID}|{link.destinationDialogueID}");
        }
        return sb.ToString();
    }
}

class PrintAllSequencer_Patch
{
    static readonly FieldInfo shortcutsFieldInfo = typeof(SequencerShortcuts).GetField("shortcuts", BindingFlags.NonPublic | BindingFlags.Instance);
    [HarmonyPatch(typeof(SequencerShortcuts))]
    [HarmonyPatch("OnEnable")]
    [HarmonyPrefix]
    static void Prefix_ListAllSequencerShortcuts(SequencerShortcuts __instance)
    {
        Plugin.Logger.LogInfo($"There are {__instance.shortcuts.Length} shortcuts");

        // for (int i = 0; i < __instance.shortcuts.Length; i++)
        for (int i = 0; i < 10; i++)
        {
            Plugin.Logger.LogInfo($"\n\nShortcut {__instance.shortcuts[i].shortcut} :\n {__instance.shortcuts[i].value}");
        }
    }

}
