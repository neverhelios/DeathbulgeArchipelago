
using System;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

namespace DeathbulgeArchipelagoClient;

class WarpManager
{
    // Not exhaustive
    private static readonly Dictionary<string, WarpDestination> warpDestinations = new()
    {
        // Bopstead
        {"BopsteadIntroSpawnPoint", new("Bopstead", "Bopstead01", "Intro01-Spawnpoint", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"BopsteadFayesHouse", new("Bopstead", "Bopstead01", "Bopstead01-FayeHouseExit", true, -1, true, "FayesHouse", "FayesHouseInteriorF0", "Default", "Objects", true, true, "Bopstead")},
        {"SaevaCutsceneTrigger", new("Bopstead", "Bopstead01", "SaevaCutsceneTrigger", true, 1, true, "FayesHouse", "FayesHouseInteriorF0", "Indoor3", "IndoorObjects", true, false, "Bopstead")},
        {"SaevaWarpBopstead01", new("Bopstead", "Bopstead01", "SaevaWarp", true, -1, true, "SaevaTent", "SaevaTent-Interior", "IndoorTent", "IndoorObjects", true, false, "Bopstead")},
        {"Bopstead01-Exit01", new("Bopstead", "Bopstead01", "Bopstead01-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead01-Exit02", new("Bopstead", "Bopstead01", "Bopstead01-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead01-Exit03", new("Bopstead", "Bopstead01", "Bopstead01-Exit03", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead02-Exit01", new("Bopstead", "Bopstead02", "Bopstead02-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead02-Exit02", new("Bopstead", "Bopstead02", "Bopstead02-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead02-Exit03", new("Bopstead", "Bopstead02", "Bopstead02-Exit03", true, -1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead02-Exit04", new("Bopstead", "Bopstead02", "Bopstead02-Exit04", true, 1, true, "BassBase", "BBInterior (Floor 0)", "Indoor2", "BehindIndoorObjects2", true, true, "Bopstead")},
        {"InterviewGroupInLoc", new("Bopstead", "Bopstead02", "InterviewGroupInLoc", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"BopsteadBusStop", new("Bopstead", "Bopstead02", "BusStopDestination", true, 1, true, "Bus Station", "Interior (Floor 0)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"MoveAsideSignupPlayer", new("Bopstead", "Bopstead02", "MoveAsideSignupPlayer", true, -1, false, "", "", "Default", "Objects", true, false, "Bopstead")},
        {"Bopstead03-Exit01", new("Bopstead", "Bopstead03", "Bopstead03-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead04-Exit01", new("Bopstead", "Bopstead04", "Bopstead04-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"PlayerPostBarry", new("Bopstead", "Bopstead04", "PlayerPostBarry", true, -1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        {"Bopstead05-Exit01", new("Bopstead", "Bopstead05", "Bopstead05-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Bopstead")},
        // Tonewood
        {"Tonewood01-Exit01", new("Tonewood", "Tonewood01", "Tonewood01-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood01-Exit02", new("Tonewood", "Tonewood01", "Tonewood01-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood02-Exit01", new("Tonewood", "Tonewood02", "Tonewood02-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood02-Exit02", new("Tonewood", "Tonewood02", "Tonewood02-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood03-Exit01", new("Tonewood", "Tonewood03", "Tonewood03-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood03-Exit02", new("Tonewood", "Tonewood03", "Tonewood03-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Ian03-FayeMeet", new("Tonewood", "Tonewood03", "Ian03-FayeMeet", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood04-Exit01", new("Tonewood", "Tonewood04", "Tonewood04-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood04-Exit02", new("Tonewood", "Tonewood04", "Tonewood04-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood04-Exit03-02", new("Tonewood", "Tonewood04", "Tonewood04-Exit03-02", true, 1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood05-Exit01", new("Tonewood", "Tonewood05", "Tonewood05-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood06-Exit01", new("Tonewood", "Tonewood06", "Tonewood06-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"Tonewood06-Exit02", new("Tonewood", "Tonewood06", "Tonewood06-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        {"TonewoodInFrontOfClaire", new("Tonewood", "Tonewood06", "Tonewood06-Cutscene02-PlayerPos02", true, -1, false, "", "", "Default", "Objects", true, true, "Tonewood")},
        // Claire's Hair
        {"CH01-Exit01", new("Claire", "ClaireHair01", "CH01-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH01-Exit02", new("Claire", "ClaireHair01", "CH01-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH02-Exit01", new("Claire", "ClaireHair02", "CH02-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH02-Exit02", new("Claire", "ClaireHair02", "CH02-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH02-Exit03", new("Claire", "ClaireHair02", "CH02-Exit03", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH03-Exit01", new("Claire", "ClaireHair03", "CH03-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH03-Exit02", new("Claire", "ClaireHair03", "CH03-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH04-Exit01", new("Claire", "ClaireHair04", "CH04-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH04-Exit02", new("Claire", "ClaireHair04", "CH04-Exit02", true, -1, false, "", "", "Upper", "UpperObjects", true, true, "Claire")},
        {"CH04-Exit03", new("Claire", "ClaireHair04", "CH04-Exit03", true, 1, false, "", "", "Upper", "UpperObjects", true, true, "Claire")},
        {"CH04-Exit04", new("Claire", "ClaireHair04", "CH04-Exit04", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH04-Exit05", new("Claire", "ClaireHair04", "CH04-Exit05", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH05-Exit01", new("Claire", "ClaireHair05", "CH05-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH06-Exit01", new("Claire", "ClaireHair06", "CH06-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH06-Exit02", new("Claire", "ClaireHair06", "CH06-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH07-Exit01", new("Claire", "ClaireHair07", "CH07-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH07-Exit02", new("Claire", "ClaireHair07", "CH07-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH07-Exit03", new("Claire", "ClaireHair07", "CH07-Exit03", true, -1, false, "", "", "Default", "Objects", true, true, "Claire")},
        {"CH08-Exit01", new("Claire", "ClaireHair08", "CH08-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Claire")},
        //Basement
        {"Basement01-Exit01", new("Basement", "Basement01", "Basement01-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement02-Exit01", new("Basement", "Basement02", "Basement02-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement02-Exit02", new("Basement", "Basement02", "Basement02-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement02-Exit03", new("Basement", "Basement02", "Basement02-Exit03", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement02-Exit04", new("Basement", "Basement02", "Basement02-Exit04", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"SaevaWarpBasement02", new("Basement", "Basement02", "SaevaWarp", true, -1, true, "SaevaTent", "SaevaTent-Interior", "IndoorTent", "IndoorObjects", true, false, "Basement")},
        {"Basement03-Exit01", new("Basement", "Basement03", "Basement03-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement03-Exit02", new("Basement", "Basement03", "Basement03-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement04-Exit01", new("Basement", "Basement04", "Basement04-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement04-Exit02", new("Basement", "Basement04", "Basement04-Exit02", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement05-Exit01", new("Basement", "Basement05", "Basement05-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement05-Exit02", new("Basement", "Basement05", "Basement05-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement05-Exit03", new("Basement", "Basement05", "Basement05-Exit03", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement06-Exit01", new("Basement", "Basement06", "Basement06-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement06-Exit02", new("Basement", "Basement06", "Basement06-Exit02", true, 1, true, "InnerTank", "ITInterior (Floor 0)", "Indoor", "BehindIndoorObjects0", true, true, "Basement")},
        {"Basement07-Exit01", new("Basement", "Basement07", "Basement07-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Basement")},
        {"Basement07-Exit02", new("Basement", "Basement07", "Basement07-Exit02", true, 1, false, "", "", "Default", "Objects", true, true, "Basement")},
        // The Bus
        {"PlayerBusEntrance", new("TheBus", "TheBus01", "PlayerBusEntrance", true, 1, true, "LowerDecks", "Interior (Floor 0)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus01-Exit01", new("TheBus", "TheBus01", "TheBus01-Exit01", true, 1, true, "LowerDecks", "Interior (Floor 3-1)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus02-Exit01", new("TheBus", "TheBus02", "TheBus02-Exit01", true, -1, true, "MidDecks01", "Interior (Floor 0-5)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus02-Exit02", new("TheBus", "TheBus02", "TheBus02-Exit02", true, 1, true, "MidDecks01", "Interior (Floor 2-7)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus03-Exit01", new("TheBus", "TheBus03", "TheBus03-Exit01", true, -1, true, "MidDecks02", "Interior (Floor 0-8)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus03-Exit02", new("TheBus", "TheBus03", "TheBus03-Exit02", true, 1, true, "MidDecks02", "Interior (Room 4-8)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        {"TheBus04-Exit01", new("TheBus", "TheBus04", "TheBus04-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "TheBus")},
        {"SaevaWarpTheBus10", new("TheBus", "TheBus10", "SaevaWarp", true, 1, true, "Floor16-1", "Interior01", "Indoor1", "IndoorObjects", true, false, "TheBus")},
        {"TheBus10-ElevatorExit", new("TheBus", "TheBus10", "TheBus10-ElevatorExit", true, -1, true, "Floor16-1", "Interior (Elevator)", "Indoor", "IndoorObjects", true, true, "TheBus")},
        // Hoho
        {"Way-BriffHide", new("Hoho", "Hoho01", "Way-BriffHide", true, -1, false, "", "", "Default", "Objects", true, true, "Hoho")},
        {"Hoho01-Exit01", new("Hoho", "Hoho01", "Hoho01-Exit01", true, 1, false, "", "", "Default", "Objects", true, true, "Hoho")},
        {"Hoho02-Exit01", new("Hoho", "Hoho02", "Hoho02-Exit01", true, -1, false, "", "", "Default", "Objects", true, true, "Hoho")},
        {"Way-BluehornBriff02", new("Hoho", "Hoho02", "Way-BluehornBriff02", true, -1, true, "BluehornInn", "Interior (Floor 0)", "Indoor", "IndoorObjects", true, true, "Hoho")},
        // Lab
        {"LabEntrance", new("Lab", "Lab01", "Cutscene01-Saeva", true, -1, false, "", "", "Default", "Objects", true, true, "Lab")},
    };

    public static void CustomPrimeWarp(string warpDestinationName)
    {
        if (warpDestinations.ContainsKey(warpDestinationName))
            CustomPrimeWarp(warpDestinations[warpDestinationName]);
        else
            throw new KeyNotFoundException($"Warp destination {warpDestinationName} does not exist");
    }

    public static void CustomPrimeWarp(WarpDestination warpDestination)
    {
        DialogueLua.SetVariable("WarpDestinationArea", warpDestination.warpDestinationArea);
        DialogueLua.SetVariable("Common.WarpDestinationMap", warpDestination.warpDestinationMap);
        DialogueLua.SetVariable("Common.WarpDestinationObject", warpDestination.warpDestinationObject);
        DialogueLua.SetVariable("Common.WarpChangeFacing", warpDestination.warpChangeFacing);
        DialogueLua.SetVariable("Common.WarpFacing", warpDestination.warpFacing);
        DialogueLua.SetVariable("Common.WarpIndoors", warpDestination.warpIndoors);
        DialogueLua.SetVariable("Common.WarpIndoorObject", warpDestination.warpIndoorObject);
        DialogueLua.SetVariable("Common.WarpIndoorRoomObject", warpDestination.warpIndoorRoomObject);
        DialogueLua.SetVariable("Common.WarpDestinationLayer", warpDestination.warpDestinationLayer);
        DialogueLua.SetVariable("Common.WarpDestinationSortLayer", warpDestination.warpDestinationSortLayer);
        DialogueLua.SetVariable("Common.WarpFadeOut", warpDestination.warpFadeOut);
        DialogueLua.SetVariable("Common.WarpAutosave", warpDestination.warpAutosave);
        DialogueLua.SetVariable("Common.LastWarpDestination", $"{warpDestination.warpDestinationMap}@{warpDestination.warpDestinationObject}");
        CommonObjects.GetFieldMain().mapLoader.PreviousArea = warpDestination.warpPreviousArea;
        DialogueLua.SetVariable("Common.CurrentArea", warpDestination.warpDestinationArea);
        DialogueLua.SetVariable("Common.CurrentMap", warpDestination.warpDestinationMap);
        CommonObjects.GetFieldMain().skipButton.Clear();
    }
}

class WarpDestination
{
    public string warpDestinationArea;
    public string warpDestinationMap;
    public string warpDestinationObject;
    public bool warpChangeFacing;
    public int warpFacing;
    public bool warpIndoors;
    public string warpIndoorObject;
    public string warpIndoorRoomObject;
    public string warpDestinationLayer;
    public string warpDestinationSortLayer;
    public bool warpFadeOut;
    public bool warpAutosave;
    public string warpPreviousArea;

    public WarpDestination(string DestinationArea, string DestinationMap, string DestinationObject, bool ChangeFacing,
                           int Facing, bool Indoors, string IndoorObject, string IndoorRoomObject, string DestinationLayer,
                           string DestinationSortLayer, bool FadeOut, bool Autosave, string PreviousArea)
    {
        warpDestinationArea = DestinationArea;
        warpDestinationMap = DestinationMap;
        warpDestinationObject = DestinationObject;
        warpChangeFacing = ChangeFacing;
        warpFacing = Facing;
        warpIndoors = Indoors;
        warpIndoorObject = IndoorObject;
        warpIndoorRoomObject = IndoorRoomObject;
        warpDestinationLayer = DestinationLayer;
        warpDestinationSortLayer = DestinationSortLayer;
        warpFadeOut = FadeOut;
        warpAutosave = Autosave;
        warpPreviousArea = PreviousArea;
    }
}