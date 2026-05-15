
using System;
using System.Collections.Generic;

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

    public static LocatedItem GetRandomItem()
    {
        int countMoneyItems = itemMoneyNames?.Count ?? 0;
        int countTreasure = itemTreasureNames?.Count ?? 0;

        int total = countMoneyItems + countTreasure;
        int index = _random.Next(total);

        if (index < countMoneyItems)
            return itemMoneyNames[index];

        return itemTreasureNames[index - countMoneyItems];
    }

    public static string GetRandomItemName()
    {
        return GetRandomItem().itemName;
    }

    public static string GetRandomTreasureLocation()
    {
        return GetRandomItem().treasureLocationName;
    }


    public static List<LocatedItem> GetItemTreasureNameList()
    {
        return itemTreasureNames;
    }

    static List<LocatedItem> itemTreasureNames = new List<LocatedItem> {
        new LocatedItem("Tonewood01Treasure01", "[Stock] Increase 1 (Tonewood01Treasure01)"),
        new LocatedItem("Tonewood02Treasure01", "[Patch] God Brain"),
        new LocatedItem("Tonewood03Treasure01", "[Merch] Emergency Horn"),
        new LocatedItem("Tonewood03Treasure02", "[Merch] Clearbuds"),
        new LocatedItem("Tonewood04Treasure01", "[Patch] HELF"),
        new LocatedItem("Tonewood06Treasure01", "[Patch] Fat Punch Five"),
        new LocatedItem("Tonewood08Treasure01", "[Key Merch] Shiny Spiky Thing"),
        new LocatedItem("[Treasure] TonewoodGig02-Shrubbanshee", "[Beat] Bansheebash"),
        new LocatedItem("Tonewood04Treasure02", "[Mod] Beefcake Bassquake"),
        new LocatedItem("Tonewood06Treasure02", "[Stock] Increase 1 (Tonewood06Treasure02)"),
        new LocatedItem("Tonewood08Treasure02", "[Key Merch] Your Inner Boot"),
        new LocatedItem("Tonewood08Treasure03", "[Mod] Crowdsurf"),
        new LocatedItem("[Treasure] PrizeTicketJim", "[Key Merch] Old Prize Draw Ticket 10"),
        new LocatedItem("[Treasure] GillianFork", "[Key Merch] Shiny Spiky Thing (Fork)"),
        new LocatedItem("Claire02Treasure01", "[Mod] Reckless Shredding"),
        new LocatedItem("Claire06Treasure01", "[Stock] Increase 1 (Claire06Treasure01)"),
        new LocatedItem("Claire04Treasure01", "[Patch] Beauty in Suffering"),
        new LocatedItem("Claire04Treasure02", "[Mod] Heartwarmer"),
        new LocatedItem("Claire07Treasure01", "[Mod] Intimidate"),
        new LocatedItem("[Treasure] ClaireGig03-Madam", "[Beat] Absolute Belter"),
        new LocatedItem("ClaireLower01Treasure01", "[Patch] Luminous Kid"),
        new LocatedItem("Claire07Treasure02", "[Stock] Increase 1 (Claire07Treasure02)"),
        new LocatedItem("Claire05Treasure01", "[Stock] Increase 1 (Claire05Treasure01)"),
        new LocatedItem("ClaireLower02Treasure01", "[Stock] Increase 1 (ClaireLower02Treasure01)"),
        new LocatedItem("ClaireLower03Treasure02", "[Mod] The Great Unjoying"),
        new LocatedItem("ClaireLower04Treasure01", "[Stock] Increase 1 (ClaireLower04Treasure01)"),
        new LocatedItem("ClaireLower03Treasure01", "[Mod] Fresh Twist"),
        new LocatedItem("ClaireLower03Treasure03", "[Mod] Everybody Broken Bones"),
        new LocatedItem("ClaireLower04Treasure02", "[Merch] Flipped Cap"),
        new LocatedItem("[Treasure] PrizeTicketMadam", "[Key Merch] Old Prize Draw Ticket 4"),
        new LocatedItem("[Treasure] MODPODClass", "[Key Merch] Class Changer"),
        new LocatedItem("[Treasure] PartyHouseReward", "[Patch] Jen & The Regens"),
        new LocatedItem("[Treasure] GeorgeousReward", "[Patch] DJ Beatseek"),
        new LocatedItem("[Treasure] NelReward", "[Patch] Barry Club"),
        new LocatedItem("Bopstead01Treasure01", "[Patch] Mega Def"),
        new LocatedItem("[Treasure] FoggyRewardPatch", "[Mod] Erasure"),
        new LocatedItem("[Treasure] DaemoPatch", "[Patch] BotB Patch"),
        new LocatedItem("Bopstead03Treasure01", "[Mod] GONG"),
        new LocatedItem("Bopstead04Treasure01", "[Key Merch] Barry's Tea Party"),
        new LocatedItem("[Treasure] PrizeTicketBarry", "[Key Merch] Old Prize Draw Ticket 3"),
        new LocatedItem("[Treasure] PrizeTicketBrioche", "[Key Merch] Old Prize Draw Ticket 2"),
        new LocatedItem("[Treasure] PrizeTicketPlat", "[Key Merch] Old Prize Draw Ticket 1"),
        new LocatedItem("[Treasure] BasementGig02-Whale", "[Mod] Zero Hertz"),
        new LocatedItem("[Treasure] BasementGig04-Shutup", "[Merch] Silenceblaster"),
        new LocatedItem("Basement03Treasure01", "[Stock] Increase 1 (Basement03Treasure01)"),
        new LocatedItem("Basement06Treasure01", "[Mod] Beefy Double"),
        new LocatedItem("[Treasure] BasementGig05-Cuttle", "[Beat] Cuttlebuddy"),
        new LocatedItem("[Treasure] BasementGig01-Cuttlebro", "[Patch] Chuckridge Cuttlebrander"),
        new LocatedItem("Basement06Treasure02", "[Stock] Increase 1 (Basement06Treasure02)"),
        new LocatedItem("Basement06Treasure03", "[Merch] The Y'almighty"),
        new LocatedItem("Basement04Treasure01", "[Mod] Heart Kickstart"),
        new LocatedItem("Basement03Treasure02", "[Stock] Increase 1 (Basement03Treasure02)"),
        new LocatedItem("Basement03Treasure03", "[Mod] Crashy Crescendo"),
        new LocatedItem("Basement05Treasure01", "[Mod] Bloody Hell"),
        new LocatedItem("Basement02Treasure01", "[Stock] Increase 1 (Basement02Treasure01)"),
        new LocatedItem("Whale25Treasure", "[Mod] Whale Music"),
        new LocatedItem("Basement02Treasure02", "[Patch] The NOW NOW NOWs"),
        new LocatedItem("[Treasure] PrizeTicketBase", "[Key Merch] Old Prize Draw Ticket 5"),
        new LocatedItem("TheBus02Treasure01", "[Mod] Upbeat"),
        new LocatedItem("TheBus05Treasure01", "[Mod] Dazzling Shred"),
        new LocatedItem("TheBus05Treasure02", "[Stock] Increase 1 (TheBus05Treasure02)"),
        new LocatedItem("TheBus08Treasure02", "[Stock] Increase 1 (TheBus08Treasure02)"),
        new LocatedItem("TheBus08Treasure01", "[Patch] WELF"),
        new LocatedItem("TheBus09Treasure01", "[Mod] Let Me Try Something"),
        new LocatedItem("TheBus10Treasure01", "[Mod] Big Angry Riff of Fury"),
        new LocatedItem("TheBus10Treasure02", "[Key Merch] 16th Deck Keycard"),
        new LocatedItem("TheBus11Treasure01", "[Patch] Purple Lightning"),
        new LocatedItem("[Treasure] TheBusGig02-Weaver", "[Mod] Weaver of Darkness"),
        new LocatedItem("[Treasure] TheBusGig01-Glamourella", "[Beat] Starstrike"),
        new LocatedItem("TheBus10Treasure03", "[Mod] Surging Sorrow"),
        new LocatedItem("TheBus08Treasure03", "[Stock] Increase 1 (TheBus08Treasure03)"),
        new LocatedItem("TheBus05Treasure03", "[Mod] Shred of the Dead"),
        new LocatedItem("[Treasure] PrizeTicketBus", "[Key Merch] Old Prize Draw Ticket 6"),
        new LocatedItem("[Treasure] 13DeckKeycard", "[Key Merch] 13th Deck Keycard"),
        new LocatedItem("[Treasure] 14DeckKeycard", "[Key Merch] 14th Deck Keycard"),
        new LocatedItem("Lab03Treasure01", "[Merch] Diva Music Box"),
        new LocatedItem("Lab04Treasure01", "[Patch] U WANT SUM"),
        new LocatedItem("Lab07Treasure01", "[Mod] Good Vibe Preservation"),
        new LocatedItem("Lab08Treasure01", "[Mod] Power Slide"),
        new LocatedItem("[Treasure] LabGig02-WIP", "[Beat] Makeshift Beat"),
        new LocatedItem("Lab01Treasure01", "[Stock] Increase 1 (Lab01Treasure01)"),
        new LocatedItem("Lab04Treasure02", "[Stock] Increase 1 (Lab04Treasure02)"),
        new LocatedItem("Lab07Treasure02", "[Stock] Increase 1 (Lab07Treasure02)"),
        new LocatedItem("Lab09Treasure01", "[Mod] Osculate in 7/8"),
        new LocatedItem("Lab10Treasure01", "[Mod] Robin Hunk Special"),
        new LocatedItem("Lab12Treasure01", "[Mod] Pyrotechnics"),
        new LocatedItem("[Treasure] PrizeTicketLab", "[Key Merch] Old Prize Draw Ticket 8"),
        new LocatedItem("Lab10Treasure02", "[Mod] ugh"),
        new LocatedItem("[Treasure] GlamReader", "[Key Merch] Glam Reader"),
        new LocatedItem("Hoho02Treasure01", "[Beat] Rinna"),
        new LocatedItem("Hoho02Treasure02", "[Beat] RH"),
        new LocatedItem("Hoho02Treasure03", "[Mod] REMIX Briff"),
        new LocatedItem("Hoho02Treasure04", "[Patch] THREE BEAT MIX PATCH"),
        new LocatedItem("Hoho01Treasure01", "[Merch] Audio Greeting Card"),
        new LocatedItem("Hoho02Treasure05", "[Mod] Beam Team"),
        new LocatedItem("Hoho01Treasure02", "[Mod] Axe of Righteousness"),
        new LocatedItem("[Treasure] PrizeTicketHoho", "[Key Merch] Old Prize Draw Ticket 7"),
        new LocatedItem("Hoho02Treasure06", "[Patch] Recently Hatched"),
        new LocatedItem("Pokalyps01Treasure01", "[Stock] Increase 1 (Pokalyps01Treasure01)"),
        new LocatedItem("Pokalyps02Treasure01", "[Mod] Hypercussion"),
        new LocatedItem("Pokalyps02Treasure02", "[Mod] ALT_FX"),
        new LocatedItem("Pokalyps04Treasure01", "[Mod] Palm Destroyer"),
        new LocatedItem("Pokalyps05Treasure01", "[Merch] SIDD X-TR3M3"),
        new LocatedItem("Pokalyps06Treasure01", "[Mod] Colossal Cuss Out"),
        new LocatedItem("Pokalyps07Treasure01", "[Stock] Increase 1 (Pokalyps07Treasure01)"),
        new LocatedItem("Pokalyps09Treasure01", "[Mod] Vicious Sacrifice"),
        new LocatedItem("Pokalyps05Treasure02", "[Mod] BOOM.WAV"),
        new LocatedItem("Pokalyps10Treasure01", "[Mod] The Power of Friendship"),
        new LocatedItem("Pokalyps01Treasure02", "[Patch] Pokalyps"),
        new LocatedItem("[Treasure] PrizeTicketPok", "[Key Merch] Old Prize Draw Ticket 9"),
        new LocatedItem("Dream03Treasure01", "[Mod] Graveyard Shuffle"),
        new LocatedItem("Dream03Treasure02", "[Mod] Dr. Tonebone"),
        new LocatedItem("Dream03Treasure03", "[Patch] Savant"),
        new LocatedItem("Dream05Treasure01", "[Mod] Eruptive Damnation"),
        new LocatedItem("Dream04Treasure02", "[Mod] Total Reinterpretation"),
        new LocatedItem("Dream04Treasure01", "[Stock] Increase 1 (Dream04Treasure01)"),
        new LocatedItem("Dream04Treasure03", "[Stock] Increase 1 (Dream04Treasure03)")
    };


    static List<LocatedItem> itemMoneyNames = new List<LocatedItem> {
        new LocatedItem("Tonewood08Treasure04", "Treasure Money(8181)"),
        new LocatedItem("Claire04Treasure03", "Treasure Money(1000)"),
        new LocatedItem("Claire04Treasure04", "Treasure Money(2000)"),
        new LocatedItem("Claire04Treasure05", "Treasure Money(3000)"),
        new LocatedItem("[Treasure] FoggyRewardMoney", "Treasure Money(5000)"),
        new LocatedItem("[Treasure] Foggy300", "Treasure Money(300)"),
        new LocatedItem("Bopstead01Treasure02", "Treasure Money(15000)"),
        new LocatedItem("Bopstead02Treasure01", "Treasure Money(1111)"),
        new LocatedItem("Bopstead02Treasure03", "Treasure Money(7777)"),
        new LocatedItem("Bopstead02Treasure02", "Treasure Money(3333)"),
        new LocatedItem("Bopstead02Treasure04", "Treasure Money(9999)"),
        new LocatedItem("Bopstead01Treasure03", "Treasure Money(5)")
    };

}