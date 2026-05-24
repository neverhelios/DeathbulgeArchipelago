# Deathbulge Archipelago

Work VERY in progress, this repo will contain only the Archipelago client, the apworld is in another castle

TODO:

- SHOPS
- Take into account the non linearity (Show some menus earlier, like key merch)
- Should allow to get out of the bus and out of the lab more freely, and make the bus free
- Better organisation of the code (Like make an interface for lua capturing)
- Claire bottle Sanity
- Door Sanity
- Beat Sanity
- Music randomizer
- Transition randomizer
- Show message that shows obtained items during a skip

Known Issues:

- Check of the Spiky Silver Thing. It DOES send a check so this is not the worst issue, but can give you the item too early (even if it doesn't really change anything)
  -> How to solve (just for my dev purpose): Change the way "SideGigTonewood01.State" is updated so treat the fork differently
- Money will be given every time you connect, enjoy :)
- `[Key Merch] Class Changer` is unlocked during the conversation with the tooltip, not with the `[Treasure] MODPODClass` check (This will be a reoccuring issue for sure)
- Lottery Tickets (all `[Treasure] PrizeTicket`) are checked in the inventory to know if they should spawn or not :(
  AND the quest advances with locations, not inventory item GAGAGAGAGAGA (I'm sure this ishow the gigs are managed)
  -> Ask the location to the archipelago server instead / (Better solution) Save the state with a custom saver like FayeDashBoosted
- `[Key Merch] Glam Reader` Doesn't unlock the Glam jauge (Fuck my life the key items are fucking useless)
- The sampler tuto is broken lol, it only sets you the beats you already have, and changes your class only if you have found it
- Legendary beats (and I suppose also just beats) are checked in the inventory to know if they should spawn or not :(
  -> Ask the location to the archipelago server instead / (Better solution) Save the state with a custom saver like FayeDashBoosted
- Inner Boot don't lock correctly but who is surprised ? x(
