using Bloodstone.API;
using KindredExtract.Commands.Converters;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using ProjectM.Tiles;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using VampireCommandFramework;

namespace KindredExtract.Commands;

[CommandGroup("state", "s")]
public class StateCommands
{
    readonly static string StateFolder = Path.Combine(Directory.GetCurrentDirectory(), "EntityStateFiles");

    struct PasteBinKeys
    {
        public string ApiKey;
        public string UserKey;
        public string FolderKey;
    }

    static Dictionary<ulong, PasteBinKeys> pasteBinKeys = [];


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
    static string OutputEntityState(ChatCommandContext ctx, Entity entity)
    {
        Directory.CreateDirectory(StateFolder);
        var fileName = $"Entity_{entity.Index}.txt";
        if (entity.Has<User>())
        {
            fileName = $"User_{entity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<PlayerCharacter>())
        {
            fileName = $"Player_{entity.Read<PlayerCharacter>().Name}_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<CastleHeart>())
        {
            fileName = $"Castle_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<Door>())
        {
            fileName = $"Door_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<InventoryConnection>())
        {
            var ic = entity.Read<InventoryConnection>();
            var prefabGuid = entity.Read<PrefabGUID>();
            if (ic.InventoryOwner.Equals(Entity.Null))
                fileName = $"Inventory_{prefabGuid.LookupName()}_{entity.Index}_{entity.Version}.txt";
            else
                fileName = $"Inventory_{ic.InventoryOwner.Read<PlayerCharacter>().Name}_{prefabGuid.LookupName()}_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<VBloodProgressionUnlockData>())
        {
            var attach = entity.Read<Attach>();
            var charName = attach.Parent.Read<User>().CharacterName;

            fileName = $"Progression_{charName}_{entity.Index}_{entity.Version}.txt";
        }
        else if (entity.Has<TeamData>())
        {
            if (entity.Has<ClanTeam>())
            {
                fileName = $"Clan_{entity.Read<ClanTeam>().Name}_{entity.Index}_{entity.Version}.txt";
            }
            else if (entity.Has<UserTeam>())
            {
                if (entity.Read<UserTeam>().UserEntity.Equals(Entity.Null))
                    fileName = $"UserTeam_{entity.Index}_{entity.Version}.txt";
                else
                    fileName = $"UserTeam_{entity.Read<UserTeam>().UserEntity.Read<User>().CharacterName}_{entity.Index}_{entity.Version}.txt";
            }
            else if (entity.Has<CastleTeamData>())
            {
                var castleHeart = entity.Read<CastleTeamData>().CastleHeart;
                if (!castleHeart.Has<UserOwner>() || castleHeart.Read<UserOwner>().Owner.GetEntityOnServer().Equals(Entity.Null))
                    fileName = $"CastleTeam_{entity.Index}.txt";
                else
                    fileName = $"CastleTeam_{castleHeart.Read<UserOwner>().Owner.GetEntityOnServer().Read<User>().CharacterName}_{entity.Index}_{entity.Version}.txt";
            }
            else
            {
                fileName = $"Team_{entity.Index}_{entity.Version}.txt";
            }
        }
        else if (entity.Has<TileModel>())
        {
            if (entity.Has<PrefabGUID>())
            {
                fileName = $"TileModel_{entity.Index}_{entity.Version}_({entity.Read<PrefabGUID>().LookupName()}).txt";
            }
            else
            {
                fileName = $"TileModel_{entity.Index}_{entity.Version}.txt";
            }
        }

        CopyStateFileToPrev(fileName);

        var entityData = EntityDebug.RetrieveComponentData(entity);

        if (entity.Has<TeamAllies>())
        {
            // Add team allies to the entity data
            var teamAllies = VWorld.Server.EntityManager.GetBuffer<TeamAllies>(entity);
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

        if (pasteBinKeys.TryGetValue(ctx.User.PlatformId, out var keys))
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

        if(string.IsNullOrEmpty(folderName))
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
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var userEntity = player?.Value.UserEntity ?? ctx.Event.SenderUserEntity;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var userFile = OutputEntityState(ctx, userEntity);
        var charFile = OutputEntityState(ctx, charEntity);

        var teamEntity = charEntity.Read<TeamReference>().Value;
        var teamFile = OutputEntityState(ctx, teamEntity);

        var progressionEntity = userEntity.Read<ProgressionMapper>().ProgressionEntity;
        var progressionFile = OutputEntityState(ctx, progressionEntity.GetEntityOnServer());

        ctx.Reply($"Player '{characterName}' state written to {userFile}, {charFile}, {teamFile}, and {progressionFile}");
    }

    [Command("inventory", "i", description: "Retrieves inventory state", adminOnly: true)] // in progress
    public static void InventoryState(ChatCommandContext ctx, OnlinePlayer player = null)
    {
        var characterName = player?.Value.CharacterName ?? ctx.Name;
        var charEntity = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

        var inventories = Core.EntityManager.GetBuffer<InventoryInstanceElement>(charEntity);
        foreach(var inventory in inventories)
        {
            if(inventory.ExternalInventoryEntity.Equals(Entity.Null)) continue;
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

    [Command("prefab", "p", description: "Spits out entity info", adminOnly: true)]
    public static void PrefabState(ChatCommandContext ctx, int id)
    {
        var collectionSystem = Core.Server.GetExistingSystem<PrefabCollectionSystem>();
        var keys = collectionSystem.PrefabGuidToNameDictionary.Keys;
        foreach (var key in keys)
        {
            if (key.GuidHash != id) continue;
            collectionSystem.PrefabLookupMap.TryGetValue(key, out var prefab);
            var fileName = OutputEntityState(ctx, prefab);
            ctx.Reply($"Prefab {id} {key.LookupName()} state written to {fileName}");
        }
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
}