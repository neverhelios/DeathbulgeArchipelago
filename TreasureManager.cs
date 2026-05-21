using System;
using System.Collections;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using Field;
using Global.UI;
using HarmonyLib;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using UnityEngine;

namespace DeathbulgeArchipelagoClient;

class TreasureManager_Patch
{
    [HarmonyPatch(typeof(DialogueLua))]
    [HarmonyPatch("SetVariable", [typeof(string), typeof(object)])]
    [HarmonyPrefix]
    static bool Prefix_DialogueLua_SetVariable_SendTreasureCheck(string variable, object value)
    {
        if (variable == "Treasure.CurrentFlag")
        {
            string locationString = LuaInterpreterExtensions.ObjectToLuaValue(value).ToString();
            ArchipelagoManager.instance.currSession?.Locations?.CompleteLocationChecks(ArchipelagoManager.instance.currSession?.Locations?.GetLocationIdFromName("Deathbulge", locationString) ?? -1);

            Lua.WasInvoked = true;
            LuaTable luaTable = Lua.Environment.GetValue("Variable") as LuaTable;
            if (luaTable == null)
            {
                return false;
            }

            if (ArchipelagoManager.instance.IsLocalLocation(locationString))
            {
                string itemName = ArchipelagoManager.instance.GetLocationItem(locationString);
                string treasureName = Items.GetTreasureFromItemName(itemName);
                Plugin.Logger.LogInfo($"======= The treasure get will should be {LuaInterpreterExtensions.ObjectToLuaValue(value)} but it will be {treasureName}");
                luaTable.SetNameValue(DialogueLua.StringToTableIndex(variable), new LuaString(treasureName));
            }
            else
            {
                luaTable.SetNameValue(DialogueLua.StringToTableIndex(variable), new LuaString($"Archipelago Item - {locationString}"));
                Plugin.Logger.LogInfo($"On va t'archipelaguer à coup de Archipelago Item - {locationString}");

            }
            return false;
        }
        return true;
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

        if (locationString.Contains("Archipelago Item - "))
        {
            locationString = locationString.Replace("Archipelago Item - ", "");

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