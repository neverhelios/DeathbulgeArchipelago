using Archipelago.MultiClient.Net.Helpers;
using Core;
using HarmonyLib;
using PixelCrushers.DialogueSystem;
using DeathbulgeArchipelagoClient.ItemsManagement;

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
            CatchGillianGigCondition(ref luaCode);
            CatchPriceDrawCount(ref luaCode);
            CatchInnerBootCondition(ref luaCode);
            CatchBusKeycardsCondition(ref luaCode);
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

                    lines[i] = $"Variable[\"Treasure.CurrentFlag\"] = \"{Items.SendCheckAndGetItem(locationString)}\"";

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

    static void CatchGillianGigCondition(ref string luaCode)
    {
        // I'm going the regexp way and you can't stop me
        if (luaCode.Contains("Variable[\"SideGigTonewood01.State\"] < 3"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""SideGigTonewood01\.State""\] < 3",
                match =>
                {
                    Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] Shiny Spiky Thing");
                    return CoreHelper.HasItem(item) ? "false" : "true";
                }
            );
            Plugin.Logger.LogInfo($"GILLIAN, YOU'RE NOW {luaCode}");
        }
    }

    static void CatchPriceDrawCount(ref string luaCode)
    {
        // Catch comparison
        if (luaCode.Contains("Variable[\"Common.PrizeDrawCount\"] <"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Common\.PrizeDrawCount""\] <",
                match =>
                {
                    int numberOfTickets = Items.CountAllPrizeTicketsGot();
                    return $"{numberOfTickets} <";
                }
            );
        }

        // Catch calculation
        if (luaCode.Contains("Variable[\"Common.PrizeDrawCount\"] - Variable[\"Common.PrizeDrawRedeem\"]"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Common\.PrizeDrawCount""\] - Variable\[""Common\.PrizeDrawRedeem""\]",
                match =>
                {
                    int numberOfTickets = Items.CountAllPrizeTicketsGot();
                    return $"{numberOfTickets} - Variable[\"Common.PrizeDrawRedeem\"]";
                }
            );
        }
    }

    static void CatchInnerBootCondition(ref string luaCode)
    {
        // Maybe this first one is useless
        if (luaCode.Contains("Variable[\"Common.DoorkickLevel\"] >= 3"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Common\.DoorkickLevel""\] >= 3",
                match =>
                {
                    Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] Your Inner Boot");
                    return CoreHelper.HasItem(item) ? "true" : "false";
                }
            );
        }

        if (luaCode.Contains("Variable[\"Tonewood.JimTalked\"] > 0"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Tonewood\.JimTalked""\] > 0",
                match =>
                {
                    return "true";
                }
            );
        }
    }

    static void CatchBusKeycardsCondition(ref string luaCode)
    {
        if (luaCode.Contains("Variable[\"Bus.ElevatorF14\"]  and"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Bus\.ElevatorF14""\]  and",
                match =>
                {
                    Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] 14th Deck Keycard");
                    return CoreHelper.HasItem(item) ? "true  and" : "false  and";
                }
            );
        }

        if (luaCode.Contains("Variable[\"Bus.ElevatorF13\"]  and"))
        {
            luaCode = System.Text.RegularExpressions.Regex.Replace(
                luaCode,
                @"Variable\[""Bus\.ElevatorF13""\]  and",
                match =>
                {
                    Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] 13th Deck Keycard");
                    return CoreHelper.HasItem(item) ? "true  and" : "false  and";
                }
            );
        }
    }


}