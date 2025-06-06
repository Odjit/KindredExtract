using BepInEx.Unity.IL2CPP.Utils.Collections;
using KindredExtract.Commands.Converters;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using ProjectM.Physics;
using ProjectM.Terrain;
using ProjectM.Tiles;
using Stunlock.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using VampireCommandFramework;

namespace KindredExtract.Commands;

[CommandGroup("state", "s")]
public class StateCommands
{
    readonly static string StateFolder = Path.Combine(Directory.GetCurrentDirectory(), "EntityStateFiles");
    readonly static string ItemDataFolder = Path.Combine(Directory.GetCurrentDirectory(), "ItemData");

    struct PasteBinKeys
    {
        public string ApiKey;
        public string UserKey;
        public string FolderKey;
    }

    static Dictionary<ulong, PasteBinKeys> pasteBinKeys = [];

    static bool useProjectMDump;

    [Command("switchdump", description: "Switches between Kindred and ProjectM entity dumping", adminOnly: true)]
    public static void SwitchDump(ChatCommandContext ctx)
    {
        useProjectMDump = !useProjectMDump;

        if (useProjectMDump)
            ctx.Reply("Swapped to ProjectM entity dumping");
        else
            ctx.Reply("Swapped to Kindred entity dumping");
    }

    static void CopyStateFileToPrev(string fileName)
    {
        if (File.Exists(Path.Combine(StateFolder, fileName)))
        {
            string prevFileName = Path.Combine(StateFolder, Path.GetFileNameWithoutExtension(fileName) + "_Prev" + Path.GetExtension(fileName));
            if (File.Exists(prevFileName))
                File.Delete(prevFileName);
            File.Copy(Path.Combine(StateFolder, fileName), prevFileName);
        }
    }

    public static string OutputEntityState(ChatCommandContext ctx = null, Entity entity = default(Entity), string fileName = null)
    {
        Directory.CreateDirectory(StateFolder);
        if (fileName == null)
        {
            fileName = $"Entity_{entity.Index}_{entity.Version}";

            string prefabName = "";
            string attachedPlayerName = null;
            string attachedPrefabName = null;

            if (entity.Has<PrefabGUID>())
            {
                prefabName = entity.Read<PrefabGUID>().LookupName();
                fileName = $"Entity_{prefabName}_{entity.Index}_{entity.Version}";
            }
            if (entity.Has<Attached>())
            {
                var attached = entity.Read<Attached>();
                if (!attached.Parent.Equals(Entity.Null))
                {
                    if (attached.Parent.Has<PlayerCharacter>())
                        attachedPlayerName = attached.Parent.Read<PlayerCharacter>().Name.ToString();
                    if (attached.Parent.Has<PrefabGUID>())
                        attachedPrefabName = attached.Parent.Read<PrefabGUID>().LookupName();
                }
            }

            if (entity.Has<User>())
            {
                fileName = $"User_{entity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<PlayerCharacter>())
            {
                fileName = $"Player_{entity.Read<PlayerCharacter>().Name}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<CastleHeart>())
            {
                fileName = $"Castle_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<Door>())
            {
                fileName = $"Door_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<InventoryConnection>())
            {
                var ic = entity.Read<InventoryConnection>();
                if (ic.InventoryOwner.Equals(Entity.Null))
                    fileName = $"Inventory_{prefabName}_{entity.Index}_{entity.Version}";
                else if (ic.InventoryOwner.Has<PlayerCharacter>())
                {
                    fileName = $"Inventory_{ic.InventoryOwner.Read<PlayerCharacter>().Name}_{prefabName}_{entity.Index}_{entity.Version}";
                }
                else if (ic.InventoryOwner.Has<NameableInteractable>())
                {
                    fileName = $"Inventory_{ic.InventoryOwner.Read<NameableInteractable>().Name}_{prefabName}_{entity.Index}_{entity.Version}";
                }
                else
                {
                    fileName = $"Inventory_{ic.InventoryOwner.Read<PrefabGUID>().LookupName()}_{prefabName}_{entity.Index}_{entity.Version}";
                }
            }
            else if (entity.Has<VBloodProgressionUnlockData>())
            {
                var attach = entity.Read<Attach>();
                var charName = attach.Parent.Read<User>().CharacterName;

                fileName = $"Progression_{charName}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<TeamData>())
            {
                if (entity.Has<ClanTeam>())
                {
                    fileName = $"Clan_{entity.Read<ClanTeam>().Name}_{entity.Index}_{entity.Version}";
                }
                else if (entity.Has<UserTeam>())
                {
                    if (entity.Read<UserTeam>().UserEntity.Equals(Entity.Null))
                        fileName = $"UserTeam_{entity.Index}_{entity.Version}";
                    else
                        fileName = $"UserTeam_{entity.Read<UserTeam>().UserEntity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}";
                }
                else if (entity.Has<CastleTeamData>())
                {
                    var castleHeart = entity.Read<CastleTeamData>().CastleHeart;
                    if (!castleHeart.Has<UserOwner>() || castleHeart.Read<UserOwner>().Owner.GetEntityOnServer().Equals(Entity.Null))
                        fileName = $"CastleTeam_{entity.Index}";
                    else
                        fileName = $"CastleTeam_{castleHeart.Read<UserOwner>().Owner.GetEntityOnServer().Read<User>().CharacterName}_{entity.Index}_{entity.Version}";
                }
                else
                {
                    fileName = $"Team_{entity.Index}_{entity.Version}";
                }
            }
            else if (entity.Has<TileModel>())
            {
                if (entity.Has<PrefabGUID>())
                {
                    fileName = $"TileModel_{entity.Index}_{entity.Version}_({prefabName})";
                }
                else
                {
                    fileName = $"TileModel_{entity.Index}_{entity.Version}";
                }
            }
            else if (entity.Has<Buff>())
            {
                if (prefabName != "")
                {
                    if (attachedPlayerName != null)
                        fileName = $"Buff_{entity.Index}_{entity.Version}_({prefabName} on Player {attachedPlayerName})";
                    else if (attachedPrefabName != null)
                        fileName = $"Buff_{entity.Index}_{entity.Version}_({prefabName} on {attachedPrefabName})";
                    else
                        fileName = $"Buff_{entity.Index}_{entity.Version}_({prefabName})";
                }
                else
                {
                    fileName = $"Buff_{entity.Index}_{entity.Version}";
                }
            }
            else if (entity.Has<AchievementClaimedElement>())
            {
                var userEntity = entity.Read<Attached>().Parent;
                fileName = $"Achievements_{userEntity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<UserMapZonePackedRevealElement>())
            {
                var userEntity = entity.Read<Attached>().Parent;
                fileName = $"UserMapZone_{userEntity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<InventoryBuffer>())
            {

                fileName = $"Inventory_{entity.Index}_{entity.Version}";
                if (prefabName != "")
                {
                    fileName = $"Inventory_{entity.Index}_{entity.Version}_({prefabName})";
                }
            }
            else if (entity.Has<MapIconData>())
            {
                if (prefabName != "")
                {
                    if (attachedPlayerName != null)
                        fileName = $"MapIcon_{entity.Index}_{entity.Version}_({prefabName} on Player {attachedPlayerName})";
                    else if (attachedPrefabName != null)
                        fileName = $"MapIcon_{entity.Index}_{entity.Version}_({prefabName} on {attachedPrefabName})";
                    else
                        fileName = $"MapIcon_{entity.Index}_{entity.Version}_({prefabName})";
                }
                else
                {
                    fileName = $"MapIcon_{entity.Index}_{entity.Version}";
                }
            }
            else if (prefabName.StartsWith("AB_"))
            {
                fileName = $"{prefabName}_{entity.Index}_{entity.Version}";
                if (attachedPlayerName != null)
                    fileName = $"{prefabName}_{entity.Index}_{entity.Version}_(Attached to Player {attachedPlayerName})";
                else if (attachedPrefabName != null)
                    fileName = $"{prefabName}_{entity.Index}_{entity.Version}_(Attached to {attachedPrefabName})";
            }
            else if (entity.Has<CastleTerritory>())
            {
                var ct = entity.Read<CastleTerritory>();
                fileName = $"CastleTerritory_{ct.CastleTerritoryIndex}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<MapZoneData>())
            {
                var mz = entity.Read<MapZoneData>();
                fileName = $"MapZone_{mz.ZoneIndex}_{entity.Index}_{entity.Version}";
            }
            else if (entity.Has<WorldRegionPolygon>())
            {
                var wrp = entity.Read<WorldRegionPolygon>();
                fileName = $"WorldRegionPolygon_{wrp.WorldRegion}_{entity.Index}_{entity.Version}";
            }
        }

        if (!fileName.EndsWith(".txt"))
            fileName = fileName + ".txt";

        /*// Create folder based on entity lookup name
        if (entity.Has<PrefabGUID>())
        {
            var prefabName = entity.Read<PrefabGUID>().LookupName();
            var folderPath = Path.Combine(StateFolder, prefabName);
            Directory.CreateDirectory(folderPath);
            fileName = Path.Combine(folderPath, fileName);
        }*/

        CopyStateFileToPrev(fileName);

        string entityData;

        if (!useProjectMDump)
        {
            entityData = EntityDebug.RetrieveComponentData(entity);
        }
        else
        {
            var sb = new Il2CppSystem.Text.StringBuilder();
            EntityDebuggingUtility.DumpEntity(Core.TheWorld, entity, true, sb);
            entityData = sb.ToString();
        }

        if (entity.Has<TeamAllies>())
        {
            // Add team allies to the entity data
            var teamAllies = Core.TheWorld.EntityManager.GetBuffer<TeamAllies>(entity);
            for (int i = 0; i < teamAllies.Length; ++i)
            {
                if (teamAllies[i].Value.Equals(Entity.Null)) continue;
                entityData += $"\n\n" +
                               "#######################################\n" +
                               "Team Ally - {i}\n" +
                               "#######################################\n" +
                               EntityDebug.RetrieveComponentData(teamAllies[i].Value);
            }
        }

        if (ctx!=null && pasteBinKeys.TryGetValue(ctx.User.PlatformId, out var keys))
        {
            ctx.Reply($"For {fileName} Paste Bin Response: " + CreatePaste(keys.ApiKey, keys.UserKey, keys.FolderKey, entityData, fileName));
        }
        else
        {
            File.WriteAllText(Path.Combine(StateFolder, fileName), entityData);
        }
        return fileName;
    }

    static string CreatePaste(string apiKey, string userKey, string folderName, string pasteText, string pasteName)
    {
        var httpClient = new HttpClient();
        var parameters = new Dictionary<string, string>
        {
            { "api_dev_key", apiKey },
            { "api_user_key", userKey },
            { "api_option", "paste" },
            { "api_paste_code", pasteText },
            { "api_paste_name", pasteName },
            { "api_folder_key", folderName },
            { "api_paste_expire_date", "1H" }
        };

        if (string.IsNullOrEmpty(folderName))
        {
            parameters.Remove("api_folder_key");
        }

        var response = httpClient.PostAsync("https://pastebin.com/api/api_post.php", new FormUrlEncodedContent(parameters)).Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;

        return responseContent;
    }

    [Command("SetPasteBinKeysNoLog", description: "Sets the Pastebin API, userKey, and optional folder keys", adminOnly: true)]
    public static void SetPasteBinKeys(ChatCommandContext ctx, string apiKey, string userKey, string folderKey = "")
    {
        pasteBinKeys[ctx.User.PlatformId] = new PasteBinKeys
        {
            ApiKey = apiKey,
            UserKey = userKey,
            FolderKey = folderKey
        };
        ctx.Reply("PasteBin keys set");
    }

    [Command("clan", "c", description: "Spits out clan info", adminOnly: true)]
    public static void ClanState(ChatCommandContext ctx, string clanName)
    {
        Directory.CreateDirectory(StateFolder);
        if (!Helper.FindClan(clanName, out var clanEntity))
        {
            ctx.Reply($"No clan found matching name '{clanName}'");
            return;
        }

        var fileName = OutputEntityState(ctx, clanEntity);
        ctx.Reply($"Clan '{clanName}' state written to {fileName}");
    }

    [Command("player", "p", description: "Removes a player from a clan", adminOnly: true)] // in progress
    public static void PlayerState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var sb = new StringBuilder();
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var userEntity = player?.Value.UserEntity ?? ctx.Event.SenderUserEntity;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var userFile = OutputEntityState(ctx, userEntity);
        var charFile = OutputEntityState(ctx, charEntity);
        sb.Append($"Player '{characterName}' state written to {userFile}, {charFile},");

        if (charEntity.Has<TeamReference>())
        {
            var teamEntity = charEntity.Read<TeamReference>().Value;
            var teamFile = OutputEntityState(ctx, teamEntity);
            sb.Append($" {teamFile},");
        }

        var progressionEntity = userEntity.Read<ProgressionMapper>().ProgressionEntity;
        var progressionFile = OutputEntityState(ctx, progressionEntity.GetEntityOnServer());
        sb.Append($" and {progressionFile}");

        ctx.Reply(sb.ToString());
    }

    [Command("slots", "s", description: "Outputs all slots of the player", adminOnly: true)] // in progress
    public static void SlotsState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var slots = Core.EntityManager.GetBuffer<AttachedBuffer>(charEntity);
        foreach (var slot in slots)
        {
            if (slot.PrefabGuid != Data.Prefabs.AbilityGroupSlot) continue;
            var ags = slot.Entity.Read<AbilityGroupSlot>();
            OutputEntityState(ctx, slot.Entity, fileName: $"{characterName}_Slot_{ags.SlotId}.txt");
        }

        ctx.Reply($"Player '{characterName}' slots saved");
    }

    [Command("inventory", "i", description: "Retrieves inventory state", adminOnly: true)] // in progress
    public static void InventoryState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var inventories = Core.EntityManager.GetBuffer<InventoryInstanceElement>(charEntity);
        foreach (var inventory in inventories)
        {
            if (inventory.ExternalInventoryEntity.Equals(Entity.Null)) continue;
            OutputEntityState(ctx, inventory.ExternalInventoryEntity.GetEntityOnServer());
        }

        ctx.Reply($"Player '{characterName}' inventory saved");
    }

    [Command("door", "d", description: "Outputs door states", adminOnly: true)] // in progress
    public static void DoorState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var userEntity = player?.Value.UserEntity ?? ctx.Event.SenderUserEntity;

        var num = 0;
        foreach (var door in Helper.GetEntitiesByComponentType<Door>(true))
        {
            if (door.Has<UserOwner>() && door.Read<UserOwner>().Owner.GetEntityOnServer().Equals(userEntity))
            {
                OutputEntityState(ctx, door);
                num++;
            }
        }

        ctx.Reply($"Wrote out {num} door states for {characterName}");
    }



    [Command("ownedby", "o", description: "Outputs state of entities owned by the player", adminOnly: true)] // in progress
    public static void OwnedByState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var userEntity = player?.Value.UserEntity ?? ctx.Event.SenderUserEntity;

        var num = 0;
        foreach (var e in Helper.GetEntitiesByComponentType<UserOwner>(true))
        {
            if (e.Read<UserOwner>().Owner.GetEntityOnServer().Equals(userEntity))
            {
                OutputEntityState(ctx, e);
                num++;
            }
        }

        ctx.Reply($"Wrote out {num} states owned by {characterName}");
    }

    [Command("entity", "e", description: "Spits out entity info", adminOnly: true)]
    public static void EntityState(ChatCommandContext ctx, int id, int version = 1)
    {
        var entity = new Entity { Index = id, Version = version };
        if (!Core.EntityManager.Exists(entity))
        {
            ctx.Reply($"No entity found with id {id}");
            return;
        }

        var fileName = OutputEntityState(ctx, entity);
        ctx.Reply($"Entity {id} state written to {fileName}");
    }

    [Command("prefab", description: "Spits out entity info", adminOnly: true)]
    public static void PrefabState(ChatCommandContext ctx, int? id = null)
    {
        var collectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();
        if (id == null)
        {
            var gameObject = new GameObject("PrefabOutputter");
            var mb = gameObject.AddComponent<IgnorePhysicsDebugSystem>();
            mb.StartCoroutine(OutputtingAllPrefabs(ctx, collectionSystem, gameObject).WrapToIl2Cpp());
            return;
        }

        var key = new PrefabGUID() { _Value = id.Value };
        if (collectionSystem._PrefabLookupMap.TryGetValue(key, out var prefab))
        {
            var fileName = OutputEntityState(ctx, prefab, key.LookupName() + ".txt");
            ctx.Reply($"Prefab {id} {key.LookupName()} state written to {fileName}");
        }
        else
        {
            ctx.Reply($"Prefab doesn't exist for {id}");
        }
    }

    static IEnumerator OutputtingAllPrefabs(ChatCommandContext ctx, PrefabCollectionSystem collectionSystem, GameObject gameObject)
    {
        
        foreach (var entry in collectionSystem._PrefabGuidToEntityMap)
        {
            var key = entry.Key;
            if (collectionSystem._PrefabLookupMap.GuidToEntityMap.TryGetValue(key, out var prefab))
            {
                try
                { 
                    var fileName = OutputEntityState(ctx, prefab, key.LookupName() + ".txt");
                    Core.Log.LogInfo($"Prefab {key.GuidHash} {key.LookupName()} state written to {fileName}");
                }
                catch (System.Exception e)
                {
                    Core.LogException(e, $"Outputting Prefab {key.LookupName()}");
                    Core.Log.LogInfo($"Error writing prefab {key.GuidHash} {key.LookupName()}: {e.Message}");
                }
                yield return null;
            }
            else
            {
                Core.Log.LogInfo($"Prefab doesn't exist for {key.GuidHash} {key.LookupName()}");
            }
        }
        GameObject.Destroy(gameObject);
    }

    [Command("teams", "t", description: "Checks all the team data", adminOnly: true)]
    public static void TeamState(ChatCommandContext ctx)
    {
        var teams = Helper.GetEntitiesByComponentType<TeamData>(true);
        foreach (var teamEntity in teams)
        {
            OutputEntityState(ctx, teamEntity);
        }
        ctx.Reply($"{teams.Length} teams written to files");
    }

    [Command("nearby", "n", description: "Gets nearby entities", adminOnly: true)]
    public static void NearbyEntityStates(ChatCommandContext ctx, int radius = 10)
    {
        var userEntity = ctx.Event.SenderUserEntity;
        var userPos = userEntity.Read<Translation>().Value;
        var entities = Helper.GetEntitiesByComponentType<Translation>(true);
        var num = 0;
        foreach (var entity in entities)
        {
            var pos = entity.Read<Translation>().Value;
            if (Vector3.Distance(userPos, pos) > radius) continue;
            OutputEntityState(ctx, entity);
            num++;
        }
        ctx.Reply($"Wrote out {num} states within {radius} units of {userEntity.Index}");
    }

    [Command("tilemodels", "tm", description: "Gets nearby tile model entities", adminOnly: true)]
    public static void NearbyTileModelStates(ChatCommandContext ctx, int radius = 10)
    {
        var userEntity = ctx.Event.SenderUserEntity;
        var userPos = userEntity.Read<Translation>().Value;
        var entities = Helper.GetEntitiesByComponentType<TileModel>(includeSpawn: true, includeDisabled: true);
        var num = 0;
        foreach (var entity in entities)
        {
            if (!entity.Has<Translation>()) continue;
            var pos = entity.Read<Translation>().Value;
            if (Vector3.Distance(userPos, pos) > radius) continue;
            OutputEntityState(ctx, entity);
            num++;
        }
        ctx.Reply($"Wrote out {num} states for Tile Models within {radius} units of {userEntity.Index}");
    }

    [Command("rooms", "r", description: "Gets nearby rooms", adminOnly: true)]
    public static void NearbyRoomStates(ChatCommandContext ctx, int radius = 10)
    {
        var userEntity = ctx.Event.SenderUserEntity;
        var userPos = userEntity.Read<Translation>().Value;
        var entities = Helper.GetEntitiesByComponentTypes<Translation, CastleRoom>(true);
        var num = 0;
        foreach (var entity in entities)
        {
            var pos = entity.Read<Translation>().Value;
            if (Vector3.Distance(userPos, pos) > radius) continue;
            OutputEntityState(ctx, entity);
            num++;
        }
        ctx.Reply($"Wrote out {num} states within {radius} units of {userEntity.Index}");
    }

    [Command("castleterritory", "ct", description: "Outputs a particular or all castle territory", adminOnly: true)]
    public static void CastleTerritoryState(ChatCommandContext ctx, int? id = null)
    {
        var entities = Helper.GetEntitiesByComponentType<CastleTerritory>(true);
        if (id == null)
        {
            foreach (var entity in entities)
            {
                OutputEntityState(ctx, entity);
            }
            ctx.Reply($"{entities.Length} castle territories written to files");
        }
        else
        {

            var entity = entities.ToArray().FirstOrDefault(e => e.Read<CastleTerritory>().CastleTerritoryIndex == id);
            if (entity.Equals(Entity.Null))
            {
                ctx.Reply($"No castle territory found with id {id}");
                return;
            }
            OutputEntityState(ctx, entity);
            ctx.Reply($"Castle territory {id} state written to file");
        }
    }

    [Command("mapzones", "mz", description: "Outputs map zones out", adminOnly: true)]
    public static void MapZoneState(ChatCommandContext ctx)
    {
        var entities = Helper.GetEntitiesByComponentType<MapZoneData>(true);
        foreach (var entity in entities)
        {
            OutputEntityState(ctx, entity);
        }
        ctx.Reply($"{entities.Length} map zones written to files");
    }

    [Command("worldregionpolygon", "wrp", description: "Outputs all world region polygons", adminOnly: true)]
    public static void WorldRegionPolygonState(ChatCommandContext ctx)
    {
        var entities = Helper.GetEntitiesByComponentType<WorldRegionPolygon>(true);
        foreach (var entity in entities)
        {
            OutputEntityState(ctx, entity);
        }
        ctx.Reply($"{entities.Length} world region polygons written to files");
    }

    [Command("chunkportals", "cp", description: "Outputs all chunk portals", adminOnly: true)]
    public static void ChunkPortalState(ChatCommandContext ctx)
    {
        var entities = Helper.GetEntitiesByComponentType<ChunkPortal>(true);
        foreach (var entity in entities)
        {
            OutputEntityState(ctx, entity);
        }
        ctx.Reply($"{entities.Length} chunk portals written to files");
    }

    [Command("buffs", "b", description: "Outputs all buffs of nearby entities", adminOnly: true)]
    public static void BuffStates(ChatCommandContext ctx, int radius = 5)
    {
        var userEntity = ctx.Event.SenderUserEntity;
        var userPos = userEntity.Read<Translation>().Value;
        var entities = Helper.GetEntitiesByComponentType<Buff>(true);
        var num = 0;
        foreach (var entity in entities)
        {
            if (!entity.Has<Attach>()) continue;
            var attach = entity.Read<Attach>();
            if (attach.Parent.Equals(Entity.Null)) continue;
            var pos = attach.Parent.Read<Translation>().Value;
            if (Vector3.Distance(userPos, pos) > radius) continue;
            OutputEntityState(ctx, entity);
            num++;
        }
        ctx.Reply($"Wrote out {num} buff states within {radius} units of {userEntity.Index}");
    }

    [Command("spawnregions", "sr", description: "Outputs all spawn regions", adminOnly: true)]
    public static void SpawnRegionState(ChatCommandContext ctx)
    {
        var entities = Helper.GetEntitiesByComponentType<SpawnRegion>(true);
        foreach (var entity in entities)
        {
            var ltw = entity.Read<LocalToWorld>();
            var spawnRegion = entity.Read<SpawnRegion>();
            Core.Log.LogInfo($"{ltw.Position}, {spawnRegion.RespawnDurationMin}, {spawnRegion.RespawnDurationMax}");

            OutputEntityState(ctx, entity);
        }
        ctx.Reply($"{entities.Length} spawn regions written to files");
    }

    [Command("time", description: "Outputs the current time", adminOnly: true)]
    public static void TimeState(ChatCommandContext ctx)
    {
        var time = Core.ServerTime;
        ctx.Reply($"Current time: {time}");
    }
}
