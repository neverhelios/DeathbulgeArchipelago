# Deathbulge Archipelago

Work VERY in progress, this repo will contain only the Archipelago client, the apworld is in another castle

# WARNING !!!!!

- You MUST be connected to the archipelago server to play a randomized game, even solo.

## Changes with the base game and limitations

- You can leave the bus whenever you want (well at some point you will need to do some OOB but fuck it I'll fix it later)

- Shortly, you will be able to teleport to bopstead from anywhere, and go back to hoho from the lab

## Install the mod

First you need [BepInEx V 5.4 64 bit](https://docs.bepinex.dev/articles/user_guide/installation/index.html#tabpanel_bHGHmlrG6S_tabid-win) on your game

1. https://github.com/BepInEx/BepInEx/releases/
2. Download BepInEx 5.4.x win_x64
3. Open Deathbulge's Gamefiles
4. Extract `BepInEx` in the base Folder (so that `doorstop_config.ini` and `winhttp.dll` are next to the `deathbulge.exe`)
5. Start the Game once
6. Open the `BepInEx` Folder
7. Open the `plugins` Folder
8. Paste the [DeathbulgeArchipelagoClient.zip](https://github.com/neverhelios/DeathbulgeArchipelago/releases/latest/download/DeathbulgeArchipelagoClient.zip) to the current folder
9. Unzip the zip in the folder

And that's it !

You can now start the game once, and then in the `BepInEx` Folder there is now a folder `config` that contains `DeathbulDeathbulgeArchipelagoClientgeArchipelago.cfg`

This file contains all the login options so you MUST change things here if you want to play

Btw you need to launch the archipelago BEFORE the client and if you deconnect, nothing is currently handled so good luck

## Building at home

Well I didn't test in another PC so I can't really be sure that it will work on your machine , I'll update it when I have the motivation (or if someone wants it, neverhelios on discord)

## TODO:

- QOL show when connected or not to the server
- Manage archipelago deconnection
- Add archipelago data to the savefile to play offline
- More user friendly configuration

- SHOPS
- Take into account the non linearity (Show some menus earlier, like key merch)
- Should allow to get out of the bus and out of the lab more freely, and make the bus free
- Claire bottle Sanity
- Door Sanity
- Beat Sanity
- Music randomizer
- Transition randomizer
- Show message that shows obtained items during a skip
- Bufferize the items allowing to play offline and send check when offline

Dev only

- Use Conversation model for everything instead of lua.run
- Create a function for displaying dialogue entry in the console more easily
- Create a dictionary (key is tuple convID & entryID) that allow change the dialogues texts in O(1) instead of ifs
- Find a way to add completely new conversations so I can just send links to these new convs

## Known Issues:

- Double proc for the Spiky Silver Thing because the location check is received too early, maybe have a global variable that avoid to to show an item if a dialogue is started and not finished
- Money will be given every time you connect, enjoy :)
- The sampler tuto is broken lol, it only sets you the beats you already have, and changes your class only if you have found it
- Well maybe the `[Key Merch] 16th Deck Keycard` doesn't work to the softlock doesn't exist looooooooool
- If you take All claire's bottle in one go it gives you one check, but if to take it step by step it gives you two other checks that are mutually exclusive with the fist one xD I'll try to give everything at the end of the quest anyway
