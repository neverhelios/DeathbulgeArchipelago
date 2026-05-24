using Archipelago.MultiClient.Net.Helpers;
using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient;

class LuaCatcher
{

    // Really bad way to patch lua but whatever, NO FUCKING INSTANCE OF Treasure.CurrentFlag WILL BE FORGOTTEN
    [HarmonyPatch(typeof(Lua), "Run", new[] { typeof(string), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    static bool PrefixLuaRunVener(ref string luaCode)
    {
        if (Plugin.logLuaCommmandsInterceptedConfig.Value)
            Plugin.Logger.LogInfo("Lua.Run intercepted VENER : " + luaCode);

        if (luaCode != null)
        {
            CatchVictoryCondition(ref luaCode);
            CatchTreasureCurrentFlag(ref luaCode);
            CatchTicketsSpawnCondition(ref luaCode);
        }
        return true;
    }

    static void CatchVictoryCondition(ref string luaCode)
    {
        // The line is there ONLY ONE TIME and I know exactly the line
        if (luaCode.Contains("Variable[\"BOTB.FinalKwakDefeated\"] = true;"))
        {
            ArchipelagoManager.instance.currSession?.SetGoalAchieved();
        }
    }

    static void CatchTreasureCurrentFlag(ref string luaCode)
    {
        if (luaCode.Contains("Variable[\"Treasure.CurrentFlag\"] =\""))
        {
            var lines = luaCode.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                // This has to be in the right line
                if (lines[i].Contains("Variable[\"Treasure.CurrentFlag\"] =\""))
                {
                    string locationString = lines[i].Split('"')[3];

                    lines[i] = $"Variable[\"Treasure.CurrentFlag\"] = \"{TreasureManager.SendCheckAndGetItem(locationString)}\"";

                    ILocationCheckHelper locations = ArchipelagoManager.instance.currSession?.Locations;
                    if (locations == null) continue;

                    locations.CompleteLocationChecks(locations.GetLocationIdFromName("Deathbulge", locationString));

                    if (ArchipelagoManager.instance.IsLocalLocation(locationString))
                    {
                        string itemName = ArchipelagoManager.instance.GetLocationItem(locationString);
                        string treasureName = Items.GetTreasureFromItemName(itemName);
                        Plugin.Logger.LogInfo($"======= The treasure get will should be {locationString} but it will be {treasureName}");
                        lines[i] = $"Variable[\"Treasure.CurrentFlag\"] = \"{treasureName}\"";
                    }
                    else
                    {
                        lines[i] = $"Variable[\"Treasure.CurrentFlag\"] = \"Archipelago Item - {locationString}\"";
                        Plugin.Logger.LogInfo($"On va t'archipelaguer à coup de Archipelago Item - {locationString}");
                    }
                }
            }
            luaCode = string.Join('\n', lines);
        }
    }

    static void CatchTicketsSpawnCondition(ref string luaCode)
    {
        // Sadly only the regexp can handle both
        // return HasItem("[Key Merch] Old Prize Draw Ticket 1")
        // and
        // return (HasItem("[Key Merch] Old Prize Draw Ticket 1")) == false

        if (luaCode.Contains("HasItem(\"[Key Merch] Old Prize Draw Ticket"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"HasItem\(""\[Key Merch\] Old Prize Draw Ticket (\d+)""\)",
                match =>
                {
                    string itemName = $"[Key Merch] Old Prize Draw Ticket {match.Groups[1].Value}";

                    string locationString = Items.GetTreasureFromItemName(itemName);

                    ILocationCheckHelper locations = ArchipelagoManager.instance.currSession?.Locations;
                    if (locations == null) return match.Value;

                    long locationId = locations.GetLocationIdFromName("Deathbulge", locationString);

                    bool isMissing = locations.AllMissingLocations.Contains(locationId);
                    return isMissing ? "false" : "true";
                }
            );
        }
    }
}