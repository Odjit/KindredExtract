using ProjectM;
using ProjectM.Network;
using Stunlock.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using VampireCommandFramework;

namespace KindredExtract.Commands;
[CommandGroup("entity", "e")]
internal static class EntityCommands
{
	[Command("teleport", "tp", description: "Teleport to the specified entity.", adminOnly: true)]
	public static void TeleportToEntity(ChatCommandContext ctx, int entityId, int version = 1)
	{
		var playerEntity = ctx.Event.SenderCharacterEntity;

		var targetEntity = new Entity { Index = entityId, Version = version };

		if (!Core.EntityManager.Exists(targetEntity))
		{
			ctx.Reply("Specified entity doesn't exist");
			return;
		}

		var pos = targetEntity.Read<Translation>().Value;

		var entity = Core.EntityManager.CreateEntity(Core.EntityManager.CreateArchetype(new ComponentType[]
				{
					ComponentType.ReadWrite<FromCharacter>(),
					ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
				}
			));

		Core.EntityManager.SetComponentData<FromCharacter>(entity, new FromCharacter()
		{
			User = ctx.Event.SenderUserEntity,
			Character = ctx.Event.SenderCharacterEntity
		});

		Core.EntityManager.SetComponentData<PlayerTeleportDebugEvent>(entity, new PlayerTeleportDebugEvent()
		{
			Position = new float3(pos.x, pos.y, pos.z),
			Target = PlayerTeleportDebugEvent.TeleportTarget.Self
		});

		var name = $"Entity({entityId}:{version})";
		if (targetEntity.Has<PrefabGUID>())
		{
			name = targetEntity.Read<PrefabGUID>().LookupName();
		}

		ctx.Reply($"Teleported to {name} at {pos}");
	}

	[Command("despawn", "d", description: "Despawn the specified entity.", adminOnly: true)]
	public static void DespawnEntity(ChatCommandContext ctx, int entityId, int version = 1)
	{
		var targetEntity = new Entity { Index = entityId, Version = version };

		if (!Core.EntityManager.Exists(targetEntity))
		{
			ctx.Reply("Specified entity doesn't exist");
			return;
		}

		var name = $"Entity({entityId}:{version})";
		if (targetEntity.Has<PrefabGUID>())
		{
			name = targetEntity.Read<PrefabGUID>().LookupName();
		}

		StatChangeUtility.KillEntity(Core.EntityManager, targetEntity, ctx.Event.SenderCharacterEntity, Time.time, StatChangeReason.Default, true);

		ctx.Reply($"Despawned {name}");
	}

	[Command("destroy", "del", description: "Destroy the specified entity.", adminOnly: true)]
	public static void DestroyEntity(ChatCommandContext ctx, int entityId, int version = 1)
	{
		var targetEntity = new Entity { Index = entityId, Version = version };

		if (!Core.EntityManager.Exists(targetEntity))
		{
			ctx.Reply("Specified entity doesn't exist");
			return;
		}

		var name = $"Entity({entityId}:{version})";
		if (targetEntity.Has<PrefabGUID>())
		{
			name = targetEntity.Read<PrefabGUID>().LookupName();
		}

		targetEntity.Add<DestroyTag>();
		if (!targetEntity.Has<DestroyData>()) targetEntity.Add<DestroyData>();
		targetEntity.Write(new DestroyData { DestroyReason = DestroyReason.Default });

		ctx.Reply($"Destroyed {name}");
	}

	[Command("topcount", "tc", description: "Counts the top entities in the world", adminOnly: true)]
    public static void TopEntityCount(ChatCommandContext ctx, int topNum=10, string filter=null)
    {
        foreach ((var prefabGUID, var count) in Core.CountPrefabs(topNum, filter))
        {
            ctx.Reply($"{prefabGUID.LookupName()} - {count}");
        }
    }
}
