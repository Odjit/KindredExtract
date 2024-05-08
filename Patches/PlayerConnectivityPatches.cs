using HarmonyLib;
using KindredExtract.Data;
using ProjectM;
using ProjectM.Network;
using Stunlock.Core;
using Stunlock.Network;
using System;
using Unity.Collections;

namespace KindredExtract.Patches;


[HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserConnected))]
public static class OnUserConnected_Patch
{
	public static void Postfix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId)
	{
		if (Core.Players == null) Core.InitializeAfterLoaded();
		try
		{
			var em = __instance.EntityManager;
			var userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
			var serverClient = __instance._ApprovedUsersLookup[userIndex];
			var userEntity = serverClient.UserEntity;
			var userData = __instance.EntityManager.GetComponentData<User>(userEntity);
			bool isNewVampire = userData.CharacterName.IsEmpty;

			if (!isNewVampire)
			{
				var playerName = userData.CharacterName.ToString();
				Core.Players.UpdatePlayerCache(userEntity, playerName, playerName);
				Core.Log.LogInfo($"Player {playerName} connected");
			}
		}
		catch (Exception e)
		{
			Core.Log.LogError($"Failure in {nameof(ServerBootstrapSystem.OnUserConnected)}\nMessage: {e.Message} Inner:{e.InnerException?.Message}\n\nStack: {e.StackTrace}\nInner Stack: {e.InnerException?.StackTrace}");
		}
	}
}

[HarmonyPatch(typeof(ServerBootstrapSystem), nameof(ServerBootstrapSystem.OnUserDisconnected))]
public static class OnUserDisconnected_Patch
{
	private static void Prefix(ServerBootstrapSystem __instance, NetConnectionId netConnectionId, ConnectionStatusChangeReason connectionStatusReason, string extraData)
	{
		if (Core.Players == null) Core.InitializeAfterLoaded();
		try
		{
			var userIndex = __instance._NetEndPointToApprovedUserIndex[netConnectionId];
			var serverClient = __instance._ApprovedUsersLookup[userIndex];
			var userData = __instance.EntityManager.GetComponentData<User>(serverClient.UserEntity);
			bool isNewVampire = userData.CharacterName.IsEmpty;

			if (!isNewVampire)
			{
				var playerName = userData.CharacterName.ToString();
				Core.Players.UpdatePlayerCache(serverClient.UserEntity, playerName, playerName, true);

				Core.Log.LogInfo($"Player {playerName} disconnected");
			}
		}
		catch { };
	}
}

[HarmonyPatch(typeof(Destroy_TravelBuffSystem), nameof(Destroy_TravelBuffSystem.OnUpdate))]
public class Destroy_TravelBuffSystem_Patch
{
	private static void Postfix(Destroy_TravelBuffSystem __instance)
	{
		if (Core.Players == null) Core.InitializeAfterLoaded();
		var entities = __instance.__query_615927195_0.ToEntityArray(Allocator.Temp);
		foreach (var entity in entities)
		{
			PrefabGUID GUID = __instance.EntityManager.GetComponentData<PrefabGUID>(entity);

			// This buff is involved when exiting the Coffin when creating a new character
			// previous to that, the connected user doesn't have a Character or name.
			if (GUID.Equals(Prefabs.AB_Interact_TombCoffinSpawn_Travel))
			{
				var owner = __instance.EntityManager.GetComponentData<EntityOwner>(entity).Owner;
				if (!__instance.EntityManager.HasComponent<PlayerCharacter>(owner)) return;

				var userEntity = __instance.EntityManager.GetComponentData<PlayerCharacter>(owner).UserEntity;
				var playerName = __instance.EntityManager.GetComponentData<User>(userEntity).CharacterName.ToString();

				Core.Players.UpdatePlayerCache(userEntity, playerName, playerName);

				Core.Log.LogInfo($"Player {playerName} created");
			}
		}

	}
}
