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
            catchVictoryCondition(ref luaCode);

            catchTreasureCurrentFlag(ref luaCode);


        }
        return true;
    }

    static void catchVictoryCondition(ref string luaCode)
    {
        if (luaCode.Contains("Variable[\"BOTB.FinalKwakDefeated\"] = true;"))
        {
            ArchipelagoManager.instance.currSession?.SetGoalAchieved();
        }
    }

    static void catchTreasureCurrentFlag(ref string luaCode)
    {
        if (luaCode.Contains("Variable[\"Treasure.CurrentFlag\"] =\""))
        {
            var lines = luaCode.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Treasure.CurrentFlag"))
                {
                    string locationString = lines[i].Split('"')[3];

                    lines[i] = $"Variable[\"Treasure.CurrentFlag\"] = \"{TreasureManager.SendCheckAndGetItem(locationString)}\"";

                    ArchipelagoManager.instance.currSession?.Locations?.CompleteLocationChecks(ArchipelagoManager.instance.currSession?.Locations?.GetLocationIdFromName("Deathbulge", locationString) ?? -1);

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
}