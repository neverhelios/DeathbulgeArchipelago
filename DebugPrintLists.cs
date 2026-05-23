using BepInEx;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient;


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

    bool dumped = false;

    IEnumerator DumpDatabaseWhenReady()
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
        foreach (var convo in db.conversations)
        {
            sb.AppendLine($"Conversation: {convo.Title} (ID: {convo.id})");

            foreach (var entry in convo.dialogueEntries)
            {
                sb.AppendLine($"  Entry ID: {entry.id}");
                sb.AppendLine($"    Text: {entry.DialogueText}");
                sb.AppendLine($"    Sequence: {entry.Sequence}");
                sb.AppendLine($"    Lua: {entry.userScript}");
            }
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
