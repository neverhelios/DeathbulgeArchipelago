using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Combat.UI;
using Core;
using Field;
using Global.UI;
using HarmonyLib;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using UnityEngine;

namespace DeathbulgeArchipelagoClient.ItemsManagement;

class TreasureManager
{
    public static string SendCheckAndGetItem(string locationString, bool only_check = false)
    {
        ArchipelagoManager.instance.currSession?.Locations?.CompleteLocationChecks(ArchipelagoManager.instance.currSession?.Locations?.GetLocationIdFromName("Deathbulge", locationString) ?? -1);

        if (only_check)
            return "";

        if (ArchipelagoManager.instance.IsLocalLocation(locationString))
        {
            string itemName = ArchipelagoManager.instance.GetLocationItem(locationString);
            string treasureName = Items.GetTreasureFromItemName(itemName);
            Plugin.Logger.LogInfo($"======= The treasure get will should be {locationString} but it will be {treasureName}");
            return treasureName;
        }
        else
        {
            Plugin.Logger.LogInfo($"On va t'archipelaguer à coup de Archipelago Item - {locationString}");
            return $"Archipelago Item - {locationString}";
        }
    }

    // Send checks for classic treasures
    [HarmonyPatch(typeof(DialogueLua))]
    [HarmonyPatch("SetVariable", [typeof(string), typeof(object)])]
    [HarmonyPrefix]
    static bool Prefix_DialogueLua_SetVariable_SendTreasureCheck(string variable, object value)
    {
        if (variable == "Treasure.CurrentFlag")
        {
            Lua.WasInvoked = true;
            LuaTable luaTable = Lua.Environment.GetValue("Variable") as LuaTable;
            if (luaTable == null)
            {
                return false;
            }

            string locationString = LuaInterpreterExtensions.ObjectToLuaValue(value).ToString();
            luaTable.SetNameValue(DialogueLua.StringToTableIndex(variable), new LuaString(SendCheckAndGetItem(locationString)));
            return false;
        }
        return true;
    }

    // Detect the Archipelago handled beats
    [HarmonyPatch(typeof(CoreHelper))]
    [HarmonyPatch("HasBeatOrUpgradedVersion", [typeof(Item)])]
    [HarmonyPrefix]
    static bool HasBeatInArchipelagoLocationsChecked(ref bool __result, Item baseBeat)
    {

        string beatName = LuaInterpreterExtensions.ObjectToLuaValue(baseBeat.Name).ToString();
        string locationString = Items.GetTreasureFromItemName(beatName);

        // Not currently handled beat drop so let the function continue
        if (locationString == "NO LOCATION")
            return true;

        ILocationCheckHelper locations = ArchipelagoManager.instance.currSession?.Locations;
        if (locations == null) return true;

        long locationId = locations.GetLocationIdFromName("Deathbulge", locationString);
        __result = locations.AllLocationsChecked.Contains(locationId);

        return false;
    }


    [HarmonyPatch(typeof(VictoryWindow))]
    [HarmonyPatch("BeatDropPopupsAsync", [typeof(List<Item>)])]
    [HarmonyReversePatch]
    static IEnumerator OriginalBeatDropPopupsAsync(object instance, List<Item> beats)
        => throw new NotImplementedException();

    static bool _skipPatch = false;

    // Send checks for beats
    [HarmonyPatch(typeof(VictoryWindow))]
    [HarmonyPatch("BeatDropPopupsAsync", [typeof(List<Item>)])]
    [HarmonyPrefix]
    static bool Prefix_VictoryWindow_BeatDropPopupsAsync_SendTreasureCheck(VictoryWindow __instance, ref IEnumerator __result, List<Item> beats)
    {
        // Using HarmonyReversePatch had issues
        if (_skipPatch) return true;

        var remoteArchipelagoLocations = new List<string>();

        // Replace directly the item values
        for (int i = beats.Count - 1; i >= 0; i--)
        {
            string beatName = LuaInterpreterExtensions.ObjectToLuaValue(beats[i].Name).ToString();
            string locationString = Items.GetTreasureFromItemName(beatName);

            // Not currently handled beat drop
            if (locationString == "NO LOCATION")
                continue;

            SendCheckAndGetItem(locationString, true);
            string itemName = ArchipelagoManager.instance.GetLocationItem(locationString);

            Plugin.Logger.LogInfo($"\n\n\n+++++++++++++++ The beat drop name is {beatName}, so treasure {locationString} and will be replaced by {itemName} ++++++++++++++++++++\n\n\n");
            if (ArchipelagoManager.instance.IsLocalLocation(locationString))
            {
                beats[i] = DialogueManager.MasterDatabase.GetItem(itemName);
            }
            else
            {
                // Replace item bc it's archipelago remote
                remoteArchipelagoLocations.Add(locationString);
                beats.RemoveAt(i);
            }
        }

        __result = ShowRemoteArchipelagoTreasures(remoteArchipelagoLocations, beats);
        return false;


        IEnumerator ShowRemoteArchipelagoTreasures(List<string> remoteArchipelagoLocations, List<Item> beats)
        {
            TreasureUI uiTrea = CommonObjects.GetTreasureUI();
            Func<bool> waitWindowCache = null;
            foreach (string locationString in remoteArchipelagoLocations)
            {
                ShowArchipelagoItemPopup(locationString);
                Func<bool> func;
                if ((func = waitWindowCache) == null)
                {
                    func = (waitWindowCache = () => !uiTrea.window.activeInHierarchy);
                }
                yield return new WaitUntil(func);
            }

            Plugin.Logger.LogInfo($"Instance is {__instance} and beats are {beats}");

            // Avoid calling the patch recursively lol
            var main = AccessTools.Field(typeof(VictoryWindow), "main").GetValue(__instance);
            var mainMono = (MonoBehaviour)main;
            yield return mainMono.StartCoroutine(OriginalBeatDropPopupsAsync(__instance, beats));
            Plugin.Logger.LogInfo("Coroutine originale terminée");
        }
    }



    // Avoid spamming GetMethod 
    static readonly MethodInfo getParameterMethodInfo = typeof(SequencerCommandShowTreasure).GetMethod("GetParameter", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo stopMethodInfo = typeof(SequencerCommandShowTreasure).GetMethod("Stop", BindingFlags.NonPublic | BindingFlags.Instance);

    [HarmonyPatch(typeof(SequencerCommandShowTreasure))]
    [HarmonyPatch("Start")]
    [HarmonyPrefix]
    static bool Prefix_SequencerCommandShowTreasure_Start(SequencerCommandShowTreasure __instance)
    {
        string locationString = (string)getParameterMethodInfo.Invoke(__instance, [0, null]);
        // Show a popup for remote items instead of doing the whole Treasure Showing (of an item that doesn't even exist lol)
        if (locationString.Contains("Archipelago Item - "))
        {
            locationString = locationString.Replace("Archipelago Item - ", "");
            ShowArchipelagoItemPopup(locationString);
            stopMethodInfo.Invoke(__instance, []);
            return false;
        }
        return true;
    }

    static readonly MethodInfo getParameterSVMethodInfo = typeof(SequencerCommandSetVariableField).GetMethod("GetParameter", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo stopSVMethodInfo = typeof(SequencerCommandSetVariableField).GetMethod("Stop", BindingFlags.NonPublic | BindingFlags.Instance);
    [HarmonyPatch(typeof(SequencerCommandSetVariableField))]
    [HarmonyPatch("Awake")]
    [HarmonyPrefix]
    static bool Prefix_SequencerCommandSetVariableField_Awake(SequencerCommandSetVariableField __instance)
    {
        string parameter = (string)getParameterSVMethodInfo.Invoke(__instance, [0, null]);
        string parameter2 = (string)getParameterSVMethodInfo.Invoke(__instance, [1, null]);
        string articyField = (string)getParameterSVMethodInfo.Invoke(__instance, [2, null]);

        Plugin.Logger.LogInfo($"Set variable awake: {parameter} {parameter2} {articyField}");

        bool bIsArchipelagoItem = parameter2.Contains("Archipelago Item - ");

        if (bIsArchipelagoItem && parameter == "Treasure.Audio")
        {

            parameter2 = parameter2.Replace("Archipelago Item - ", "");
        }

        Item item = DialogueManager.MasterDatabase.GetItem(parameter2);
        FieldType type = item.fields.Find((PixelCrushers.DialogueSystem.Field f) => f.title == articyField).type;
        object obj;
        if (type != FieldType.Number)
        {
            if (type == FieldType.Boolean)
            {
                obj = item.LookupBool(articyField);
            }
            else
            {
                obj = item.LookupValue(articyField);
            }
        }
        else
        {
            obj = item.LookupInt(articyField);
        }
        DialogueLua.SetVariable(parameter, obj);
        stopSVMethodInfo.Invoke(__instance, []);
        return false;
    }

    private static void ShowArchipelagoItemPopup(string locationString)
    {
        ItemFlags itemFlags = ArchipelagoManager.instance.GetItemFlags(locationString);

        string itemTypeString;
        Sprite itemSprite = ResourcesLoader.GetSprite("archipelago.png");
        if ((itemFlags & ItemFlags.Advancement) != 0)
        {
            itemTypeString = "Progression";
            itemSprite = ResourcesLoader.GetSprite("archipelago_arrow_up.png");
        }
        else if ((itemFlags & ItemFlags.Trap) != 0)
        {
            itemTypeString = "Trap";
        }
        else if (itemFlags == 0)
        {
            itemTypeString = "Filler";
            itemSprite = ResourcesLoader.GetSprite("archipelago_grayscale.png");
        }
        else
        {
            itemTypeString = "Useful";
        }

        ShowTreasurePopup(ArchipelagoManager.instance.GetLocationItem(locationString), "Archipelago Item",
                          itemTypeString, $"A {itemTypeString} item for {ArchipelagoManager.instance.GetPlayerName(locationString)}", "", false, itemSprite);
    }

    public static void ShowTreasurePopup(string name, string title, string subtitle, string description, string charMod, bool activateCharMod, string treasureSprite)
    {
        ShowTreasurePopup(name, title, subtitle, description, charMod, activateCharMod, CommonObjects.GetGlobal().assetLoader.GetGlobalSprite(treasureSprite));
    }

    public static void ShowTreasurePopup(string name, string title, string subtitle, string description, string charMod, bool activateCharMod, Sprite treasureSprite)
    {
        TreasureUI treasureUI = CommonObjects.GetTreasureUI();
        Type treasureUIType = typeof(TreasureUI);

        FieldInfo continueButtonFieldInfo = treasureUIType.GetField("continueButton", BindingFlags.NonPublic | BindingFlags.Instance);
        ((Global.UI.StealFocus)continueButtonFieldInfo.GetValue(treasureUI)).enabled = false;
        treasureUI.window.SetActive(value: true);

        MethodInfo FocusContinueAfterDelayMethodInfo = treasureUIType.GetMethod("FocusContinueAfterDelay", BindingFlags.NonPublic | BindingFlags.Instance);
        treasureUI.StartCoroutine((IEnumerator)FocusContinueAfterDelayMethodInfo.Invoke(treasureUI, new object[] { }));


        ItemWindow itemWindow = treasureUI.itemWindow;
        UIHelper.SetText(itemWindow.gearName, name);
        UIHelper.SetText(itemWindow.gearType, title);
        Global.UI.UIHelper.SetText(treasureUI.tutorialText, subtitle);
        UIHelper.SetText(itemWindow.effectText, description);

        Type itemWindowType = typeof(ItemWindow);
        itemWindow.HideAllWindows();

        FieldInfo modEligibleCharNameFieldInfo = itemWindowType.GetField("modEligibleCharName", BindingFlags.NonPublic | BindingFlags.Instance);
        UIHelper.SetText((TMPro.TextMeshProUGUI)modEligibleCharNameFieldInfo.GetValue(itemWindow), charMod);
        FieldInfo eligibleCharContainerFieldInfo = itemWindowType.GetField("eligibleCharContainer", BindingFlags.NonPublic | BindingFlags.Instance);
        ((GameObject)eligibleCharContainerFieldInfo.GetValue(itemWindow)).SetActive(activateCharMod);

        UIHelper.SetImageSprite(treasureUI.itemWindow.gearIcon, treasureSprite);
    }
}