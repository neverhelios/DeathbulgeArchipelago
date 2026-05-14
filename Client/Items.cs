
using System;
using System.Collections.Generic;

class Items
{
    private static Random _random = new Random();

    public static string GetRandomItemName()
    {
        int countMoneyItems = itemMoneyNames?.Count ?? 0;
        int countTreasure = itemTreasureNames?.Count ?? 0;

        int total = countMoneyItems + countTreasure;
        int index = _random.Next(total);

        if (index < countMoneyItems)
            return itemMoneyNames[index];

        return itemTreasureNames[index - countMoneyItems];
    }

    public static List<string> GetItemTreasureNameList()
    {
        return itemTreasureNames;
    }

    static List<string> itemTreasureNames = [
"Tonewood01Treasure01",
"Tonewood02Treasure01",
"Tonewood03Treasure01",
"Tonewood03Treasure02",
"Tonewood04Treasure01",
"Tonewood06Treasure01",
"Tonewood08Treasure01",
"[Treasure] TonewoodGig02-Shrubbanshee",
"Tonewood04Treasure02",
"Tonewood06Treasure02",
"Tonewood08Treasure02",
"Tonewood08Treasure03",
"[Treasure] PrizeTicketJim",
"[Treasure] GillianFork",
"Claire02Treasure01",
"Claire06Treasure01",
"Claire04Treasure01",
"Claire04Treasure02",
"Claire07Treasure01",
"[Treasure] ClaireGig03-Madam",
"ClaireLower01Treasure01",
"Claire07Treasure02",
"Claire05Treasure01",
"ClaireLower02Treasure01",
"ClaireLower03Treasure02",
"ClaireLower04Treasure01",
"ClaireLower03Treasure01",
"ClaireLower03Treasure03",
"ClaireLower04Treasure02",
"[Treasure] PrizeTicketMadam",
"[Treasure] MODPODClass",
"[Treasure] PartyHouseReward",
"[Treasure] GeorgeousReward",
"[Treasure] NelReward",
"Bopstead01Treasure01",
"[Treasure] FoggyRewardPatch",
"[Treasure] DaemoPatch",
"Bopstead03Treasure01",
"Bopstead04Treasure01",
"[Treasure] PrizeTicketBarry",
"[Treasure] PrizeTicketBrioche",
"[Treasure] PrizeTicketPlat",
"[Treasure] BasementGig02-Whale",
"[Treasure] BasementGig04-Shutup",
"Basement03Treasure01",
"Basement06Treasure01",
"[Treasure] BasementGig05-Cuttle",
"[Treasure] BasementGig01-Cuttlebro",
"Basement06Treasure02",
"Basement06Treasure03",
"Basement04Treasure01",
"Basement03Treasure02",
"Basement03Treasure03",
"Basement05Treasure01",
"Basement02Treasure01",
"Whale25Treasure",
"Basement02Treasure02",
"[Treasure] PrizeTicketBase",
"TheBus02Treasure01",
"TheBus05Treasure01",
"TheBus05Treasure02",
"TheBus08Treasure02",
"TheBus08Treasure01",
"TheBus09Treasure01",
"TheBus10Treasure01",
"TheBus10Treasure02",
"TheBus11Treasure01",
"[Treasure] TheBusGig02-Weaver",
"[Treasure] TheBusGig01-Glamourella",
"TheBus10Treasure03",
"TheBus08Treasure03",
"TheBus05Treasure03",
"[Treasure] PrizeTicketBus",
"[Treasure] 13DeckKeycard",
"[Treasure] 14DeckKeycard",
"Lab03Treasure01",
"Lab04Treasure01",
"Lab07Treasure01",
"Lab08Treasure01",
"[Treasure] LabGig02-WIP",
"Lab01Treasure01",
"Lab04Treasure02",
"Lab07Treasure02",
"Lab09Treasure01",
"Lab10Treasure01",
"Lab12Treasure01",
"[Treasure] PrizeTicketLab",
"Lab10Treasure02",
"[Treasure] GlamReader",
"Hoho02Treasure01",
"Hoho02Treasure02",
"Hoho02Treasure03",
"Hoho02Treasure04",
"Hoho01Treasure01",
"Hoho02Treasure05",
"Hoho01Treasure02",
"[Treasure] PrizeTicketHoho",
"Hoho02Treasure06",
"Pokalyps01Treasure01",
"Pokalyps02Treasure01",
"Pokalyps02Treasure02",
"Pokalyps04Treasure01",
"Pokalyps05Treasure01",
"Pokalyps06Treasure01",
"Pokalyps07Treasure01",
"Pokalyps09Treasure01",
"Pokalyps05Treasure02",
"Pokalyps10Treasure01",
"Pokalyps01Treasure02",
"[Treasure] PrizeTicketPok",
"Dream03Treasure01",
"Dream03Treasure02",
"Dream03Treasure03",
"Dream05Treasure01",
"Dream04Treasure02",
"Dream04Treasure01",
"Dream04Treasure03"];

    static List<string> itemMoneyNames = [
"Tonewood08Treasure04",
"Claire04Treasure03",
"Claire04Treasure04",
"Claire04Treasure05",
"[Treasure] FoggyRewardMoney",
"[Treasure] Foggy300",
"Bopstead01Treasure02",
"Bopstead02Treasure01",
"Bopstead02Treasure03",
"Bopstead02Treasure02",
"Bopstead02Treasure04",
"Bopstead01Treasure03"];
}