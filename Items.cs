
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Field;
using PixelCrushers.DialogueSystem;
using Core;
using DeathbulgeArchipelagoClient;

class LocatedItem
{
    public string treasureLocationName;
    public string itemName;
    public LocatedItem(string treasureLocationName, string itemName)
    {
        this.treasureLocationName = treasureLocationName;
        this.itemName = itemName;
    }
}


class Items
{
    private static Random _random = new Random();

    public static KeyValuePair<string, string> GetRandomItem()
    {
        int countMoneyItems = moneyToTreasureNames?.Count ?? 0;
        int countTreasure = itemToTreasureNames?.Count ?? 0;

        int total = countMoneyItems + countTreasure;
        int index = _random.Next(total);

        if (index < countMoneyItems)
            return moneyToTreasureNames.ElementAt(index);

        return itemToTreasureNames.ElementAt(index - countMoneyItems);
    }

    public static string GetRandomItemName()
    {
        return GetRandomItem().Key;
    }

    public static string GetRandomTreasureLocation()
    {
        return GetRandomItem().Value;
    }


    public static Dictionary<string, string> GetItemTreasureNameDict()
    {
        return itemToTreasureNames;
    }

    public static string GetTreasureFromItemName(string itemName)
    {
        return itemToTreasureNames.GetValueOrDefault(itemName, "NO LOCATION");
    }

    static readonly Dictionary<string, string> itemToTreasureNames = new()
    {
        {"[Stock] Increase 1 (Tonewood01Treasure01)", "Tonewood01Treasure01"},
        {"[Patch] God Brain", "Tonewood02Treasure01"},
        {"[Merch] Emergency Horn", "Tonewood03Treasure01"},
        {"[Merch] Clearbuds", "Tonewood03Treasure02"},
        {"[Patch] HELF", "Tonewood04Treasure01"},
        {"[Patch] Fat Punch Five", "Tonewood06Treasure01"},
        {"[Key Merch] Shiny Spiky Thing", "Tonewood08Treasure01"},
        {"[Beat] Bansheebash", "[Treasure] TonewoodGig02-Shrubbanshee"},
        {"[Mod] Beefcake Bassquake", "Tonewood04Treasure02"},
        {"[Stock] Increase 1 (Tonewood06Treasure02)", "Tonewood06Treasure02"},
        {"[Key Merch] Your Inner Boot", "Tonewood08Treasure02"},
        {"[Mod] Crowdsurf", "Tonewood08Treasure03"},
        {"[Key Merch] Old Prize Draw Ticket 10", "[Treasure] PrizeTicketJim"},
        {"[Key Merch] Shiny Spiky Thing (Fork)", "[Treasure] GillianFork"},
        {"[Mod] Reckless Shredding", "Claire02Treasure01"},
        {"[Stock] Increase 1 (Claire06Treasure01)", "Claire06Treasure01"},
        {"[Patch] Beauty in Suffering", "Claire04Treasure01"},
        {"[Mod] Heartwarmer", "Claire04Treasure02"},
        {"[Mod] Intimidate", "Claire07Treasure01"},
        {"[Beat] Absolute Belter", "[Treasure] ClaireGig03-Madam"},
        {"[Patch] Luminous Kid", "ClaireLower01Treasure01"},
        {"[Stock] Increase 1 (Claire07Treasure02)", "Claire07Treasure02"},
        {"[Stock] Increase 1 (Claire05Treasure01)", "Claire05Treasure01"},
        {"[Stock] Increase 1 (ClaireLower02Treasure01)", "ClaireLower02Treasure01"},
        {"[Mod] The Great Unjoying", "ClaireLower03Treasure02"},
        {"[Stock] Increase 1 (ClaireLower04Treasure01)", "ClaireLower04Treasure01"},
        {"[Mod] Fresh Twist", "ClaireLower03Treasure01"},
        {"[Mod] Everybody Broken Bones", "ClaireLower03Treasure03"},
        {"[Merch] Flipped Cap", "ClaireLower04Treasure02"},
        {"[Key Merch] Old Prize Draw Ticket 4", "[Treasure] PrizeTicketMadam"},
        {"[Key Merch] Class Changer", "[Treasure] MODPODClass"},
        {"[Patch] Jen & The Regens", "[Treasure] PartyHouseReward"},
        {"[Patch] DJ Beatseek", "[Treasure] GeorgeousReward"},
        {"[Patch] Barry Club", "[Treasure] NelReward"},
        {"[Patch] Mega Def", "Bopstead01Treasure01"},
        {"[Mod] Erasure", "[Treasure] FoggyRewardPatch"},
        {"[Patch] BotB Patch", "[Treasure] DaemoPatch"},
        {"[Mod] GONG", "Bopstead03Treasure01"},
        {"[Key Merch] Barry's Tea Party", "Bopstead04Treasure01"},
        {"[Key Merch] Old Prize Draw Ticket 3", "[Treasure] PrizeTicketBarry"},
        {"[Key Merch] Old Prize Draw Ticket 2", "[Treasure] PrizeTicketBrioche"},
        {"[Key Merch] Old Prize Draw Ticket 1", "[Treasure] PrizeTicketPlat"},
        {"[Mod] Zero Hertz", "[Treasure] BasementGig02-Whale"},
        {"[Merch] Silenceblaster", "[Treasure] BasementGig04-Shutup"},
        {"[Stock] Increase 1 (Basement03Treasure01)", "Basement03Treasure01"},
        {"[Mod] Beefy Double", "Basement06Treasure01"},
        {"[Beat] Cuttlebuddy", "[Treasure] BasementGig05-Cuttle"},
        {"[Patch] Chuckridge Cuttlebrander", "[Treasure] BasementGig01-Cuttlebro"},
        {"[Stock] Increase 1 (Basement06Treasure02)", "Basement06Treasure02"},
        {"[Merch] The Y'almighty", "Basement06Treasure03"},
        {"[Mod] Heart Kickstart", "Basement04Treasure01"},
        {"[Stock] Increase 1 (Basement03Treasure02)", "Basement03Treasure02"},
        {"[Mod] Crashy Crescendo", "Basement03Treasure03"},
        {"[Mod] Bloody Hell", "Basement05Treasure01"},
        {"[Stock] Increase 1 (Basement02Treasure01)", "Basement02Treasure01"},
        {"[Mod] Whale Music", "Whale25Treasure"},
        {"[Patch] The NOW NOW NOWs", "Basement02Treasure02"},
        {"[Key Merch] Old Prize Draw Ticket 5", "[Treasure] PrizeTicketBase"},
        {"[Mod] Upbeat", "TheBus02Treasure01"},
        {"[Mod] Dazzling Shred", "TheBus05Treasure01"},
        {"[Stock] Increase 1 (TheBus05Treasure02)", "TheBus05Treasure02"},
        {"[Stock] Increase 1 (TheBus08Treasure02)", "TheBus08Treasure02"},
        {"[Patch] WELF", "TheBus08Treasure01"},
        {"[Mod] Let Me Try Something", "TheBus09Treasure01"},
        {"[Mod] Big Angry Riff of Fury", "TheBus10Treasure01"},
        {"[Key Merch] 16th Deck Keycard", "TheBus10Treasure02"},
        {"[Patch] Purple Lightning", "TheBus11Treasure01"},
        {"[Mod] Weaver of Darkness", "[Treasure] TheBusGig02-Weaver"},
        {"[Beat] Starstrike", "[Treasure] TheBusGig01-Glamourella"},
        {"[Mod] Surging Sorrow", "TheBus10Treasure03"},
        {"[Stock] Increase 1 (TheBus08Treasure03)", "TheBus08Treasure03"},
        {"[Mod] Shred of the Dead", "TheBus05Treasure03"},
        {"[Key Merch] Old Prize Draw Ticket 6", "[Treasure] PrizeTicketBus"},
        {"[Key Merch] 13th Deck Keycard", "[Treasure] 13DeckKeycard"},
        {"[Key Merch] 14th Deck Keycard", "[Treasure] 14DeckKeycard"},
        {"[Merch] Diva Music Box", "Lab03Treasure01"},
        {"[Patch] U WANT SUM", "Lab04Treasure01"},
        {"[Mod] Good Vibe Preservation", "Lab07Treasure01"},
        {"[Mod] Power Slide", "Lab08Treasure01"},
        {"[Beat] Makeshift Beat", "[Treasure] LabGig02-WIP"},
        {"[Stock] Increase 1 (Lab01Treasure01)", "Lab01Treasure01"},
        {"[Stock] Increase 1 (Lab04Treasure02)", "Lab04Treasure02"},
        {"[Stock] Increase 1 (Lab07Treasure02)", "Lab07Treasure02"},
        {"[Mod] Osculate in 7/8", "Lab09Treasure01"},
        {"[Mod] Robin Hunk Special", "Lab10Treasure01"},
        {"[Mod] Pyrotechnics", "Lab12Treasure01"},
        {"[Key Merch] Old Prize Draw Ticket 8", "[Treasure] PrizeTicketLab"},
        {"[Mod] ugh", "Lab10Treasure02"},
        {"[Key Merch] Glam Reader", "[Treasure] GlamReader"},
        {"[Beat] Rinna", "Hoho02Treasure01"},
        {"[Beat] RH", "Hoho02Treasure02"},
        {"[Mod] REMIX Briff", "Hoho02Treasure03"},
        {"[Patch] THREE BEAT MIX PATCH", "Hoho02Treasure04"},
        {"[Merch] Audio Greeting Card", "Hoho01Treasure01"},
        {"[Mod] Beam Team", "Hoho02Treasure05"},
        {"[Mod] Axe of Righteousness", "Hoho01Treasure02"},
        {"[Key Merch] Old Prize Draw Ticket 7", "[Treasure] PrizeTicketHoho"},
        {"[Patch] Recently Hatched", "Hoho02Treasure06"},
        {"[Stock] Increase 1 (Pokalyps01Treasure01)", "Pokalyps01Treasure01"},
        {"[Mod] Hypercussion", "Pokalyps02Treasure01"},
        {"[Mod] ALT_FX", "Pokalyps02Treasure02"},
        {"[Mod] Palm Destroyer", "Pokalyps04Treasure01"},
        {"[Merch] SIDD X-TR3M3", "Pokalyps05Treasure01"},
        {"[Mod] Colossal Cuss Out", "Pokalyps06Treasure01"},
        {"[Stock] Increase 1 (Pokalyps07Treasure01)", "Pokalyps07Treasure01"},
        {"[Mod] Vicious Sacrifice", "Pokalyps09Treasure01"},
        {"[Mod] BOOM.WAV", "Pokalyps05Treasure02"},
        {"[Mod] The Power of Friendship", "Pokalyps10Treasure01"},
        {"[Patch] Pokalyps", "Pokalyps01Treasure02"},
        {"[Key Merch] Old Prize Draw Ticket 9", "[Treasure] PrizeTicketPok"},
        {"[Mod] Graveyard Shuffle", "Dream03Treasure01"},
        {"[Mod] Dr. Tonebone", "Dream03Treasure02"},
        {"[Patch] Savant", "Dream03Treasure03"},
        {"[Mod] Eruptive Damnation", "Dream05Treasure01"},
        {"[Mod] Total Reinterpretation", "Dream04Treasure02"},
        {"[Stock] Increase 1 (Dream04Treasure01)", "Dream04Treasure01"},
        {"[Stock] Increase 1 (Dream04Treasure03)", "Dream04Treasure03"},
        {"Treasure Money", "[Treasure] FoggyRewardMoney"},
    };


    static readonly Dictionary<string, string> moneyToTreasureNames = new()
    {
        {"Treasure Money(8181)", "Tonewood08Treasure04"},
        { "Treasure Money(1000)", "Claire04Treasure03"},
        { "Treasure Money(2000)", "Claire04Treasure04"},
        { "Treasure Money(3000)", "Claire04Treasure05"},
        { "Treasure Money(5000)", "[Treasure] FoggyRewardMoney"},
        { "Treasure Money(300)", "[Treasure] Foggy300"},
        { "Treasure Money(15000)", "Bopstead01Treasure02"},
        { "Treasure Money(1111)", "Bopstead02Treasure01"},
        { "Treasure Money(7777)", "Bopstead02Treasure03"},
        { "Treasure Money(3333)", "Bopstead02Treasure02"},
        { "Treasure Money(9999)", "Bopstead02Treasure04"},
        { "Treasure Money(5)", "Bopstead01Treasure03"},
    };

}

// Make key items great again
class ItemsBehavior_Patch
{
    // Use the ClassChange 
    [HarmonyPatch(typeof(FieldMain))]
    [HarmonyPatch("CanChangeClassSlot")]
    [HarmonyPrefix]
    static bool FixClassChangeCondition(ref bool __result)
    {
        Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] Class Changer");
        __result = CoreHelper.HasItem(item);
        return false;
    }


    // Use the GlamBar
    [HarmonyPatch(typeof(Party))]
    [HarmonyPatch("IsGlamUnlocked")]
    [HarmonyPrefix]
    static bool FixGlamUnlockedCondition(ref bool __result)
    {
        Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] Glam Reader");
        __result = CoreHelper.HasItem(item);
        Plugin.Logger.LogInfo($"++++++++++++++++++++++++++ I have the item {__result} ++++++++++++++++++++++++++");
        return false;
    }

    [HarmonyPatch(typeof(Party))]
    [HarmonyPatch("IsPerformanceUnlocked")]
    [HarmonyPrefix]
    static bool FixPerformanceUnlockedCondition(ref bool __result)
    {

        Item item = DialogueManager.MasterDatabase.GetItem("[Key Merch] Glam Reader");
        __result = CoreHelper.HasItem(item);
        if (__result)
            return false;
        return true;
    }
}

