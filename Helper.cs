using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.Shared;
using Stunlock.Core;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KindredExtract;

// This is an anti-pattern, move stuff away from Helper not into it
internal static partial class Helper
{
    public static AdminAuthSystem adminAuthSystem = Core.Server.GetExistingSystemManaged<AdminAuthSystem>();

    public static PrefabGUID GetPrefabGUID(Entity entity)
    {
        var entityManager = Core.EntityManager;
        PrefabGUID guid;
        try
        {
            guid = entityManager.GetComponentData<PrefabGUID>(entity);
        }
        catch
        {
            guid = new PrefabGUID(0);
        }
        return guid;
    }


    public static Entity AddItemToInventory(Entity recipient, PrefabGUID guid, int amount)
    {
        try
        {
            var gameData = Core.Server.GetExistingSystemManaged<GameDataSystem>();
            var itemSettings = AddItemSettings.Create(Core.EntityManager, gameData.ItemHashLookupMap);
            var inventoryResponse = InventoryUtilitiesServer.TryAddItem(itemSettings, recipient, guid, amount);

            return inventoryResponse.NewEntity;
        }
        catch (System.Exception e)
        {
            Core.LogException(e);
        }
        return new Entity();
    }

    public static NativeArray<Entity> GetEntitiesByComponentType<T1>(bool includeAll = false, bool includeDisabled = false, bool includeSpawn = false, bool includePrefab = false, bool includeDestroyed = false)
    {
        EntityQueryOptions options = EntityQueryOptions.Default;
        if (includeAll) options |= EntityQueryOptions.IncludeAll;
        if (includeDisabled) options |= EntityQueryOptions.IncludeDisabled;
        if (includeSpawn) options |= EntityQueryOptions.IncludeSpawnTag;
        if (includePrefab) options |= EntityQueryOptions.IncludePrefab;
        if (includeDestroyed) options |= EntityQueryOptions.IncludeDestroyTag;

        EntityQueryDesc queryDesc = new()
        {
            All = new ComponentType[] { new(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite) },
            Options = options
        };

        var query = Core.EntityManager.CreateEntityQuery(queryDesc);

        var entities = query.ToEntityArray(Allocator.Temp);
        return entities;
    }

    public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2>(bool includeAll = false, bool includeDisabled = false, bool includeSpawn = false, bool includePrefab = false, bool includeDestroyed = false)
    {
        EntityQueryOptions options = EntityQueryOptions.Default;
        if (includeAll) options |= EntityQueryOptions.IncludeAll;
        if (includeDisabled) options |= EntityQueryOptions.IncludeDisabled;
        if (includeSpawn) options |= EntityQueryOptions.IncludeSpawnTag;
        if (includePrefab) options |= EntityQueryOptions.IncludePrefab;
        if (includeDestroyed) options |= EntityQueryOptions.IncludeDestroyTag;

        EntityQueryDesc queryDesc = new()
        {
            All = new ComponentType[] { new(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite), new(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite) },
            Options = options
        };

        var query = Core.EntityManager.CreateEntityQuery(queryDesc);

        var entities = query.ToEntityArray(Allocator.Temp);
        return entities;
    }

    public static void RepairGear(Entity Character, bool repair = true)
    {
        Equipment equipment = Character.Read<Equipment>();
        NativeList<Entity> equippedItems = new(Allocator.Temp);
        equipment.GetAllEquipmentEntities(equippedItems);
        foreach (var equippedItem in equippedItems)
        {
            if (equippedItem.Has<Durability>())
            {
                var durability = equippedItem.Read<Durability>();
                if (repair)
                {
                    durability.Value = durability.MaxDurability;
                }
                else
                {
                    durability.Value = 0;
                }

                equippedItem.Write(durability);
            }
        }
        equippedItems.Dispose();

        for (int i = 0; i < 36; i++)
        {
            if (InventoryUtilities.TryGetItemAtSlot(Core.EntityManager, Character, i, out InventoryBuffer item))
            {
                var itemEntity = item.ItemEntity._Entity;
                if (itemEntity.Has<Durability>())
                {
                    var durability = itemEntity.Read<Durability>();
                    if (repair)
                    {
                        durability.Value = durability.MaxDurability;
                    }
                    else
                    {
                        durability.Value = 0;
                    }

                    itemEntity.Write(durability);
                }
            }
        }
    }

    public static bool FindClan(string clanName, out Entity clanEntity)
    {
        var clans = Helper.GetEntitiesByComponentType<ClanTeam>().ToArray();
        var matchedClans = clans.Where(x => x.Read<ClanTeam>().Name.ToString().ToLower() == clanName.ToLower());

        foreach (var clan in matchedClans)
        {
            var members = Core.EntityManager.GetBuffer<ClanMemberStatus>(clan);
            if (members.Length == 0) continue;
            clanEntity = clan;
            return true;
        }
        clanEntity = new Entity();
        return false;
    }
}
