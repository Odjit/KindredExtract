using System.Collections.Generic;
using System.Linq;
using Bloodstone.API;
using KindredExtract.Models;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace KindredExtract.Services;

internal class PlayerService
{
	Dictionary<FixedString64, PlayerData> NamePlayerCache = new();
	Dictionary<ulong, PlayerData> SteamPlayerCache = new();

	internal bool TryFindSteam(ulong steamId, out PlayerData playerData)
	{
		return SteamPlayerCache.TryGetValue(steamId, out playerData);
	}

	internal bool TryFindName(FixedString64 name, out PlayerData playerData)
	{
		return NamePlayerCache.TryGetValue(name, out playerData);
	}

	internal PlayerService()
	{
		NamePlayerCache.Clear();
		SteamPlayerCache.Clear();
		EntityQuery query = Core.EntityManager.CreateEntityQuery(new EntityQueryDesc()
		{
			All = new ComponentType[]
				{
					ComponentType.ReadOnly<User>()
				},
			Options = EntityQueryOptions.IncludeDisabled
		});
		var userEntities = query.ToEntityArray(Allocator.Temp);
		foreach (var entity in userEntities)
		{
			var userData = Core.EntityManager.GetComponentData<User>(entity);
			PlayerData playerData = new PlayerData(userData.CharacterName, userData.PlatformId, userData.IsConnected, entity, userData.LocalCharacter._Entity);

			NamePlayerCache.TryAdd(userData.CharacterName.ToString().ToLower(), playerData);
			SteamPlayerCache.TryAdd(userData.PlatformId, playerData);
		}


		var onlinePlayers = NamePlayerCache.Values.Where(p => p.IsOnline).Select(p => $"\t{p.CharacterName}");
		Core.Log.LogWarning($"Player Cache Created with {NamePlayerCache.Count} entries total, listing {onlinePlayers.Count()} online:");
		Core.Log.LogWarning(string.Join("\n", onlinePlayers));
	}

	internal void UpdatePlayerCache(Entity userEntity, string oldName, string newName, bool forceOffline = false)
	{
		var userData = Core.EntityManager.GetComponentData<User>(userEntity);
		NamePlayerCache.Remove(oldName.ToLower());

		if (forceOffline) userData.IsConnected = false;
		PlayerData playerData = new PlayerData(newName, userData.PlatformId, userData.IsConnected, userEntity, userData.LocalCharacter._Entity);

		NamePlayerCache[newName.ToLower()] = playerData;
		SteamPlayerCache[userData.PlatformId] = playerData;
	}

	internal bool RenamePlayer(Entity userEntity, Entity charEntity, FixedString64 newName)
	{
		var des = Core.Server.GetExistingSystem<DebugEventsSystem>();
		var networkId = Core.EntityManager.GetComponentData<NetworkId>(userEntity);
		var userData = Core.EntityManager.GetComponentData<User>(userEntity);
		var renameEvent = new RenameUserDebugEvent
		{
			NewName = newName,
			Target = networkId
		};
		var fromCharacter = new FromCharacter
		{
			User = userEntity,
			Character = charEntity
		};

		des.RenameUser(fromCharacter, renameEvent);
		UpdatePlayerCache(userEntity, userData.CharacterName.ToString(), newName.ToString());
		return true;
	}
	public static IEnumerable<Entity> GetUsersOnline()
	{

		NativeArray<Entity> _userEntities = VWorld.Server.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp);
		int len = _userEntities.Length;
		for (int i = 0; i < len; ++i)
		{
			if (_userEntities[i].Read<User>().IsConnected)
				yield return _userEntities[i];
		}

	}
}
