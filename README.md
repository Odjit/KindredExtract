![](logo.png)
# KindredExtract for V Rising
KindredExtract is a mod development tool.

This is NOT meant for live servers, nor is it a mod meant for regular players. 
It is meant for developers who want to use a tool to check things around them for reference points.
It will print out a lot of files to your DedicatedServer directory. This action isn't great on live servers.
Keep ranges tight, as there are a lot more things around you than you would think.

**Note:** Dependencies are not working while we are in a test period. Please refer to the **Installation** section for more information.

---
Also, thanks to the V Rising modding and server communities for ideas and requests!
Feel free to reach out to me on discord (odjit) if you have any questions or need help with the mod.

[V Rising Modding Discord](https://vrisingmods.com/discord)




## Commands

### State Commands
- `.state switchdump` 
  - Switches between Kindred and ProjectM entity dumping
- `.state player (name)` 
  - Dumps the state of a player (User/Char/Team/Progression)
  - shortcut: .s p
- `.state slots (player name)`
  - Outputs all slots of the player
  - shortcut: .s s
- `.state clan` 
  - Spits out Clan Info 
  - shortcut .s c
- `.state inventory` 
  - retrieves inventory state
  - shortcut: .s i
- `.state door`
  - retrieves door state
  - shortcut: .s d
- `.state ownedby (player name)` 
  - Outputs state of entities owned by the player
  - shortcut: .s o
- `.state entity (entityID#) (version)`
  - spits out enitiy info of the entity specified by ID and Version
  - shortcut: .s e
- `.state prefab (guid)` 
  - spits out prefab info- singular if GUID specified, otherwise all prefabs to a file each with components attached within
- `.state teams`
  - checks all the team data
  - shortcut: .s t
- `.state nearby (radius)`
  - gets nearby entities based on range. Careful there are a lot even very close to you.
  - shortcut: .s n
- `.state tilemodels (radius)`
  - gets nearby tilemodel entities
  - shortcut: .s tm
- `.state rooms (radius)`
  - gets nearby rooms
  - shortcut: .s r
- `.state castleterritory (index#)`
  - outputs a particular or all castle territories.
  - shortcut: .s ct
- `.state mapzones`
  - outputs map zones
  - shortcut: .s mz
- `.state worldregionpolygon`
  - outputs all world region polygons
  - shortcut: .s wrp
- `.state chunkportals`
  - outputs all chunk portals
  - shortcut: .s cp
- `.state buffs (radius)`
  - outputs all buffs of nearby entities
  - shortcut: .s b
- `.state spawnregions`
  - outputs all spawn regions
  - shortcut: .s sr
- `.state time`
  - outputs the current server time
- `.state SetPasteBinKeysNoLog`
  - sets the Pastebin API, userkey, and optional folder keys (This is kind of legacy from a foray into use on lives, but pastebin has limits and the like. I'm leaving it, but its not needed)

### Entity Commands
- `.entity teleport (entityID) (version)`
  - teleport to the specified entity
  - shortcut: .e tp
- `.entity despawn (entityID) (version)`
  - despawn the specified entity
  - shortcut: .e d
- `.entity destroy (entityID) (Version)`
  - Destroy the specified entity
  - shortcut: .e del
- `.entity topcount (topNum) (filter)`
  - Counts the top entities in the world
  - shortcut: .e tc

### Dump Commands
- `.dump prefabs`
  - dumps all prefabs to a file as prefabGuids for a Prefabs.cs file
  - shortcut: .dump p
- `.dump types`
  - Dumps all ECS component types to file (for usage in ComponentExtractors.tt)
  - shortcut: .dump t
- `.dump entityqueries`
  - Dumps all ECS entity queries to file
  - shortcut: .dump eq
- `.dump prefabjsons`
  - dumps all prefab names and IDs to JSON files, grouped by prefix
  - shortcut: .dump pj
- `.dump guidpos (prefab)`
  - Dumps positions of all instances of a prefab to a CSV file
- `.dump localization`
  - Dumps localization data
- `.dump prefabnames`
  - Dumps prefab names to JSON
- `.dump systems`
  - Dumps ECS system hierarchies to files (per world)
  - shortcut: .dump s
  
  
## Eventual To-Do/Possible features
- I add as I require.

## Installation
<details> <summary>Steps</summary>

1. Install BepInEx, which is required for modding VRising. Follow the instructions provided at [BepInEx Installation Guide](https://wiki.vrisingmods.com/user/bepinex_install.html) to set it up correctly in your VRising game directory.
   - **Note:** Until BepInEx is updated for 1.1, please do not use the thunderstore version. Get the correct testing version https://wiki.vrisingmods.com/user/game_update.html.

2. Download the plugin along with its dependencies (VCF). Ensure you select the correct versions that are compatible with your game.
   - **Note:** Again, until dependencies are updated for 1.1, please do not use the thunderstore version. Get the correct testing version https://wiki.vrisingmods.com/user/game_update.html.

3. After downloading, locate the .dll files for this plugin and its dependencies. Move or copy these .dll files into the `BepInEx\Plugins` directory within your VRising installation folder.

   - **Single Player Note:**
     - If you are playing in single player mode, you will need to install [ServerLaunchFix](https://thunderstore.io/c/v-rising/p/Mythic/ServerLaunchFix/). This is a server-side mod that is essential for making the commands work properly on the client side server. Make sure to download and place it in the same `BepInEx\Plugins` directory.

4. Launch the Game: Start VRising. If everything has been set up correctly, the plugin should now be active in the game.

</details>
