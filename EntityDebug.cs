
using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.Network;
using ProjectM.Shared;
using ProjectM.Terrain;
using ProjectM.Tiles;
using Stunlock.Core;
using Stunlock.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.Entities;
using UnityEngine;

namespace KindredExtract;

internal class EntityDebug
{
    public delegate string ComponentExtractor(Entity entity);
    static Dictionary<TypeIndex, ComponentExtractor> componentExtractors = new ();

    public static void RegisterExtractor<T>() where T : struct
    {
        try
        {
            var ct = new ComponentType(Il2CppType.Of<T>());

            if (ct.IsZeroSized)
            {
                componentExtractors.Add(ct.TypeIndex, (entity) => "  " + typeof(T).ToString() + "\n");
                return;
            }

            if (ct.IsBuffer)
            {
                componentExtractors.Add(ct.TypeIndex, (entity) =>
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"  {typeof(T).ToString()}");
                    var buffer = ReadBuffer<T>(entity);
                    for (int i = 0; i < Mathf.Min(buffer.Length, 36); i++)
                    {
                        sb.AppendLine($"   [{i}]");
                        sb.AppendLine(RetrieveFields(buffer[i]));
                    }

                    if (buffer.Length > 36)
                        sb.AppendLine($"   {buffer.Length} total elements but only showing the first 36");

                    return sb.ToString();
                });
                return;
            }

            componentExtractors.Add(ct.TypeIndex, (entity) =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"  {typeof(T).ToString()}");
                sb.AppendLine(RetrieveFields(entity.Read<T>()));
                return sb.ToString();
            });
        }
        catch (Exception e)
        {
            Core.Log.LogError($"Failed to register extractor for {typeof(T).ToString()}\n{e.Message}");
        }
    }

    public static string RetrieveComponentData(Entity entity)
    {
		var entityManager = Core.EntityManager;
        var componentData = new StringBuilder();

        if(!entityManager.Exists(entity))
        {
            componentData.AppendLine("Entity does not exist");
            return componentData.ToString();
        }

        if (entity.Has<User>())
        {
            componentData.AppendLine($"User {entity.Read<User>().CharacterName} - Entity({entity.Index}:{entity.Version})");
        }
        else if (entity.Has<PlayerCharacter>())
        {
            componentData.AppendLine($"Player {entity.Read<PlayerCharacter>().Name} - Entity({entity.Index}:{entity.Version})");
        }
        else if (entity.Has<PrefabGUID>())
        {
            if(entity.Has<Prefab>())
                componentData.AppendLine($"Prefab {entity.Read<PrefabGUID>().LookupName()}");
            else
                componentData.AppendLine($"Prefab {entity.Read<PrefabGUID>().LookupName()} - Entity({entity.Index}:{entity.Version})");
        }
        else
        {
            componentData.AppendLine($"Entity({entity.Index}:{entity.Version})");
        }

        componentData.AppendLine("Components");

        var allTypes = entityManager.GetComponentTypes(entity, Unity.Collections.Allocator.Persistent);
        foreach(var t in allTypes)
        {
            if(componentExtractors.TryGetValue(t.TypeIndex, out var extractor))
            {
                try
                {
                    componentData.Append(extractor(entity));
                }
                catch (Exception e)
                {
                    componentData.AppendLine($"  {t} failed to extract: {e.Message}");
                }
            }
            else
            {
                componentData.AppendLine($"  {t} isn't handled");
            }
        }
        allTypes.Dispose();

        return componentData.ToString();
    }

    static string RetrieveFields<T>(T component, string prepend= "    ")
    {
        StringBuilder fields = new();
        FieldInfo[] fieldInfos = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fieldInfos)
        {
            // ModInt ModBool ModFloat3 PrefabGUID
            if (field.FieldType == typeof(ModifiableFloat))
            {
                var v = (ModifiableFloat)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Value}");
            }
            else if (field.FieldType == typeof(ModifiableInt))
            {
                var v = (ModifiableInt)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Value}");
            }
            else if (field.FieldType == typeof(ModifiableBool))
            {
                var v = (ModifiableBool)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Value}");
            }
            else if (field.FieldType == typeof(ModifiableFloat3))
            {
                var v = (ModifiableFloat3)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Value}");
            }
            else if (field.FieldType == typeof(PrefabGUID))
            {
                var v = (PrefabGUID)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.LookupName()}");
            }
            else if (field.FieldType == typeof(ModificationData<Single>))
            {
                var v = (ModificationData<Single>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.Id.ToString()} and priority {v.Priority}");
            }
            else if (field.FieldType == typeof(ModificationData<Int32>))
            {
                var v = (ModificationData<Int32>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.Id.ToString()} and priority {v.Priority}");
            }
            else if (field.FieldType == typeof(ModificationData<Int64>))
            {
                var v = (ModificationData<Int64>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.Id.ToString()} and priority {v.Priority}");
            }
            else if (field.FieldType == typeof(ModificationData<bool>))
            {
                var v = (ModificationData<bool>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.Id.ToString()} and priority {v.Priority}");
            }
            else if (field.FieldType == typeof(GameplayEventId))
            {
                var v = (GameplayEventId)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.GameplayEventType} - {v.EventId}");
            }
            else if (field.FieldType == typeof(MapZoneId))
            {
                var v = (MapZoneId)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: MapZoneID(ZoneId: {v.ZoneId}, ZoneIndex: {v.ZoneIndex}, Chunk: ({v.ChunkCoordinate.X}, {v.ChunkCoordinate.Y}))");
            }
            else if (field.FieldType == typeof(BlobAssetReference<ConditionBlob>))
            {
                try
                {
                    var b = (BlobAssetReference<ConditionBlob>)field.GetValue(component);
                    if (b.IsCreated)
                    {
                        unsafe
                        {
                            var v = *(ConditionBlob*)b.m_data.m_Ptr;

                            fields.AppendLine(prepend + $"{field.Name}: ConditionBlob");
                            fields.AppendLine(prepend + " ConditionInfo");
                            fields.AppendLine(prepend + $"  Prefab {v.Info.Prefab.ToString()}");
                            fields.AppendLine(prepend + $"  Component {v.Info.Component.ToString()}");
                            fields.AppendLine(prepend + " ConditionalElements");
                            /*for (int i = 0; i < v.Conditionals.Length; ++i)
                            {
                                ConditionElement e = ((ConditionElement*)v.Conditionals.GetUnsafePtr())[i];
                                fields.AppendLine(prepend + $"  [{i}] Source: {e.Source} Success: {e.SuccessIndex} Failure: {e.FailureIndex} Union: {e.Union}  {e}");
                            }*/
                        }
                    }
                    else
                    {
                        fields.AppendLine(prepend + $"{field.Name}: None");
                    }
                }
                catch
                {
                    fields.AppendLine(prepend + $"{field.Name}: {field.GetValue(component).ToString()}");
                }
            }
            else if (field.FieldType == typeof(BlobString))
            {
                var v = (BlobString)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.ToString()}");
            }
            else if (field.FieldType == typeof(Entity))
            {
                var v = (Entity)field.GetValue(component);
                var entityString = $"Entity({v.Index}:{v.Version})";
                if (v.Has<User>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: User {v.Read<User>().CharacterName} - {entityString}");
                }
                else if (v.Has<PlayerCharacter>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: Player {v.Read<PlayerCharacter>().Name} - {entityString}");
                }
                else if (v.Has<PrefabGUID>())
                {
                    if (v.Has<Prefab>())
                        fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {entityString}");
                    else
                        fields.AppendLine(prepend + $"{field.Name}: Entity {v.Read<PrefabGUID>().LookupName()} - {entityString}");
                }
                else
                {
                    fields.AppendLine(prepend + $"{field.Name}: {entityString}");
                }
            }
            else if (field.FieldType == typeof(NetworkedEntity))
            {
                var v = ((NetworkedEntity)field.GetValue(component)).GetEntityOnServer();
                var networkEntityString = $"NetworkedEntity({v.Index}:{v.Version})";
                if (v.Has<User>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: User {v.Read<User>().CharacterName} - {networkEntityString}");
                }
                else if (v.Has<PlayerCharacter>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: Player {v.Read<PlayerCharacter>().Name} - {networkEntityString}");
                }
                else if (v.Has<PrefabGUID>())
                {
                    if (v.Has<Prefab>())
                        fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {networkEntityString}");
                    else
                        fields.AppendLine(prepend + $"{field.Name}: Entity {v.Read<PrefabGUID>().LookupName()} - {networkEntityString}");
                }
                else
                {
                    fields.AppendLine(prepend + $"{field.Name}: {networkEntityString}");
                }
            }
            else if (field.FieldType == typeof(ProjectM.ModifiableEntity))
            {
                var v = ((ModifiableEntity)field.GetValue(component)).Value;
                var entityString = $"ModifiableEntity({v.Index}:{v.Version})";
                if (v.Has<User>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: User {v.Read<User>().CharacterName} - {entityString}");
                }
                else if (v.Has<PlayerCharacter>())
                {
                    fields.AppendLine(prepend + $"{field.Name}: Player {v.Read<PlayerCharacter>().Name} - {entityString}");
                }
                else if (v.Has<PrefabGUID>())
                {
                    if (v.Has<Prefab>())
                        fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {entityString}");
                    else
                        fields.AppendLine(prepend + $"{field.Name}: Entity {v.Read<PrefabGUID>().LookupName()} - {entityString}");
                }
                else
                {
                    fields.AppendLine(prepend + $"{field.Name}: {entityString}");
                }
            }
            else if (field.FieldType == typeof(ModificationId))
            {
                var v = (ModificationId)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {(v.IsEmpty() ? "Unset" : v.Id)}");
            }
            else if (field.FieldType == typeof(CreateGameplayEventsOnSpawn))
            {
                var v = (CreateGameplayEventsOnSpawn)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.EventId.EventId} {v.EventId.GameplayEventType} - {v.Target}");
            }
            else if (field.FieldType == typeof(GameplayEventId))
            {
                var v = (GameplayEventId)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: (EventId: {v.EventId}, GameplayEventType: {v.GameplayEventType})");
            }
            else if (field.FieldType == typeof(TilePivotSettings))
            {
                var v = (TilePivotSettings)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: {v.PivotType} - {v.CustomPivotPoint}");
            }
            else if (field.FieldType == typeof(TileDatas<CollisionData>))
            {
                var v = (TileDatas<CollisionData>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: [");
                /*for (var i = 0; i < v.Tiles.Length; ++i)
                {
                    unsafe
                    {
                        var tile = ((TileDatas<CollisionData>.DataStruct*)v.Tiles.GetUnsafePtr())[i];
                        fields.AppendLine(prepend + $"  ({tile.Tile.x}, {tile.Tile.y}) - {tile.Data.CollisionFlags}");
                    }
                }*/
                fields.AppendLine(prepend + $"]");
            }
            else if (field.FieldType == typeof(TileDatas<TileHeightData>))
            {
                var v = (TileDatas<TileHeightData>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: [");
                /*for (var i = 0; i < v.Tiles.Length; ++i)
                {
                    unsafe
                    {
                        var tile = ((TileDatas<TileHeightData>.DataStruct*)v.Tiles.GetUnsafePtr())[i];
                        fields.AppendLine(prepend + $"  ({tile.Tile.x}, {tile.Tile.y}) - {tile.Data.Height}");
                    }
                }*/
                fields.AppendLine(prepend + $"]");
            }
            else if (field.FieldType == typeof(TileDatas<SurfaceFluffData>))
            {
                var v = (TileDatas<SurfaceFluffData>)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: [");
                /*for (var i = 0; i < v.Tiles.Length; ++i)
                {
                    unsafe
                    {
                        var tile = ((TileDatas<SurfaceFluffData>.DataStruct*)v.Tiles.GetUnsafePtr())[i];
                        fields.AppendLine(prepend + $"  ({tile.Tile.x}, {tile.Tile.y}) - {tile.Data.FluffAllowance}");
                    }
                }*/
                fields.AppendLine(prepend + $"]");
            }
            else if (field.FieldType == typeof(BlobAssetReference<WallpaperStyleBlob>))
            {
                var b = (BlobAssetReference<WallpaperStyleBlob>)field.GetValue(component);
                if (b.IsCreated)
                {
                    unsafe
                    {
                        var v = *(WallpaperStyleBlob*)b.m_data.m_Ptr;

                        fields.AppendLine(prepend + $"{field.Name}: WallpaperStyleBlob");
                        fields.AppendLine(prepend + $"  Styles: [");
                        fields.AppendLine(prepend + $"    {v.Styles.Length} entries");
                        /*for (var i = 0; i < v.Styles.Length; ++i)
                        {
                            unsafe
                            {
                                var style = ((WallpaperStyleData*)v.Styles.GetUnsafePtr())[i];
                                fields.AppendLine(prepend + $"    {style.ParentBlueprintGUID.LookupName()} - {style.MeshVariationIndex} {style.MeshVariationGUID.LookupName()}");
                            }
                        }*/
                        fields.AppendLine(prepend + $"  ]");

                        fields.AppendLine(prepend + $"  MeshVariationsByIndex: [");
                        fields.AppendLine(prepend + $"    {v.MeshVariationsByIndex.Length} entries");
                        /*for (var i = 0; i < v.MeshVariationsByIndex.Length; ++i)
                        {
                            unsafe
                            {
                                var meshVariation = ((PrefabGUID*)v.MeshVariationsByIndex.GetUnsafePtr())[i];
                                fields.AppendLine(prepend + $"    [{i}] {meshVariation.LookupName()}");
                            }
                        }*/
                        fields.AppendLine(prepend + $"  ]");
                    }
                }
                else
                {
                    fields.AppendLine(prepend + $"{field.Name}: None");
                }
            }
            else if (field.FieldType == typeof(Il2CppSystem.Nullable_Unboxed<HeightPlacementConfig>))
            {
                var v = (Il2CppSystem.Nullable_Unboxed<HeightPlacementConfig>)field.GetValue(component);
                if (v.HasValue)
                {
                    fields.AppendLine(prepend + $"{field.Name}:");
                    fields.AppendLine(RetrieveFields(v.Value, prepend + "  "));
                }
                else
                {
                    fields.AppendLine(prepend + $"{field.Name}: None");
                }
            }
            else if (field.FieldType == typeof(WallpaperOrientation))
            {
                var v = (WallpaperOrientation)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}:");
                fields.Append(RetrieveFields(v, prepend + "  "));
            }
            else if (field.FieldType == typeof(BlobAssetReference<SpawnChainBlobAsset>))
            {
                var b = (BlobAssetReference<SpawnChainBlobAsset>)field.GetValue(component);
                if (b.IsCreated)
                {
                    unsafe
                    {
                        var v = *(SpawnChainBlobAsset*)b.m_data.m_Ptr;
                        fields.AppendLine(prepend + $"{field.Name}: SpawnChainBlobAsset(main {v.MainElementIndex} out of)");
                    }
                }
            }
            else if (field.FieldType == typeof(Unity.Physics.Aabb))
            {
                var v = (Unity.Physics.Aabb)field.GetValue(component);
                fields.AppendLine(prepend + $"{field.Name}: Aaab({v.Min} to {v.Max})");
            }
            else if (field.FieldType == typeof(LocalizationKey))
            {
                var v = (LocalizationKey)field.GetValue(component);
                var guid = v.Key.ToGuid().ToString();
                var s = Core.Localization.GetLocalization(guid);
                fields.AppendLine(prepend + $"{field.Name}: {guid} - {s}");
            }
            else if (field.FieldType.AssemblyQualifiedName.StartsWith("System"))
            {
                fields.AppendLine(prepend + $"{field.Name}: {field.GetValue(component)}");
            }
            else
            {
                fields.AppendLine(prepend + $"{field.Name}: {field.FieldType} {field.GetValue(component)}");
            }
        }

        return fields.ToString();
	}

	unsafe static T[] ReadBuffer<T>(Entity entity) where T : struct
	{
		var returning = new List<T>();
		try
		{
			var b = Core.EntityManager.GetBuffer<T>(entity);
			foreach (var a in b)
			{
				returning.Add(a);
			}
		}
		catch (Exception)
		{
			try
			{
				var b = Core.EntityManager.GetBufferReadOnly<T>(entity);
				foreach (var a in b)
				{
					returning.Add(a);
				}
			}
			catch (Exception)
			{

			}
		}

        return returning.ToArray();
	}
}
