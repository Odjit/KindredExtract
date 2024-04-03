
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ProjectM;
using ProjectM.Behaviours;
using ProjectM.CastleBuilding;
using ProjectM.CastleBuilding.AssetSwapping;
using ProjectM.CastleBuilding.Placement;
using ProjectM.Gameplay;
using ProjectM.Gameplay.Scripting;
using ProjectM.Hybrid;
using ProjectM.LightningStorm;
using ProjectM.Network;
using ProjectM.Pathfinding;
using ProjectM.Physics;
using ProjectM.Roofs;
using ProjectM.Scripting;
using ProjectM.Sequencer;
using ProjectM.Shared;
using ProjectM.Terrain;
using ProjectM.Tiles;
using Stunlock.Sequencer;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using static ProjectM.AbilityGroupSlot;
using static ProjectM.CrowdednessDropTableSettingsAsset;
using static ProjectM.HitColliderCast;
using static ProjectM.SharedModifiableFunctions;
using static ProjectM.SpawnChainData;
using Il2CppInterop.Runtime;
using System;
using static UnityEngine.Rendering.DebugUI;
using Unity.Entities.UniversalDelegates;
using Unity.Collections;

namespace KindredExtract;

internal class EntityDebug
{
    public static string RetrieveComponentData(Entity entity)
    {
		var entityManager = Core.EntityManager;
        var componentData = new StringBuilder();

        if(!entityManager.Exists(entity))
        {
            componentData.AppendLine("Entity does not exist");
            return componentData.ToString();
        }

        var allTypes = entityManager.GetComponentTypes(entity, Unity.Collections.Allocator.Persistent);
        var checkedTypes = new List<ComponentType>();

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
			componentData.AppendLine($"Prefab {entity.Read<PrefabGUID>().LookupName()} - Entity({entity.Index}:{entity.Version})");
		}
		else
		{
			componentData.AppendLine($"Entity({entity.Index}:{entity.Version})");
		}

		componentData.AppendLine("Components");

        if (entity.Has<AbilityBarInitializationState>())
        {
            componentData.AppendLine("  AbilityBarInitializationState");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityBar_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityBarInitializationState>()));
        }
        if (entity.Has<AbilityBar_Shared>())
        {
            componentData.AppendLine("  AbilityBar_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityBar_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityBar_Shared>()));
        }
        if (entity.Has<AbilityCastCondition>())
        {
            componentData.AppendLine("  AbilityCastCondition");
            var buffer = ReadBuffer<AbilityCastCondition>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityCastCondition>()));
        }
        if (entity.Has<AbilityCastTimeData>())
        {
            componentData.AppendLine("  AbilityCastTimeData");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityCastTimeData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityCastTimeData>()));
        }
        if (entity.Has<AbilityCooldownData>())
        {
            componentData.AppendLine("  AbilityCooldownData");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityCooldownData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityCooldownData>()));
        }
        if (entity.Has<AbilityCooldownState>())
        {
            componentData.AppendLine("  AbilityCooldownState");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityCooldownState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityCooldownState>()));
        }
        if (entity.Has<AbilityGroupSlot>())
        {
            componentData.AppendLine("  AbilityGroupSlot");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityGroupSlot>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityGroupSlot>()));
        }
        if (entity.Has<AbilityGroupSlotBuffer>())
        {
            componentData.AppendLine("  AbilityGroupSlotBuffer");
            var buffer = entityManager.GetBufferReadOnly<AbilityGroupSlotBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityGroupSlotBuffer>()));
        }
        if (entity.Has<AbilityGroupStartAbilitiesBuffer>())
        {
            componentData.AppendLine("  AbilityGroupStartAbilitiesBuffer");
            /*var buffer = entityManager.GetBufferReadOnly<AbilityGroupStartAbilitiesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }*/
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityGroupStartAbilitiesBuffer>()));
        }
        if (entity.Has<AbilityGroupState>())
        {
            componentData.AppendLine("  AbilityGroupState");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityGroupState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityGroupState>()));
        }
        if (entity.Has<AbilityIgnoreSettings>())
        {
            componentData.AppendLine("  AbilityIgnoreSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityIgnoreSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityIgnoreSettings>()));
        }
        if (entity.Has<AbilityOwner>())
        {
            componentData.AppendLine("  AbilityOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityOwner>()));
        }
        if (entity.Has<AbilityPriority>())
        {
            componentData.AppendLine("  AbilityPriority");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityPriority>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityPriority>()));
        }
        if (entity.Has<AbilityProjectileFanOnGameplayEvent_DataServer>())
        {
            componentData.AppendLine("  AbilityProjectileFanOnGameplayEvent_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityProjectileFanOnGameplayEvent_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityProjectileFanOnGameplayEvent_DataServer>()));
        }
        if (entity.Has<AbilitySpawnPrefabOnCast>())
        {
            componentData.AppendLine("  AbilitySpawnPrefabOnCast");
            var buffer = ReadBuffer<AbilitySpawnPrefabOnCast>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilitySpawnPrefabOnCast>()));
        }
        if (entity.Has<AbilityState>())
        {
            componentData.AppendLine("  AbilityState");
            componentData.AppendLine(RetrieveFields(entity.Read<AbilityState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityState>()));
        }
        if (entity.Has<AbilityStateBuffer>())
        {
            componentData.AppendLine("  AbilityStateBuffer");
            var buffer = ReadBuffer<AbilityStateBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbilityStateBuffer>()));
        }
        if (entity.Has<AbsorbBuff>())
        {
            componentData.AppendLine("  AbsorbBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<AbsorbBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbsorbBuff>()));
        }
        if (entity.Has<AbsorbCapStackModifier>())
        {
            componentData.AppendLine("  AbsorbCapStackModifier");
            componentData.AppendLine(RetrieveFields(entity.Read<AbsorbCapStackModifier>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AbsorbCapStackModifier>()));
        }
        if (entity.Has<AchievementClaimedElement>())
        {
            componentData.AppendLine("  AchievementClaimedElement");
            var buffer = ReadBuffer<AchievementClaimedElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementClaimedElement>()));
        }
        if (entity.Has<AchievementData>())
        {
            componentData.AppendLine("  AchievementData");
            componentData.AppendLine(RetrieveFields(entity.Read<AchievementData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementData>()));
        }
        if (entity.Has<AchievementInProgressElement>())
        {
            componentData.AppendLine("  AchievementInProgressElement");
            var buffer = ReadBuffer<AchievementInProgressElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementInProgressElement>()));
        }
        if (entity.Has<AchievementOwner>())
        {
            componentData.AppendLine("  AchievementOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<AchievementOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementOwner>()));
        }
        if (entity.Has<AchievementSubTaskData>())
        {
            componentData.AppendLine("  AchievementSubTaskData");
            componentData.AppendLine(RetrieveFields(entity.Read<AchievementSubTaskData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementSubTaskData>()));
        }
        if (entity.Has<AchievementSubTaskEntry>())
        {
            componentData.AppendLine("  AchievementSubTaskEntry");
            var buffer = ReadBuffer<AchievementSubTaskEntry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AchievementSubTaskEntry>()));
        }
        if (entity.Has<ActiveChildElement>())
        {
            componentData.AppendLine("  ActiveChildElement");
            componentData.AppendLine(RetrieveFields(entity.Read<ActiveChildElement>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ActiveChildElement>()));
        }
        if (entity.Has<ActiveServantMission>())
        {
            componentData.AppendLine("  ActiveServantMission");
            var buffer = ReadBuffer<ActiveServantMission>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ActiveServantMission>()));
        }
        if (entity.Has<AdminUser>())
        {
            componentData.AppendLine("  AdminUser");
            componentData.AppendLine(RetrieveFields(entity.Read<AdminUser>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AdminUser>()));
        }
        if (entity.Has<AffectPrisonerWithToxic>())
        {
            componentData.AppendLine("  AffectPrisonerWithToxic");
            componentData.AppendLine(RetrieveFields(entity.Read<AffectPrisonerWithToxic>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AffectPrisonerWithToxic>()));
        }
        if (entity.Has<Age>())
        {
            componentData.AppendLine("  Age");
            componentData.AppendLine(RetrieveFields(entity.Read<Age>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Age>()));
        }
        if (entity.Has<AggroBuffer>())
        {
            componentData.AppendLine("  AggroBuffer");
            var buffer = ReadBuffer<AggroBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroBuffer>()));
        }
        if (entity.Has<AggroCandidateBufferElement>())
        {
            componentData.AppendLine("  AggroCandidateBufferElement");
            var buffer = ReadBuffer<AggroCandidateBufferElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroCandidateBufferElement>()));
        }
        if (entity.Has<AggroConsumer>())
        {
            componentData.AppendLine("  AggroConsumer");
            componentData.AppendLine(RetrieveFields(entity.Read<AggroConsumer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroConsumer>()));
        }
        if (entity.Has<AggroDamageHistoryBufferElement>())
        {
            componentData.AppendLine("  AggroDamageHistoryBufferElement");
            var buffer = ReadBuffer<AggroDamageHistoryBufferElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroDamageHistoryBufferElement>()));
        }
        if (entity.Has<AggroDamageHistoryConfig>())
        {
            componentData.AppendLine("  AggroDamageHistoryConfig");
            componentData.AppendLine(RetrieveFields(entity.Read<AggroDamageHistoryConfig>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroDamageHistoryConfig>()));
        }
        if (entity.Has<AggroModifiers>())
        {
            componentData.AppendLine("  AggroModifiers");
            componentData.AppendLine(RetrieveFields(entity.Read<AggroModifiers>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AggroModifiers>()));
        }
        if (entity.Has<Aggroable>())
        {
            componentData.AppendLine("  Aggroable");
            componentData.AppendLine(RetrieveFields(entity.Read<Aggroable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Aggroable>()));
        }
        if (entity.Has<AiDebugDraw>())
        {
            componentData.AppendLine("  AiDebugDraw");
            componentData.AppendLine(RetrieveFields(entity.Read<AiDebugDraw>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiDebugDraw>()));
        }
        if (entity.Has<AiMoveSpeeds>())
        {
            componentData.AppendLine("  AiMoveSpeeds");
            componentData.AppendLine(RetrieveFields(entity.Read<AiMoveSpeeds>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiMoveSpeeds>()));
        }
        if (entity.Has<AiMove_Server>())
        {
            componentData.AppendLine("  AiMove_Server");
            componentData.AppendLine(RetrieveFields(entity.Read<AiMove_Server>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiMove_Server>()));
        }
        if (entity.Has<AiMove_Shared>())
        {
            componentData.AppendLine("  AiMove_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<AiMove_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiMove_Shared>()));
        }
        if (entity.Has<AiPointOfInterest>())
        {
            componentData.AppendLine("  AiPointOfInterest");
            componentData.AppendLine(RetrieveFields(entity.Read<AiPointOfInterest>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiPointOfInterest>()));
        }
        if (entity.Has<AiPointOfInterestTarget>())
        {
            componentData.AppendLine("  AiPointOfInterestTarget");
            componentData.AppendLine(RetrieveFields(entity.Read<AiPointOfInterestTarget>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiPointOfInterestTarget>()));
        }
        if (entity.Has<AiPointOfInterest_BossCenterPosition>())
        {
            componentData.AppendLine("  AiPointOfInterest_BossCenterPosition");
            componentData.AppendLine(RetrieveFields(entity.Read<AiPointOfInterest_BossCenterPosition>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiPointOfInterest_BossCenterPosition>()));
        }
        if (entity.Has<AiPrioritization_Data>())
        {
            componentData.AppendLine("  AiPrioritization_Data");
            componentData.AppendLine(RetrieveFields(entity.Read<AiPrioritization_Data>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiPrioritization_Data>()));
        }
        if (entity.Has<AiPrioritization_State>())
        {
            componentData.AppendLine("  AiPrioritization_State");
            componentData.AppendLine(RetrieveFields(entity.Read<AiPrioritization_State>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AiPrioritization_State>()));
        }
        if (entity.Has<AimRotationParameters>())
        {
            componentData.AppendLine("  AimRotationParameters");
            componentData.AppendLine(RetrieveFields(entity.Read<AimRotationParameters>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AimRotationParameters>()));
        }
        if (entity.Has<AlertBuffer>())
        {
            componentData.AppendLine("  AlertBuffer");
            var buffer = ReadBuffer<AlertBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AlertBuffer>()));
        }
        if (entity.Has<AlertModifiers>())
        {
            componentData.AppendLine("  AlertModifiers");
            componentData.AppendLine(RetrieveFields(entity.Read<AlertModifiers>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AlertModifiers>()));
        }
        if (entity.Has<AllowJumpFromCliffsBuff>())
        {
            componentData.AppendLine("  AllowJumpFromCliffsBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<AllowJumpFromCliffsBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AllowJumpFromCliffsBuff>()));
        }
        if (entity.Has<AllyPermission>())
        {
            componentData.AppendLine("  AllyPermission");
            var buffer = ReadBuffer<AllyPermission>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AllyPermission>()));
        }
        if (entity.Has<AlwaysNetworked>())
        {
            componentData.AppendLine("  AlwaysNetworked");
            componentData.AppendLine(RetrieveFields(entity.Read<AlwaysNetworked>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AlwaysNetworked>()));
        }
        if (entity.Has<AmplifyBuff>())
        {
            componentData.AppendLine("  AmplifyBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<AmplifyBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AmplifyBuff>()));
        }
        if (entity.Has<AmplifyStackModifier>())
        {
            componentData.AppendLine("  AmplifyStackModifier");
            componentData.AppendLine(RetrieveFields(entity.Read<AmplifyStackModifier>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AmplifyStackModifier>()));
        }
        if (entity.Has<AnnounceCastleBreached>())
        {
            componentData.AppendLine("  AnnounceCastleBreached");
            componentData.AppendLine(RetrieveFields(entity.Read<AnnounceCastleBreached>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AnnounceCastleBreached>()));
        }
        if (entity.Has<AnnounceSiegeWeapon>())
        {
            componentData.AppendLine("  AnnounceSiegeWeapon");
            componentData.AppendLine(RetrieveFields(entity.Read<AnnounceSiegeWeapon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AnnounceSiegeWeapon>()));
        }
        if (entity.Has<AoETargetImportance>())
        {
            componentData.AppendLine("  AoETargetImportance");
            componentData.AppendLine(RetrieveFields(entity.Read<AoETargetImportance>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AoETargetImportance>()));
        }
        if (entity.Has<ApplyBuffOnGameplayEvent>())
        {
            componentData.AppendLine("  ApplyBuffOnGameplayEvent");
            var buffer = ReadBuffer<ApplyBuffOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ApplyBuffOnGameplayEvent>()));
        }
        if (entity.Has<ApplyBuffOnSpawn>())
        {
            componentData.AppendLine("  ApplyBuffOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<ApplyBuffOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ApplyBuffOnSpawn>()));
        }
        if (entity.Has<ApplyKnockbackOnGameplayEvent>())
        {
            componentData.AppendLine("  ApplyKnockbackOnGameplayEvent");
            var buffer = ReadBuffer<ApplyKnockbackOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ApplyKnockbackOnGameplayEvent>()));
        }
        if (entity.Has<ArmorLevel>())
        {
            componentData.AppendLine("  ArmorLevel");
            componentData.AppendLine(RetrieveFields(entity.Read<ArmorLevel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ArmorLevel>()));
        }
        if (entity.Has<ArmorLevelSource>())
        {
            componentData.AppendLine("  ArmorLevelSource");
            componentData.AppendLine(RetrieveFields(entity.Read<ArmorLevelSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ArmorLevelSource>()));
        }
        if (entity.Has<AssetSwapColliderBuffer>())
        {
            componentData.AppendLine("  AssetSwapColliderBuffer");
            var buffer = ReadBuffer<AssetSwapColliderBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AssetSwapColliderBuffer>()));
        }
        if (entity.Has<AssetSwapState>())
        {
            componentData.AppendLine("  AssetSwapState");
            componentData.AppendLine(RetrieveFields(entity.Read<AssetSwapState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AssetSwapState>()));
        }
        if (entity.Has<Attach>())
        {
            componentData.AppendLine("  Attach");
            componentData.AppendLine(RetrieveFields(entity.Read<Attach>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Attach>()));
		}
		if (entity.Has<AttachParentId>())
        {
            componentData.AppendLine("  AttachParentId");
            componentData.AppendLine(RetrieveFields(entity.Read<AttachParentId>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AttachParentId>()));
        }
        if (entity.Has<AttachMapIconsToEntity>())
        {
            componentData.AppendLine("  AttachMapIconsToEntity");
            var buffer = ReadBuffer<AttachMapIconsToEntity>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AttachMapIconsToEntity>()));
        }
        if (entity.Has<AttachParentId>())
        {
            componentData.AppendLine("  AttachParentId");
            componentData.AppendLine(RetrieveFields(entity.Read<AttachParentId>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AttachParentId>()));
		}
		if (entity.Has<Attached>())
		{
			componentData.AppendLine("  Attached");
			componentData.AppendLine(RetrieveFields(entity.Read<Attached>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<Attached>()));
		}
		if (entity.Has<AttachedBuffer>())
        {
            componentData.AppendLine("  AttachedBuffer");
            componentData.AppendLine(RetrieveFields(entity.Read<AttachedBuffer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AttachedBuffer>()));
        }
        if (entity.Has<AutoChainInstanceData>())
        {
            componentData.AppendLine("  AutoChainInstanceData");
            componentData.AppendLine(RetrieveFields(entity.Read<AutoChainInstanceData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<AutoChainInstanceData>()));
		}
		if (entity.Has<AttachedDepth>())
		{
			componentData.AppendLine("  AttachedDepth");
			componentData.AppendLine(RetrieveFields(entity.Read<AttachedDepth>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<AttachedDepth>()));
		}
		if (entity.Has<BagHolder>())
        {
            componentData.AppendLine("  BagHolder");
            componentData.AppendLine(RetrieveFields(entity.Read<BagHolder>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BagHolder>()));
        }
        if (entity.Has<BehaviourTreeBinding>())
        {
            componentData.AppendLine("  BehaviourTreeBinding");
            componentData.AppendLine(RetrieveFields(entity.Read<BehaviourTreeBinding>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeBinding>()));
        }
        if (entity.Has<BehaviourTreeInstance>())
        {
            componentData.AppendLine("  BehaviourTreeInstance");
            componentData.AppendLine(RetrieveFields(entity.Read<BehaviourTreeInstance>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeInstance>()));
        }
        if (entity.Has<BehaviourTreeNodeInstanceElement>())
        {
            componentData.AppendLine("  BehaviourTreeNodeInstanceElement");
            var buffer = ReadBuffer<BehaviourTreeNodeInstanceElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeNodeInstanceElement>()));
        }
        if (entity.Has<BehaviourTreeState>())
        {
            componentData.AppendLine("  BehaviourTreeState");
            componentData.AppendLine(RetrieveFields(entity.Read<BehaviourTreeState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeState>()));
        }
        if (entity.Has<BehaviourTreeStateActiveBuffsBuffer>())
        {
            componentData.AppendLine("  BehaviourTreeStateActiveBuffsBuffer");
            var buffer = ReadBuffer<BehaviourTreeStateActiveBuffsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeStateActiveBuffsBuffer>()));
        }
        if (entity.Has<BehaviourTreeStateBuffsBuffer>())
        {
            componentData.AppendLine("  BehaviourTreeStateBuffsBuffer");
            var buffer = ReadBuffer<BehaviourTreeStateBuffsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeStateBuffsBuffer>()));
        }
        if (entity.Has<BehaviourTreeStateMetadata>())
        {
            componentData.AppendLine("  BehaviourTreeStateMetadata");
            componentData.AppendLine(RetrieveFields(entity.Read<BehaviourTreeStateMetadata>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BehaviourTreeStateMetadata>()));
        }
        if (entity.Has<BlackboardElement>())
        {
            componentData.AppendLine("  BlackboardElement");
            var buffer = ReadBuffer<BlackboardElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BlackboardElement>()));
        }
        if (entity.Has<BlobAssetOwner>())
        {
            componentData.AppendLine("  BlobAssetOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<BlobAssetOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BlobAssetOwner>()));
        }
        if (entity.Has<BlockFeedBuff>())
        {
            componentData.AppendLine("  BlockFeedBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<BlockFeedBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BlockFeedBuff>()));
        }
        if (entity.Has<Blood>())
        {
            componentData.AppendLine("  Blood");
            componentData.AppendLine(RetrieveFields(entity.Read<Blood>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Blood>()));
        }
        if (entity.Has<BloodAltar>())
        {
            componentData.AppendLine("  BloodAltar");
            componentData.AppendLine(RetrieveFields(entity.Read<BloodAltar>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodAltar>()));
        }
        if (entity.Has<BloodConsumeSource>())
        {
            componentData.AppendLine("  BloodConsumeSource");
            componentData.AppendLine(RetrieveFields(entity.Read<BloodConsumeSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodConsumeSource>()));
        }
        if (entity.Has<BloodHuntBuffer>())
        {
            componentData.AppendLine("  BloodHuntBuffer");
            var buffer = ReadBuffer<BloodHuntBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodHuntBuffer>()));
        }
        if (entity.Has<BloodHuntsData>())
        {
            componentData.AppendLine("  BloodHuntsData");
            componentData.AppendLine(RetrieveFields(entity.Read<BloodHuntsData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodHuntsData>()));
        }
        if (entity.Has<BloodMoonBuff>())
        {
            componentData.AppendLine("  BloodMoonBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<BloodMoonBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodMoonBuff>()));
        }
        if (entity.Has<BloodMoonBuffState>())
        {
            componentData.AppendLine("  BloodMoonBuffState");
            componentData.AppendLine(RetrieveFields(entity.Read<BloodMoonBuffState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodMoonBuffState>()));
        }
        if (entity.Has<BloodQualityBuff>())
        {
            componentData.AppendLine("  BloodQualityBuff");
            var buffer = ReadBuffer<BloodQualityBuff>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodQualityBuff>()));
        }
        if (entity.Has<BloodQualityCurveSetting>())
        {
            componentData.AppendLine("  BloodQualityCurveSetting");
            var buffer = ReadBuffer<BloodQualityCurveSetting>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodQualityCurveSetting>()));
        }
        if (entity.Has<BloodQualityUnitBuff>())
        {
            componentData.AppendLine("  BloodQualityUnitBuff");
            var buffer = ReadBuffer<BloodQualityUnitBuff>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BloodQualityUnitBuff>()));
        }
        if (entity.Has<BlueprintData>())
        {
            componentData.AppendLine("  BlueprintData");
            componentData.AppendLine(RetrieveFields(entity.Read<BlueprintData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BlueprintData>()));
        }
        if (entity.Has<BlueprintRequirementBuffer>())
        {
            componentData.AppendLine("  BlueprintRequirementBuffer");
            var buffer = ReadBuffer<BlueprintRequirementBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BlueprintRequirementBuffer>()));
        }
        if (entity.Has<Bonfire>())
        {
            componentData.AppendLine("  Bonfire");
            componentData.AppendLine(RetrieveFields(entity.Read<Bonfire>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Bonfire>()));
        }
        if (entity.Has<BoolModificationBuffer>())
        {
            componentData.AppendLine("  BoolModificationBuffer");
            var buffer = ReadBuffer<BoolModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BoolModificationBuffer>()));
        }
        if (entity.Has<BranchThroughGameplayEvent>())
        {
            componentData.AppendLine("  BranchThroughGameplayEvent");
            var buffer = ReadBuffer<BranchThroughGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BranchThroughGameplayEvent>()));
        }
        if (entity.Has<Buff>())
        {
            componentData.AppendLine("  Buff");
            componentData.AppendLine(RetrieveFields(entity.Read<Buff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buff>()));
        }
        if (entity.Has<BuffBuffer>())
        {
            componentData.AppendLine("  BuffBuffer");
            var buffer = ReadBuffer<BuffBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffBuffer>()));
        }
        if (entity.Has<BuffByItemCategoryCount>())
        {
            componentData.AppendLine("  BuffByItemCategoryCount");
            var buffer = ReadBuffer<BuffByItemCategoryCount>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffByItemCategoryCount>()));
        }
        if (entity.Has<BuffCategory>())
        {
            componentData.AppendLine("  BuffCategory");
            componentData.AppendLine(RetrieveFields(entity.Read<BuffCategory>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffCategory>()));
        }
        if (entity.Has<BuffModificationFlagData>())
        {
            componentData.AppendLine("  BuffModificationFlagData");
            componentData.AppendLine(RetrieveFields(entity.Read<BuffModificationFlagData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffModificationFlagData>()));
        }
        if (entity.Has<BuffOnConnectionStatusElement>())
        {
            componentData.AppendLine("  BuffOnConnectionStatusElement");
            var buffer = ReadBuffer<BuffOnConnectionStatusElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffOnConnectionStatusElement>()));
        }
        if (entity.Has<BuffResistances>())
        {
            componentData.AppendLine("  BuffResistances");
            componentData.AppendLine(RetrieveFields(entity.Read<BuffResistances>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffResistances>()));
        }
        if (entity.Has<Buff_Destroy_On_Owner_Death>())
        {
            componentData.AppendLine("  Buff_Destroy_On_Owner_Death");
            componentData.AppendLine(RetrieveFields(entity.Read<Buff_Destroy_On_Owner_Death>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buff_Destroy_On_Owner_Death>()));
        }
        if (entity.Has<Buff_HealAttackerOnDamageType_DataShared>())
        {
            componentData.AppendLine("  Buff_HealAttackerOnDamageType_DataShared");
            componentData.AppendLine(RetrieveFields(entity.Read<Buff_HealAttackerOnDamageType_DataShared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buff_HealAttackerOnDamageType_DataShared>()));
        }
        if (entity.Has<Buff_Persists_Through_Death>())
        {
            componentData.AppendLine("  Buff_Persists_Through_Death");
            componentData.AppendLine(RetrieveFields(entity.Read<Buff_Persists_Through_Death>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buff_Persists_Through_Death>()));
        }
        if (entity.Has<Buff_Reduce_SpellCooldown_DataShared>())
        {
            componentData.AppendLine("  Buff_Reduce_SpellCooldown_DataShared");
            componentData.AppendLine(RetrieveFields(entity.Read<Buff_Reduce_SpellCooldown_DataShared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buff_Reduce_SpellCooldown_DataShared>()));
        }
        if (entity.Has<Buffable>())
        {
            componentData.AppendLine("  Buffable");
            componentData.AppendLine(RetrieveFields(entity.Read<Buffable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Buffable>()));
        }
        if (entity.Has<BuffableFlagState>())
        {
            componentData.AppendLine("  BuffableFlagState");
            componentData.AppendLine(RetrieveFields(entity.Read<BuffableFlagState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BuffableFlagState>()));
        }
        if (entity.Has<BurnContainer>())
        {
            componentData.AppendLine("  BurnContainer");
            componentData.AppendLine(RetrieveFields(entity.Read<BurnContainer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<BurnContainer>()));
        }
        if (entity.Has<CanBuildTileModels>())
        {
            componentData.AppendLine("  CanBuildTileModels");
            componentData.AppendLine(RetrieveFields(entity.Read<CanBuildTileModels>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CanBuildTileModels>()));
        }
        if (entity.Has<CanFly>())
        {
            componentData.AppendLine("  CanFly");
            componentData.AppendLine(RetrieveFields(entity.Read<CanFly>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CanFly>()));
        }
        if (entity.Has<CanPreventDisableWhenNoPlayersInRange>())
        {
            componentData.AppendLine("  CanPreventDisableWhenNoPlayersInRange");
            componentData.AppendLine(RetrieveFields(entity.Read<CanPreventDisableWhenNoPlayersInRange>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CanPreventDisableWhenNoPlayersInRange>()));
        }
        if (entity.Has<CastAbilityInStateScript_DataServer>())
        {
            componentData.AppendLine("  CastAbilityInStateScript_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<CastAbilityInStateScript_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastAbilityInStateScript_DataServer>()));
        }
        if (entity.Has<CastAbilityOnConsume>())
        {
            componentData.AppendLine("  CastAbilityOnConsume");
            componentData.AppendLine(RetrieveFields(entity.Read<CastAbilityOnConsume>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastAbilityOnConsume>()));
        }
        if (entity.Has<CastHistoryBufferElement>())
        {
            componentData.AppendLine("  CastHistoryBufferElement");
            var buffer = ReadBuffer<CastHistoryBufferElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastHistoryBufferElement>()));
        }
        if (entity.Has<CastHistoryData>())
        {
            componentData.AppendLine("  CastHistoryData");
            componentData.AppendLine(RetrieveFields(entity.Read<CastHistoryData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastHistoryData>()));
        }
        if (entity.Has<CastOptionRoot>())
        {
            componentData.AppendLine("  CastOptionRoot");
            componentData.AppendLine(RetrieveFields(entity.Read<CastOptionRoot>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastOptionRoot>()));
        }
        if (entity.Has<CastOptionStateBuffer>())
        {
            componentData.AppendLine("  CastOptionStateBuffer");
            var buffer = ReadBuffer<CastOptionStateBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastOptionStateBuffer>()));
        }
        if (entity.Has<CastleAreaRequirement>())
        {
            componentData.AppendLine("  CastleAreaRequirement");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleAreaRequirement>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleAreaRequirement>()));
        }
        if (entity.Has<CastleBuffsSettings>())
        {
            componentData.AppendLine("  CastleBuffsSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleBuffsSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuffsSettings>()));
        }
        if (entity.Has<CastleBuildingAttachSettings>())
        {
            componentData.AppendLine("  CastleBuildingAttachSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleBuildingAttachSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingAttachSettings>()));
        }
        if (entity.Has<CastleBuildingAttachToParentsBuffer>())
        {
            componentData.AppendLine("  CastleBuildingAttachToParentsBuffer");
            var buffer = ReadBuffer<CastleBuildingAttachToParentsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingAttachToParentsBuffer>()));
        }
        if (entity.Has<CastleBuildingAttachedChildrenBuffer>())
        {
            componentData.AppendLine("  CastleBuildingAttachedChildrenBuffer");
            var buffer = ReadBuffer<CastleBuildingAttachedChildrenBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingAttachedChildrenBuffer>()));
        }
        if (entity.Has<CastleBuildingAttachmentActiveBuffsBuffer>())
        {
            componentData.AppendLine("  CastleBuildingAttachmentActiveBuffsBuffer");
            var buffer = ReadBuffer<CastleBuildingAttachmentActiveBuffsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingAttachmentActiveBuffsBuffer>()));
        }
        if (entity.Has<CastleBuildingAttachmentApplyBuff>())
        {
            componentData.AppendLine("  CastleBuildingAttachmentApplyBuff");
            var buffer = ReadBuffer<CastleBuildingAttachmentApplyBuff>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingAttachmentApplyBuff>()));
        }
        if (entity.Has<CastleBuildingFusedChild>())
        {
            componentData.AppendLine("  CastleBuildingFusedChild");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleBuildingFusedChild>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingFusedChild>()));
        }
        if (entity.Has<CastleBuildingFusedChildrenBuffer>())
        {
            componentData.AppendLine("  CastleBuildingFusedChildrenBuffer");
            var buffer = ReadBuffer<CastleBuildingFusedChildrenBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingFusedChildrenBuffer>()));
        }
        if (entity.Has<CastleBuildingFusedRoot>())
        {
            componentData.AppendLine("  CastleBuildingFusedRoot");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleBuildingFusedRoot>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingFusedRoot>()));
        }
        if (entity.Has<CastleBuildingMaxRange>())
        {
            componentData.AppendLine("  CastleBuildingMaxRange");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleBuildingMaxRange>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleBuildingMaxRange>()));
        }
        if (entity.Has<CastleDecayAndRegen>())
        {
            componentData.AppendLine("  CastleDecayAndRegen");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleDecayAndRegen>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleDecayAndRegen>()));
        }
        if (entity.Has<CastleFloor>())
        {
            componentData.AppendLine("  CastleFloor");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleFloor>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleFloor>()));
        }
        if (entity.Has<CastleFloorRoof>())
        {
            componentData.AppendLine("  CastleFloorRoof");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleFloorRoof>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleFloorRoof>()));
        }
        if (entity.Has<CastleHeart>())
        {
            componentData.AppendLine("  CastleHeart");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleHeart>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleHeart>()));
        }
        if (entity.Has<CastleHeartConnection>())
        {
            componentData.AppendLine("  CastleHeartConnection");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleHeartConnection>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleHeartConnection>()));
        }
        if (entity.Has<CastleLimited>())
        {
            componentData.AppendLine("  CastleLimited");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleLimited>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleLimited>()));
        }
        if (entity.Has<CastleMemberNames>())
        {
            componentData.AppendLine("  CastleMemberNames");
            var buffer = ReadBuffer<CastleMemberNames>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleMemberNames>()));
        }
        if (entity.Has<CastleRailing>())
        {
            componentData.AppendLine("  CastleRailing");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleRailing>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRailing>()));
        }
        if (entity.Has<CastleResistanceBuff>())
        {
            componentData.AppendLine("  CastleResistanceBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleResistanceBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleResistanceBuff>()));
        }
        if (entity.Has<CastleRoofOrnaments>())
        {
            componentData.AppendLine("  CastleRoofOrnaments");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleRoofOrnaments>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoofOrnaments>()));
        }
        if (entity.Has<CastleRoom>())
        {
            componentData.AppendLine("  CastleRoom");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleRoom>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoom>()));
        }
        if (entity.Has<CastleRoomConnection>())
        {
            componentData.AppendLine("  CastleRoomConnection");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleRoomConnection>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoomConnection>()));
        }
        if (entity.Has<CastleRoomFloorsBuffer>())
        {
            componentData.AppendLine("  CastleRoomFloorsBuffer");
            var buffer = ReadBuffer<CastleRoomFloorsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoomFloorsBuffer>()));
        }
        if (entity.Has<CastleRoomWall>())
        {
            componentData.AppendLine("  CastleRoomWall");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleRoomWall>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoomWall>()));
        }
        if (entity.Has<CastleRoomWallsBuffer>())
        {
            componentData.AppendLine("  CastleRoomWallsBuffer");
            var buffer = ReadBuffer<CastleRoomWallsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoomWallsBuffer>()));
        }
        if (entity.Has<CastleRoomWorkstationsBuffer>())
        {
            componentData.AppendLine("  CastleRoomWorkstationsBuffer");
            var buffer = ReadBuffer<CastleRoomWorkstationsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleRoomWorkstationsBuffer>()));
        }
        if (entity.Has<CastleStairs>())
        {
            componentData.AppendLine("  CastleStairs");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleStairs>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleStairs>()));
		}
		if (entity.Has<CastleTeam>())
		{
			componentData.AppendLine("  CastleTeam");
			componentData.AppendLine(RetrieveFields(entity.Read<CastleTeam>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTeam>()));
		}
		if (entity.Has<CastleTeamData>())
		{
			componentData.AppendLine("  CastleTeamData");
			componentData.AppendLine(RetrieveFields(entity.Read<CastleTeamData>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTeamData>()));
		}
		if (entity.Has<CastleTeleporterComponent>())
        {
            componentData.AppendLine("  CastleTeleporterComponent");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleTeleporterComponent>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTeleporterComponent>()));
        }
        if (entity.Has<CastleTeleporterElement>())
        {
            componentData.AppendLine("  CastleTeleporterElement");
            var buffer = ReadBuffer<CastleTeleporterElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTeleporterElement>()));
        }
        if (entity.Has<CastleTerritoryBlocks>())
        {
            componentData.AppendLine("  CastleTerritoryBlocks");
            var buffer = ReadBuffer<CastleTerritoryBlocks>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTerritoryBlocks>()));
        }
        if (entity.Has<CastleTerritoryDecay>())
        {
            componentData.AppendLine("  CastleTerritoryDecay");
            var buffer = ReadBuffer<CastleTerritoryDecay>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTerritoryDecay>()));
        }
        if (entity.Has<CastleTerritoryManager>())
        {
            componentData.AppendLine("  CastleTerritoryManager");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleTerritoryManager>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTerritoryManager>()));
        }
        if (entity.Has<CastleTerritoryOccupant>())
        {
            componentData.AppendLine("  CastleTerritoryOccupant");
            var buffer = ReadBuffer<CastleTerritoryOccupant>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTerritoryOccupant>()));
        }
        if (entity.Has<CastleTerritoryTiles>())
        {
            componentData.AppendLine("  CastleTerritoryTiles");
            var buffer = ReadBuffer<CastleTerritoryTiles>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleTerritoryTiles>()));
        }
        if (entity.Has<CastleWall>())
        {
            componentData.AppendLine("  CastleWall");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleWall>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleWall>()));
        }
        if (entity.Has<CastleWallPillar>())
        {
            componentData.AppendLine("  CastleWallPillar");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleWallPillar>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleWallPillar>()));
        }
        if (entity.Has<CastleWaypoint>())
        {
            componentData.AppendLine("  CastleWaypoint");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleWaypoint>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleWaypoint>()));
        }
        if (entity.Has<CastleWorkstation>())
        {
            componentData.AppendLine("  CastleWorkstation");
            componentData.AppendLine(RetrieveFields(entity.Read<CastleWorkstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CastleWorkstation>()));
        }
        if (entity.Has<ChangeKnockbackResistanceBuff>())
        {
            componentData.AppendLine("  ChangeKnockbackResistanceBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<ChangeKnockbackResistanceBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ChangeKnockbackResistanceBuff>()));
        }
        if (entity.Has<CharacterVoiceActivity>())
        {
            componentData.AppendLine("  CharacterVoiceActivity");
            componentData.AppendLine(RetrieveFields(entity.Read<CharacterVoiceActivity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CharacterVoiceActivity>()));
        }
        if (entity.Has<CharmSource>())
        {
            componentData.AppendLine("  CharmSource");
            componentData.AppendLine(RetrieveFields(entity.Read<CharmSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CharmSource>()));
        }
        if (entity.Has<ChunkPortal>())
        {
            componentData.AppendLine("  ChunkPortal");
            componentData.AppendLine(RetrieveFields(entity.Read<ChunkPortal>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ChunkPortal>()));
        }
        if (entity.Has<ChunkWaypoint>())
        {
            componentData.AppendLine("  ChunkWaypoint");
            componentData.AppendLine(RetrieveFields(entity.Read<ChunkWaypoint>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ChunkWaypoint>()));
        }
        if (entity.Has<ClanInviteRequest_Shared>())
        {
            componentData.AppendLine("  ClanInviteRequest_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<ClanInviteRequest_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ClanInviteRequest_Shared>()));
        }
        if (entity.Has<ClanMemberStatus>())
        {
            componentData.AppendLine("  ClanMemberStatus");
            var buffer = ReadBuffer<ClanMemberStatus>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ClanMemberStatus>()));
        }
        if (entity.Has<ClanRole>())
        {
            componentData.AppendLine("  ClanRole");
            componentData.AppendLine(RetrieveFields(entity.Read<ClanRole>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ClanRole>()));
        }
        if (entity.Has<ClanTeam>())
        {
            componentData.AppendLine("  ClanTeam");
            componentData.AppendLine(RetrieveFields(entity.Read<ClanTeam>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ClanTeam>()));
        }
        if (entity.Has<CloudCookie>())
        {
            componentData.AppendLine("  CloudCookie");
            componentData.AppendLine(RetrieveFields(entity.Read<CloudCookie>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CloudCookie>()));
        }
        if (entity.Has<CollisionCastOnDestroy>())
        {
            componentData.AppendLine("  CollisionCastOnDestroy");
            componentData.AppendLine(RetrieveFields(entity.Read<CollisionCastOnDestroy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CollisionCastOnDestroy>()));
        }
        if (entity.Has<CollisionCastOnSpawn>())
        {
            componentData.AppendLine("  CollisionCastOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<CollisionCastOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CollisionCastOnSpawn>()));
        }
        if (entity.Has<CollisionCastOnUpdate>())
        {
            componentData.AppendLine("  CollisionCastOnUpdate");
            componentData.AppendLine(RetrieveFields(entity.Read<CollisionCastOnUpdate>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CollisionCastOnUpdate>()));
        }
        if (entity.Has<CollisionRadius>())
        {
            componentData.AppendLine("  CollisionRadius");
            componentData.AppendLine(RetrieveFields(entity.Read<CollisionRadius>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CollisionRadius>()));
        }
        if (entity.Has<CombatMusicListener_Shared>())
        {
            componentData.AppendLine("  CombatMusicListener_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<CombatMusicListener_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CombatMusicListener_Shared>()));
        }
        if (entity.Has<CombatMusicListener_SourceElement>())
        {
            componentData.AppendLine("  CombatMusicListener_SourceElement");
            var buffer = ReadBuffer<CombatMusicListener_SourceElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CombatMusicListener_SourceElement>()));
        }
        if (entity.Has<CombatMusicSource_Server>())
        {
            componentData.AppendLine("  CombatMusicSource_Server");
            componentData.AppendLine(RetrieveFields(entity.Read<CombatMusicSource_Server>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CombatMusicSource_Server>()));
        }
        if (entity.Has<CompositeScale>())
        {
            componentData.AppendLine("  CompositeScale");
            componentData.AppendLine(RetrieveFields(entity.Read<CompositeScale>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CompositeScale>()));
		}
		if (entity.Has<ConnectedUser>())
		{
			componentData.AppendLine("  ConnectedUser");
			componentData.AppendLine(RetrieveFields(entity.Read<ConnectedUser>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<ConnectedUser>()));
		}
		if (entity.Has<ConsumableCondition>())
        {
            componentData.AppendLine("  ConsumableCondition");
            var buffer = ReadBuffer<ConsumableCondition>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ConsumableCondition>()));
		}
		if (entity.Has<ControlledBy>())
		{
			componentData.AppendLine("  ControlledBy");
			componentData.AppendLine(RetrieveFields(entity.Read<ControlledBy>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<ControlledBy>()));
		}
		if (entity.Has<Controller>())
        {
            componentData.AppendLine("  Controller");
            componentData.AppendLine(RetrieveFields(entity.Read<Controller>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Controller>()));
        }
        if (entity.Has<CreateGameplayEventOnDamageTaken>())
        {
            componentData.AppendLine("  CreateGameplayEventOnDamageTaken");
            var buffer = ReadBuffer<CreateGameplayEventOnDamageTaken>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventOnDamageTaken>()));
        }
        if (entity.Has<CreateGameplayEventOnDeath>())
        {
            componentData.AppendLine("  CreateGameplayEventOnDeath");
            var buffer = ReadBuffer<CreateGameplayEventOnDeath>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventOnDeath>()));
        }
        if (entity.Has<CreateGameplayEventOnKill>())
        {
            componentData.AppendLine("  CreateGameplayEventOnKill");
            var buffer = ReadBuffer<CreateGameplayEventOnKill>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventOnKill>()));
        }
        if (entity.Has<CreateGameplayEventsOnAbilityTrigger>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnAbilityTrigger");
            var buffer = ReadBuffer<CreateGameplayEventsOnAbilityTrigger>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnAbilityTrigger>()));
        }
        if (entity.Has<CreateGameplayEventsOnAbilityTriggerAbilityPrefabTargets>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnAbilityTriggerAbilityPrefabTargets");
            var buffer = ReadBuffer<CreateGameplayEventsOnAbilityTriggerAbilityPrefabTargets>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnAbilityTriggerAbilityPrefabTargets>()));
        }
        if (entity.Has<CreateGameplayEventsOnDestroy>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnDestroy");
            var buffer = ReadBuffer<CreateGameplayEventsOnDestroy>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnDestroy>()));
        }
        if (entity.Has<CreateGameplayEventsOnHit>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnHit");
            var buffer = ReadBuffer<CreateGameplayEventsOnHit>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnHit>()));
        }
        if (entity.Has<CreateGameplayEventsOnSpawn>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnSpawn");
            var buffer = ReadBuffer<CreateGameplayEventsOnSpawn>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnSpawn>()));
        }
        if (entity.Has<CreateGameplayEventsOnTick>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnTick");
            var buffer = ReadBuffer<CreateGameplayEventsOnTick>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnTick>()));
        }
        if (entity.Has<CreateGameplayEventsOnTimePassed>())
        {
            componentData.AppendLine("  CreateGameplayEventsOnTimePassed");
            var buffer = ReadBuffer<CreateGameplayEventsOnTimePassed>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreateGameplayEventsOnTimePassed>()));
        }
        if (entity.Has<CreatedTime>())
        {
            componentData.AppendLine("  CreatedTime");
            componentData.AppendLine(RetrieveFields(entity.Read<CreatedTime>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CreatedTime>()));
        }
        if (entity.Has<Crowdedness>())
        {
            componentData.AppendLine("  Crowdedness");
            componentData.AppendLine(RetrieveFields(entity.Read<Crowdedness>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Crowdedness>()));
        }
        if (entity.Has<CrowdednessPlayerBufferElement>())
        {
            componentData.AppendLine("  CrowdednessPlayerBufferElement");
            var buffer = ReadBuffer<CrowdednessPlayerBufferElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CrowdednessPlayerBufferElement>()));
        }
        if (entity.Has<CrowdednessSetting>())
        {
            componentData.AppendLine("  CrowdednessSetting");
            var buffer = ReadBuffer<CrowdednessSetting>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CrowdednessSetting>()));
        }
        if (entity.Has<CurrentMapZone>())
        {
            componentData.AppendLine("  CurrentMapZone");
            componentData.AppendLine(RetrieveFields(entity.Read<CurrentMapZone>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CurrentMapZone>()));
        }
        if (entity.Has<CurrentTileModelEditing>())
        {
            componentData.AppendLine("  CurrentTileModelEditing");
            componentData.AppendLine(RetrieveFields(entity.Read<CurrentTileModelEditing>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CurrentTileModelEditing>()));
        }
        if (entity.Has<CurrentWorldRegion>())
        {
            componentData.AppendLine("  CurrentWorldRegion");
            componentData.AppendLine(RetrieveFields(entity.Read<CurrentWorldRegion>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CurrentWorldRegion>()));
        }
        if (entity.Has<CurseArea>())
        {
            componentData.AppendLine("  CurseArea");
            componentData.AppendLine(RetrieveFields(entity.Read<CurseArea>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CurseArea>()));
        }
        if (entity.Has<CustomizationFeatures>())
        {
            componentData.AppendLine("  CustomizationFeatures");
            componentData.AppendLine(RetrieveFields(entity.Read<CustomizationFeatures>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<CustomizationFeatures>()));
        }
        if (entity.Has<DamageCategoryStats>())
        {
            componentData.AppendLine("  DamageCategoryStats");
            componentData.AppendLine(RetrieveFields(entity.Read<DamageCategoryStats>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DamageCategoryStats>()));
        }
        if (entity.Has<Dash>())
        {
            componentData.AppendLine("  Dash");
            componentData.AppendLine(RetrieveFields(entity.Read<Dash>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Dash>()));
        }
        if (entity.Has<DashSpawn>())
        {
            componentData.AppendLine("  DashSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<DashSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DashSpawn>()));
        }
        if (entity.Has<DayNightCycle>())
        {
            componentData.AppendLine("  DayNightCycle");
            componentData.AppendLine(RetrieveFields(entity.Read<DayNightCycle>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DayNightCycle>()));
        }
        if (entity.Has<DealDamageOnGameplayEvent>())
        {
            componentData.AppendLine("  DealDamageOnGameplayEvent");
            var buffer = ReadBuffer<DealDamageOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DealDamageOnGameplayEvent>()));
        }
        if (entity.Has<DealDamageToPrisoner>())
        {
            componentData.AppendLine("  DealDamageToPrisoner");
            componentData.AppendLine(RetrieveFields(entity.Read<DealDamageToPrisoner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DealDamageToPrisoner>()));
        }
        if (entity.Has<DeathBuff>())
        {
            componentData.AppendLine("  DeathBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<DeathBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DeathBuff>()));
        }
        if (entity.Has<DeathContainerMapIcon>())
        {
            componentData.AppendLine("  DeathContainerMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<DeathContainerMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DeathContainerMapIcon>()));
        }
        if (entity.Has<DeathRagdollForce>())
        {
            componentData.AppendLine("  DeathRagdollForce");
            componentData.AppendLine(RetrieveFields(entity.Read<DeathRagdollForce>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DeathRagdollForce>()));
        }
        if (entity.Has<DefaultAction>())
        {
            componentData.AppendLine("  DefaultAction");
            var buffer = ReadBuffer<DefaultAction>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DefaultAction>()));
        }
        if (entity.Has<Description>())
        {
            componentData.AppendLine("  Description");
            componentData.AppendLine(RetrieveFields(entity.Read<Description>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Description>()));
        }
        if (entity.Has<DestroyAfterDuration>())
        {
            componentData.AppendLine("  DestroyAfterDuration");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyAfterDuration>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyAfterDuration>()));
        }
        if (entity.Has<DestroyAfterDuration_ActiveUserCheck>())
        {
            componentData.AppendLine("  DestroyAfterDuration_ActiveUserCheck");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyAfterDuration_ActiveUserCheck>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyAfterDuration_ActiveUserCheck>()));
        }
        if (entity.Has<DestroyAfterTimeOnInventoryChange>())
        {
            componentData.AppendLine("  DestroyAfterTimeOnInventoryChange");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyAfterTimeOnInventoryChange>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyAfterTimeOnInventoryChange>()));
        }
        if (entity.Has<DestroyBuffOnDamageTaken>())
        {
            componentData.AppendLine("  DestroyBuffOnDamageTaken");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyBuffOnDamageTaken>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyBuffOnDamageTaken>()));
        }
        if (entity.Has<DestroyData>())
        {
            componentData.AppendLine("  DestroyData");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyData>()));
        }
        if (entity.Has<DestroyOnGameplayEvent>())
        {
            componentData.AppendLine("  DestroyOnGameplayEvent");
            var buffer = ReadBuffer<DestroyOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyOnGameplayEvent>()));
        }
        if (entity.Has<DestroyOnManualInterrupt>())
        {
            componentData.AppendLine("  DestroyOnManualInterrupt");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyOnManualInterrupt>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyOnManualInterrupt>()));
        }
        if (entity.Has<DestroyOnSpawn>())
        {
            componentData.AppendLine("  DestroyOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyOnSpawn>()));
        }
        if (entity.Has<DestroyState>())
        {
            componentData.AppendLine("  DestroyState");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyState>()));
        }
        if (entity.Has<DestroyWhenInventoryIsEmpty>())
        {
            componentData.AppendLine("  DestroyWhenInventoryIsEmpty");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyWhenInventoryIsEmpty>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyWhenInventoryIsEmpty>()));
        }
        if (entity.Has<DestroyWhenNoCharacterNearbyAfterDuration>())
        {
            componentData.AppendLine("  DestroyWhenNoCharacterNearbyAfterDuration");
            componentData.AppendLine(RetrieveFields(entity.Read<DestroyWhenNoCharacterNearbyAfterDuration>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DestroyWhenNoCharacterNearbyAfterDuration>()));
        }
        if (entity.Has<DiminishingReturn>())
        {
            componentData.AppendLine("  DiminishingReturn");
            componentData.AppendLine(RetrieveFields(entity.Read<DiminishingReturn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DiminishingReturn>()));
        }
        if (entity.Has<DiminishingReturnBuff>())
        {
            componentData.AppendLine("  DiminishingReturnBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<DiminishingReturnBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DiminishingReturnBuff>()));
        }
        if (entity.Has<DiminishingReturnElement>())
        {
            componentData.AppendLine("  DiminishingReturnElement");
            var buffer = ReadBuffer<DiminishingReturnElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DiminishingReturnElement>()));
        }
        if (entity.Has<DirtyTag>())
        {
            componentData.AppendLine("  DirtyTag");
            componentData.AppendLine(RetrieveFields(entity.Read<DirtyTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DirtyTag>()));
        }
        if (entity.Has<DisableAggroBuff>())
        {
            componentData.AppendLine("  DisableAggroBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<DisableAggroBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DisableAggroBuff>()));
        }
        if (entity.Has<DisableWhenNoPlayersInRange>())
        {
            componentData.AppendLine("  DisableWhenNoPlayersInRange");
            componentData.AppendLine(RetrieveFields(entity.Read<DisableWhenNoPlayersInRange>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DisableWhenNoPlayersInRange>()));
        }
        if (entity.Has<DisconnectedTimer>())
        {
            componentData.AppendLine("  DisconnectedTimer");
            componentData.AppendLine(RetrieveFields(entity.Read<DisconnectedTimer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DisconnectedTimer>()));
        }
        if (entity.Has<DiscoverCostBuffer>())
        {
            componentData.AppendLine("  DiscoverCostBuffer");
            var buffer = ReadBuffer<DiscoverCostBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DiscoverCostBuffer>()));
        }
        if (entity.Has<DiscoveredMapZoneElement>())
        {
            componentData.AppendLine("  DiscoveredMapZoneElement");
            var buffer = ReadBuffer<DiscoveredMapZoneElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DiscoveredMapZoneElement>()));
        }
        if (entity.Has<DismantleDestroyData>())
        {
            componentData.AppendLine("  DismantleDestroyData");
            componentData.AppendLine(RetrieveFields(entity.Read<DismantleDestroyData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DismantleDestroyData>()));
        }
        if (entity.Has<Door>())
        {
            componentData.AppendLine("  Door");
            componentData.AppendLine(RetrieveFields(entity.Read<Door>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Door>()));
        }
        if (entity.Has<DropInInventoryOnSpawn>())
        {
            componentData.AppendLine("  DropInInventoryOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<DropInInventoryOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropInInventoryOnSpawn>()));
        }
        if (entity.Has<DropTable>())
        {
            componentData.AppendLine("  DropTable");
            componentData.AppendLine(RetrieveFields(entity.Read<DropTable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropTable>()));
        }
        if (entity.Has<DropTableBuffer>())
        {
            componentData.AppendLine("  DropTableBuffer");
            var buffer = ReadBuffer<DropTableBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropTableBuffer>()));
        }
        if (entity.Has<DropTableOnDeath>())
        {
            componentData.AppendLine("  DropTableOnDeath");
            componentData.AppendLine(RetrieveFields(entity.Read<DropTableOnDeath>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropTableOnDeath>()));
        }
        if (entity.Has<DropTableOnDestroy>())
        {
            componentData.AppendLine("  DropTableOnDestroy");
            componentData.AppendLine(RetrieveFields(entity.Read<DropTableOnDestroy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropTableOnDestroy>()));
        }
        if (entity.Has<DropTableOnSalvageDestroy>())
        {
            componentData.AppendLine("  DropTableOnSalvageDestroy");
            componentData.AppendLine(RetrieveFields(entity.Read<DropTableOnSalvageDestroy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DropTableOnSalvageDestroy>()));
        }
        if (entity.Has<Durability>())
        {
            componentData.AppendLine("  Durability");
            componentData.AppendLine(RetrieveFields(entity.Read<Durability>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Durability>()));
        }
        if (entity.Has<DurabilityTarget>())
        {
            componentData.AppendLine("  DurabilityTarget");
            componentData.AppendLine(RetrieveFields(entity.Read<DurabilityTarget>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DurabilityTarget>()));
        }
        if (entity.Has<DyeableCastleObject>())
        {
            componentData.AppendLine("  DyeableCastleObject");
            componentData.AppendLine(RetrieveFields(entity.Read<DyeableCastleObject>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DyeableCastleObject>()));
        }
        if (entity.Has<DynamicCollision>())
        {
            componentData.AppendLine("  DynamicCollision");
            componentData.AppendLine(RetrieveFields(entity.Read<DynamicCollision>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DynamicCollision>()));
        }
        if (entity.Has<DynamicallyWeakenAttackers>())
        {
            componentData.AppendLine("  DynamicallyWeakenAttackers");
            componentData.AppendLine(RetrieveFields(entity.Read<DynamicallyWeakenAttackers>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<DynamicallyWeakenAttackers>()));
        }
        if (entity.Has<EditableTileModel>())
        {
            componentData.AppendLine("  EditableTileModel");
            componentData.AppendLine(RetrieveFields(entity.Read<EditableTileModel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EditableTileModel>()));
        }
        if (entity.Has<EmoteAbility>())
        {
            componentData.AppendLine("  EmoteAbility");
            var buffer = ReadBuffer<EmoteAbility>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EmoteAbility>()));
        }
        if (entity.Has<Emoter>())
        {
            componentData.AppendLine("  Emoter");
            componentData.AppendLine(RetrieveFields(entity.Read<Emoter>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Emoter>()));
        }
        if (entity.Has<Energy>())
        {
            componentData.AppendLine("  Energy");
            componentData.AppendLine(RetrieveFields(entity.Read<Energy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Energy>()));
        }
        if (entity.Has<EntitiesInView_Server>())
        {
            componentData.AppendLine("  EntitiesInView_Server");
            var buffer = ReadBuffer<EntitiesInView_Server>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntitiesInView_Server>()));
        }
        if (entity.Has<EntityAimData>())
        {
            componentData.AppendLine("  EntityAimData");
            componentData.AppendLine(RetrieveFields(entity.Read<EntityAimData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityAimData>()));
        }
        if (entity.Has<EntityCategory>())
        {
            componentData.AppendLine("  EntityCategory");
            componentData.AppendLine(RetrieveFields(entity.Read<EntityCategory>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityCategory>()));
        }
        if (entity.Has<EntityCreator>())
        {
            componentData.AppendLine("  EntityCreator");
            componentData.AppendLine(RetrieveFields(entity.Read<EntityCreator>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityCreator>()));
        }
        if (entity.Has<EntityInput>())
        {
            componentData.AppendLine("  EntityInput");
            componentData.AppendLine(RetrieveFields(entity.Read<EntityInput>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityInput>()));
		}
		if (entity.Has<EntityMetadata>())
		{
			componentData.AppendLine("  EntityMetadata");
			componentData.AppendLine(RetrieveFields(entity.Read<EntityMetadata>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityMetadata>()));
		}
		if (entity.Has<EntityModificationBuffer>())
        {
            componentData.AppendLine("  EntityModificationBuffer");
            var buffer = ReadBuffer<EntityModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityModificationBuffer>()));
        }
        if (entity.Has<EntityOwner>())
        {
            componentData.AppendLine("  EntityOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<EntityOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EntityOwner>()));
        }
		if (entity.Has<EntitySpawnedMetadata>())
		{
			componentData.AppendLine("  EntitySpawnedMetadata");
			componentData.AppendLine(RetrieveFields(entity.Read<EntitySpawnedMetadata>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<EntitySpawnedMetadata>()));
		}
		if (entity.Has<Equipment>())
        {
            componentData.AppendLine("  Equipment");
            componentData.AppendLine(RetrieveFields(entity.Read<Equipment>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Equipment>()));
        }
        if (entity.Has<EquipmentSetBuff>())
        {
            componentData.AppendLine("  EquipmentSetBuff");
            var buffer = ReadBuffer<EquipmentSetBuff>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EquipmentSetBuff>()));
        }
        if (entity.Has<EquippableData>())
        {
            componentData.AppendLine("  EquippableData");
            componentData.AppendLine(RetrieveFields(entity.Read<EquippableData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EquippableData>()));
        }
        if (entity.Has<EvenSpreadCluster_Tick_DataServer>())
        {
            componentData.AppendLine("  EvenSpreadCluster_Tick_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<EvenSpreadCluster_Tick_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<EvenSpreadCluster_Tick_DataServer>()));
        }
        if (entity.Has<ExternalAggroBufferElement>())
        {
            componentData.AppendLine("  ExternalAggroBufferElement");
            var buffer = ReadBuffer<ExternalAggroBufferElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ExternalAggroBufferElement>()));
        }
        if (entity.Has<FactionReference>())
        {
            componentData.AppendLine("  FactionReference");
            componentData.AppendLine(RetrieveFields(entity.Read<FactionReference>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FactionReference>()));
        }
        if (entity.Has<FadeToBlack>())
        {
            componentData.AppendLine("  FadeToBlack");
            componentData.AppendLine(RetrieveFields(entity.Read<FadeToBlack>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FadeToBlack>()));
        }
        if (entity.Has<FadeToBlack_Manual>())
        {
            componentData.AppendLine("  FadeToBlack_Manual");
            componentData.AppendLine(RetrieveFields(entity.Read<FadeToBlack_Manual>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FadeToBlack_Manual>()));
        }
        if (entity.Has<FallToHeight>())
        {
            componentData.AppendLine("  FallToHeight");
            componentData.AppendLine(RetrieveFields(entity.Read<FallToHeight>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FallToHeight>()));
        }
        if (entity.Has<FeedPrisoner>())
        {
            componentData.AppendLine("  FeedPrisoner");
            componentData.AppendLine(RetrieveFields(entity.Read<FeedPrisoner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FeedPrisoner>()));
        }
        if (entity.Has<FeedableInventory>())
        {
            componentData.AppendLine("  FeedableInventory");
            componentData.AppendLine(RetrieveFields(entity.Read<FeedableInventory>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FeedableInventory>()));
        }
        if (entity.Has<Float3ModificationBuffer>())
        {
            componentData.AppendLine("  Float3ModificationBuffer");
            var buffer = ReadBuffer<Float3ModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Float3ModificationBuffer>()));
        }
        if (entity.Has<FloatModificationBuffer>())
        {
            componentData.AppendLine("  FloatModificationBuffer");
            var buffer = ReadBuffer<FloatModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FloatModificationBuffer>()));
        }
        if (entity.Has<Follower>())
        {
            componentData.AppendLine("  Follower");
            componentData.AppendLine(RetrieveFields(entity.Read<Follower>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Follower>()));
        }
        if (entity.Has<FollowerBuffer>())
        {
            componentData.AppendLine("  FollowerBuffer");
            var buffer = ReadBuffer<FollowerBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FollowerBuffer>()));
        }
        if (entity.Has<ForceCastOnGameplayEvent>())
        {
            componentData.AppendLine("  ForceCastOnGameplayEvent");
            var buffer = ReadBuffer<ForceCastOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ForceCastOnGameplayEvent>()));
        }
        if (entity.Has<Forge_Shared>())
        {
            componentData.AppendLine("  Forge_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<Forge_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Forge_Shared>()));
        }
        if (entity.Has<FrameChanged>())
        {
            componentData.AppendLine("  FrameChanged");
            componentData.AppendLine(RetrieveFields(entity.Read<FrameChanged>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<FrameChanged>()));
        }
        if (entity.Has<GainAggroByAlert>())
        {
            componentData.AppendLine("  GainAggroByAlert");
            componentData.AppendLine(RetrieveFields(entity.Read<GainAggroByAlert>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GainAggroByAlert>()));
        }
        if (entity.Has<GainAggroByVicinity>())
        {
            componentData.AppendLine("  GainAggroByVicinity");
            componentData.AppendLine(RetrieveFields(entity.Read<GainAggroByVicinity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GainAggroByVicinity>()));
        }
        if (entity.Has<GainAlertByVicinity>())
        {
            componentData.AppendLine("  GainAlertByVicinity");
            componentData.AppendLine(RetrieveFields(entity.Read<GainAlertByVicinity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GainAlertByVicinity>()));
        }
        if (entity.Has<GameplayEventIdMapping>())
        {
            componentData.AppendLine("  GameplayEventIdMapping");
            var buffer = ReadBuffer<GameplayEventIdMapping>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GameplayEventIdMapping>()));
        }
        if (entity.Has<GameplayEventListeners>())
        {
            componentData.AppendLine("  GameplayEventListeners");
            var buffer = ReadBuffer<GameplayEventListeners>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GameplayEventListeners>()));
        }
        if (entity.Has<GarlicArea>())
        {
            componentData.AppendLine("  GarlicArea");
            componentData.AppendLine(RetrieveFields(entity.Read<GarlicArea>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GarlicArea>()));
        }
        if (entity.Has<GenerateAggroOnGameplayEvent>())
        {
            componentData.AppendLine("  GenerateAggroOnGameplayEvent");
            var buffer = ReadBuffer<GenerateAggroOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GenerateAggroOnGameplayEvent>()));
        }
        if (entity.Has<GeneratedName>())
        {
            componentData.AppendLine("  GeneratedName");
            componentData.AppendLine(RetrieveFields(entity.Read<GeneratedName>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GeneratedName>()));
        }
        if (entity.Has<GenericCombatMovementData>())
        {
            componentData.AppendLine("  GenericCombatMovementData");
            componentData.AppendLine(RetrieveFields(entity.Read<GenericCombatMovementData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GenericCombatMovementData>()));
        }
        if (entity.Has<GetOwnerPrimaryAggroTargetOnSpawn>())
        {
            componentData.AppendLine("  GetOwnerPrimaryAggroTargetOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<GetOwnerPrimaryAggroTargetOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GetOwnerPrimaryAggroTargetOnSpawn>()));
        }
        if (entity.Has<GetOwnerRotation>())
        {
            componentData.AppendLine("  GetOwnerRotation");
            componentData.AppendLine(RetrieveFields(entity.Read<GetOwnerRotation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GetOwnerRotation>()));
        }
        if (entity.Has<GetOwnerRotationOnlyOnSpawnTag>())
        {
            componentData.AppendLine("  GetOwnerRotationOnlyOnSpawnTag");
            componentData.AppendLine(RetrieveFields(entity.Read<GetOwnerRotationOnlyOnSpawnTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GetOwnerRotationOnlyOnSpawnTag>()));
        }
        if (entity.Has<GetOwnerTeamOnSpawn>())
        {
            componentData.AppendLine("  GetOwnerTeamOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<GetOwnerTeamOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GetOwnerTeamOnSpawn>()));
        }
        if (entity.Has<GetOwnerTranslationOnSpawn>())
        {
            componentData.AppendLine("  GetOwnerTranslationOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<GetOwnerTranslationOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GetOwnerTranslationOnSpawn>()));
        }
        if (entity.Has<GiveAchievementOnSpawn>())
        {
            componentData.AppendLine("  GiveAchievementOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<GiveAchievementOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GiveAchievementOnSpawn>()));
        }
        if (entity.Has<GlobalCooldown>())
        {
            componentData.AppendLine("  GlobalCooldown");
            componentData.AppendLine(RetrieveFields(entity.Read<GlobalCooldown>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GlobalCooldown>()));
        }
        if (entity.Has<GuaranteedStaticTransform>())
        {
            componentData.AppendLine("  GuaranteedStaticTransform");
            componentData.AppendLine(RetrieveFields(entity.Read<GuaranteedStaticTransform>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<GuaranteedStaticTransform>()));
        }
        if (entity.Has<HeadgearToggleData>())
        {
            componentData.AppendLine("  HeadgearToggleData");
            componentData.AppendLine(RetrieveFields(entity.Read<HeadgearToggleData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HeadgearToggleData>()));
        }
        if (entity.Has<HealOnGameplayEvent>())
        {
            componentData.AppendLine("  HealOnGameplayEvent");
            var buffer = ReadBuffer<HealOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HealOnGameplayEvent>()));
        }
        if (entity.Has<HealingBuff>())
        {
            componentData.AppendLine("  HealingBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<HealingBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HealingBuff>()));
        }
        if (entity.Has<Health>())
        {
            componentData.AppendLine("  Health");
            componentData.AppendLine(RetrieveFields(entity.Read<Health>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Health>()));
        }
        if (entity.Has<HealthConstants>())
        {
            componentData.AppendLine("  HealthConstants");
            componentData.AppendLine(RetrieveFields(entity.Read<HealthConstants>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HealthConstants>()));
        }
        if (entity.Has<Height>())
        {
            componentData.AppendLine("  Height");
            componentData.AppendLine(RetrieveFields(entity.Read<Height>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Height>()));
        }
        if (entity.Has<HideOutsideVision>())
        {
            componentData.AppendLine("  HideOutsideVision");
            componentData.AppendLine(RetrieveFields(entity.Read<HideOutsideVision>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HideOutsideVision>()));
        }
        if (entity.Has<HideTargetHUD>())
        {
            componentData.AppendLine("  HideTargetHUD");
            componentData.AppendLine(RetrieveFields(entity.Read<HideTargetHUD>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HideTargetHUD>()));
        }
        if (entity.Has<HideWeapon>())
        {
            componentData.AppendLine("  HideWeapon");
            componentData.AppendLine(RetrieveFields(entity.Read<HideWeapon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HideWeapon>()));
        }
        if (entity.Has<HideWeaponDuringCast>())
        {
            componentData.AppendLine("  HideWeaponDuringCast");
            componentData.AppendLine(RetrieveFields(entity.Read<HideWeaponDuringCast>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HideWeaponDuringCast>()));
        }
        if (entity.Has<Hideable>())
        {
            componentData.AppendLine("  Hideable");
            componentData.AppendLine(RetrieveFields(entity.Read<Hideable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Hideable>()));
        }
        if (entity.Has<HitColliderCast>())
        {
            componentData.AppendLine("  HitColliderCast");
            var buffer = ReadBuffer<HitColliderCast>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HitColliderCast>()));
        }
        if (entity.Has<HitTrigger>())
        {
            componentData.AppendLine("  HitTrigger");
            var buffer = ReadBuffer<HitTrigger>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HitTrigger>()));
        }
        if (entity.Has<HolyArea>())
        {
            componentData.AppendLine("  HolyArea");
            componentData.AppendLine(RetrieveFields(entity.Read<HolyArea>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HolyArea>()));
        }
        if (entity.Has<HybridCameraFrustumPlanes>())
        {
            componentData.AppendLine("  HybridCameraFrustumPlanes");
            var buffer = ReadBuffer<HybridCameraFrustumPlanes>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HybridCameraFrustumPlanes>()));
        }
        if (entity.Has<HybridModelSeed>())
        {
            componentData.AppendLine("  HybridModelSeed");
            componentData.AppendLine(RetrieveFields(entity.Read<HybridModelSeed>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<HybridModelSeed>()));
        }
        if (entity.Has<IgnoreInCombatBuff>())
        {
            componentData.AppendLine("  IgnoreInCombatBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<IgnoreInCombatBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IgnoreInCombatBuff>()));
        }
        if (entity.Has<IgnorePvETag>())
        {
            componentData.AppendLine("  IgnorePvETag");
            componentData.AppendLine(RetrieveFields(entity.Read<IgnorePvETag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IgnorePvETag>()));
        }
        if (entity.Has<ImmaterialWhileRaided>())
        {
            componentData.AppendLine("  ImmaterialWhileRaided");
            componentData.AppendLine(RetrieveFields(entity.Read<ImmaterialWhileRaided>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ImmaterialWhileRaided>()));
        }
        if (entity.Has<Immortal>())
        {
            componentData.AppendLine("  Immortal");
            componentData.AppendLine(RetrieveFields(entity.Read<Immortal>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Immortal>()));
        }
        if (entity.Has<ImpactMaterial>())
        {
            componentData.AppendLine("  ImpactMaterial");
            componentData.AppendLine(RetrieveFields(entity.Read<ImpactMaterial>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ImpactMaterial>()));
        }
        if (entity.Has<ImprisonedBuff>())
        {
            componentData.AppendLine("  ImprisonedBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<ImprisonedBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ImprisonedBuff>()));
        }
        if (entity.Has<InputCommandBufferElement>())
        {
            componentData.AppendLine("  InputCommandBufferElement");
            var buffer = ReadBuffer<InputCommandBufferElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<InputCommandBufferElement>()));
        }
        if (entity.Has<IncomingClientMessage>())
        {
            componentData.AppendLine("  IncomingClientMessage");
            var buffer = ReadBuffer<IncomingClientMessage>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IncomingClientMessage>()));
        }
        if (entity.Has<IncomingNetBuffer>())
        {
            componentData.AppendLine("  IncomingNetBuffer");
            var buffer = ReadBuffer<IncomingNetBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IncomingNetBuffer>()));
        }
        if (entity.Has<InputCommandDataProxy>())
        {
            componentData.AppendLine("  InputCommandDataProxy");
            componentData.AppendLine(RetrieveFields(entity.Read<InputCommandDataProxy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InputCommandDataProxy>()));
        }
        if (entity.Has<InputCommandState>())
        {
            componentData.AppendLine("  InputCommandState");
            componentData.AppendLine(RetrieveFields(entity.Read<InputCommandState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InputCommandState>()));
        }
        if (entity.Has<InputCommandStateHistoryBufferElement>())
        {
            componentData.AppendLine("  InputCommandStateHistoryBufferElement");
            var buffer = ReadBuffer<InputCommandStateHistoryBufferElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<InputCommandStateHistoryBufferElement>()));
        }
        if (entity.Has<IntModificationBuffer>())
        {
            componentData.AppendLine("  IntModificationBuffer");
            var buffer = ReadBuffer<IntModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IntModificationBuffer>()));
        }
        if (entity.Has<InteractAbilityBuffer>())
        {
            componentData.AppendLine("  InteractAbilityBuffer");
            var buffer = ReadBuffer<InteractAbilityBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
			}
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InteractAbilityBuffer>()));
        }
        if (entity.Has<Interactable>())
        {
            componentData.AppendLine("  Interactable");
            componentData.AppendLine(RetrieveFields(entity.Read<Interactable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Interactable>()));
        }
        if (entity.Has<InteractedUpon>())
        {
            componentData.AppendLine("  InteractedUpon");
            componentData.AppendLine(RetrieveFields(entity.Read<InteractedUpon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InteractedUpon>()));
        }
        if (entity.Has<Interactor>())
        {
            componentData.AppendLine("  Interactor");
            componentData.AppendLine(RetrieveFields(entity.Read<Interactor>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Interactor>()));
        }
        if (entity.Has<InventoryBuffer>())
        {
            componentData.AppendLine("  InventoryBuffer");
            var buffer = ReadBuffer<InventoryBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryBuffer>()));
        }
        if (entity.Has<InventoryConnection>())
        {
            componentData.AppendLine("  InventoryConnection");
            componentData.AppendLine(RetrieveFields(entity.Read<InventoryConnection>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryConnection>()));
        }
        if (entity.Has<InventoryInstanceElement>())
        {
            componentData.AppendLine("  InventoryInstanceElement");
            var buffer = ReadBuffer<InventoryInstanceElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryInstanceElement>()));
        }
        if (entity.Has<InventoryItem>())
        {
            componentData.AppendLine("  InventoryItem");
            componentData.AppendLine(RetrieveFields(entity.Read<InventoryItem>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryItem>()));
        }
        if (entity.Has<InventoryOwner>())
        {
            componentData.AppendLine("  InventoryOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<InventoryOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryOwner>()));
        }
        if (entity.Has<InventoryStartItems>())
        {
            componentData.AppendLine("  InventoryStartItems");
            componentData.AppendLine(RetrieveFields(entity.Read<InventoryStartItems>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<InventoryStartItems>()));
		}
		if (entity.Has<IsConnected>())
		{
			componentData.AppendLine("  IsConnected");
			componentData.AppendLine(RetrieveFields(entity.Read<IsConnected>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<IsConnected>()));
		}
		if (entity.Has<IsSpellControlled>())
        {
            componentData.AppendLine("  IsSpellControlled");
            componentData.AppendLine(RetrieveFields(entity.Read<IsSpellControlled>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IsSpellControlled>()));
        }
        if (entity.Has<ItemData>())
        {
            componentData.AppendLine("  ItemData");
            componentData.AppendLine(RetrieveFields(entity.Read<ItemData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ItemData>()));
        }
        if (entity.Has<ItemPickup>())
        {
            componentData.AppendLine("  ItemPickup");
            componentData.AppendLine(RetrieveFields(entity.Read<ItemPickup>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ItemPickup>()));
        }
        if (entity.Has<IterateThroughGameplayEvent>())
        {
            componentData.AppendLine("  IterateThroughGameplayEvent");
            var buffer = ReadBuffer<IterateThroughGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<IterateThroughGameplayEvent>()));
        }
        if (entity.Has<JewelArithmeticModification>())
        {
            componentData.AppendLine("  JewelArithmeticModification");
            var buffer = ReadBuffer<JewelArithmeticModification>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelArithmeticModification>()));
        }
        if (entity.Has<JewelCraftingProcessingRequiredItem>())
        {
            componentData.AppendLine("  JewelCraftingProcessingRequiredItem");
            var buffer = ReadBuffer<JewelCraftingProcessingRequiredItem>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelCraftingProcessingRequiredItem>()));
        }
        if (entity.Has<JewelCraftingStation>())
        {
            componentData.AppendLine("  JewelCraftingStation");
            componentData.AppendLine(RetrieveFields(entity.Read<JewelCraftingStation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelCraftingStation>()));
        }
        if (entity.Has<JewelInstance>())
        {
            componentData.AppendLine("  JewelInstance");
            componentData.AppendLine(RetrieveFields(entity.Read<JewelInstance>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelInstance>()));
        }
        if (entity.Has<JewelLevelSource>())
        {
            componentData.AppendLine("  JewelLevelSource");
            componentData.AppendLine(RetrieveFields(entity.Read<JewelLevelSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelLevelSource>()));
        }
        if (entity.Has<JewelTemplate>())
        {
            componentData.AppendLine("  JewelTemplate");
            componentData.AppendLine(RetrieveFields(entity.Read<JewelTemplate>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JewelTemplate>()));
        }
        if (entity.Has<JoinDefaultTeamOnSpawn>())
        {
            componentData.AppendLine("  JoinDefaultTeamOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<JoinDefaultTeamOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JoinDefaultTeamOnSpawn>()));
        }
        if (entity.Has<JumpFromCliffs>())
        {
            componentData.AppendLine("  JumpFromCliffs");
            componentData.AppendLine(RetrieveFields(entity.Read<JumpFromCliffs>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JumpFromCliffs>()));
        }
        if (entity.Has<JumpFromCliffsTravelBuff>())
        {
            componentData.AppendLine("  JumpFromCliffsTravelBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<JumpFromCliffsTravelBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<JumpFromCliffsTravelBuff>()));
        }
        if (entity.Has<LastPathRequest>())
        {
            componentData.AppendLine("  LastPathRequest");
            componentData.AppendLine(RetrieveFields(entity.Read<LastPathRequest>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LastPathRequest>()));
        }
        if (entity.Has<LastTranslation>())
        {
            componentData.AppendLine("  LastTranslation");
            componentData.AppendLine(RetrieveFields(entity.Read<LastTranslation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LastTranslation>()));
        }
        if (entity.Has<Latency>())
        {
            componentData.AppendLine("  Latency");
            componentData.AppendLine(RetrieveFields(entity.Read<Latency>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Latency>()));
        }
        if (entity.Has<LaunchProjectileFromKiller_DataServer>())
        {
            componentData.AppendLine("  LaunchProjectileFromKiller_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<LaunchProjectileFromKiller_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LaunchProjectileFromKiller_DataServer>()));
        }
        if (entity.Has<LegDirection_Server>())
        {
            componentData.AppendLine("  LegDirection_Server");
            componentData.AppendLine(RetrieveFields(entity.Read<LegDirection_Server>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegDirection_Server>()));
        }
        if (entity.Has<LegDirection_Shared>())
        {
            componentData.AppendLine("  LegDirection_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<LegDirection_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegDirection_Shared>()));
        }
        if (entity.Has<LegendaryItemGeneratorTemplate>())
        {
            componentData.AppendLine("  LegendaryItemGeneratorTemplate");
            componentData.AppendLine(RetrieveFields(entity.Read<LegendaryItemGeneratorTemplate>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegendaryItemGeneratorTemplate>()));
        }
        if (entity.Has<LegendaryItemInstance>())
        {
            componentData.AppendLine("  LegendaryItemInstance");
            componentData.AppendLine(RetrieveFields(entity.Read<LegendaryItemInstance>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegendaryItemInstance>()));
        }
        if (entity.Has<LegendaryItemSpellModSetComponent>())
        {
            componentData.AppendLine("  LegendaryItemSpellModSetComponent");
            componentData.AppendLine(RetrieveFields(entity.Read<LegendaryItemSpellModSetComponent>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegendaryItemSpellModSetComponent>()));
        }
        if (entity.Has<LegendaryItemTemplate>())
        {
            componentData.AppendLine("  LegendaryItemTemplate");
            componentData.AppendLine(RetrieveFields(entity.Read<LegendaryItemTemplate>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LegendaryItemTemplate>()));
        }
        if (entity.Has<LifeLeech>())
        {
            componentData.AppendLine("  LifeLeech");
            componentData.AppendLine(RetrieveFields(entity.Read<LifeLeech>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LifeLeech>()));
        }
        if (entity.Has<LifeTime>())
        {
            componentData.AppendLine("  LifeTime");
            componentData.AppendLine(RetrieveFields(entity.Read<LifeTime>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LifeTime>()));
        }
        if (entity.Has<LightningAttractorAmbience>())
        {
            componentData.AppendLine("  LightningAttractorAmbience");
            componentData.AppendLine(RetrieveFields(entity.Read<LightningAttractorAmbience>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LightningAttractorAmbience>()));
        }
        if (entity.Has<LightningAttractorGameplay>())
        {
            componentData.AppendLine("  LightningAttractorGameplay");
            componentData.AppendLine(RetrieveFields(entity.Read<LightningAttractorGameplay>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LightningAttractorGameplay>()));
        }
        if (entity.Has<LightningConsumer>())
        {
            componentData.AppendLine("  LightningConsumer");
            componentData.AppendLine(RetrieveFields(entity.Read<LightningConsumer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LightningConsumer>()));
        }
        if (entity.Has<LightningRodStation>())
        {
            componentData.AppendLine("  LightningRodStation");
            componentData.AppendLine(RetrieveFields(entity.Read<LightningRodStation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LightningRodStation>()));
        }
        if (entity.Has<LimitAbilityPriorityBuff>())
        {
            componentData.AppendLine("  LimitAbilityPriorityBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<LimitAbilityPriorityBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LimitAbilityPriorityBuff>()));
        }
        if (entity.Has<LinkedEntityGroup>())
        {
            componentData.AppendLine("  LinkedEntityGroup");
            var buffer = ReadBuffer<LinkedEntityGroup>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LinkedEntityGroup>()));
        }
        if (entity.Has<LocalToWorld>())
        {
            componentData.AppendLine("  LocalToWorld");
            componentData.AppendLine(RetrieveFields(entity.Read<LocalToWorld>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LocalToWorld>()));
        }
        if (entity.Has<LongModificationBuffer>())
        {
            componentData.AppendLine("  LongModificationBuffer");
            var buffer = ReadBuffer<LongModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<LongModificationBuffer>()));
        }
        if (entity.Has<ManualFirstFrameLastTranslation>())
        {
            componentData.AppendLine("  ManualFirstFrameLastTranslation");
            componentData.AppendLine(RetrieveFields(entity.Read<ManualFirstFrameLastTranslation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ManualFirstFrameLastTranslation>()));
        }
        if (entity.Has<MapCollision>())
        {
            componentData.AppendLine("  MapCollision");
            componentData.AppendLine(RetrieveFields(entity.Read<MapCollision>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapCollision>()));
        }
        if (entity.Has<MapIconData>())
        {
            componentData.AppendLine("  MapIconData");
            componentData.AppendLine(RetrieveFields(entity.Read<MapIconData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapIconData>()));
        }
        if (entity.Has<MapIconPerUserData>())
        {
            componentData.AppendLine("  MapIconPerUserData");
            var buffer = ReadBuffer<MapIconPerUserData>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<MapIconPerUserData>()));
        }
        if (entity.Has<MapIconPosition>())
        {
            componentData.AppendLine("  MapIconPosition");
            componentData.AppendLine(RetrieveFields(entity.Read<MapIconPosition>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapIconPosition>()));
        }
        if (entity.Has<MapIconTargetEntity>())
        {
            componentData.AppendLine("  MapIconTargetEntity");
            componentData.AppendLine(RetrieveFields(entity.Read<MapIconTargetEntity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapIconTargetEntity>()));
        }
        if (entity.Has<MapPylonArea>())
        {
            componentData.AppendLine("  MapPylonArea");
            componentData.AppendLine(RetrieveFields(entity.Read<MapPylonArea>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapPylonArea>()));
        }
        if (entity.Has<MapZoneData>())
        {
            componentData.AppendLine("  MapZoneData");
            componentData.AppendLine(RetrieveFields(entity.Read<MapZoneData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MapZoneData>()));
        }
        if (entity.Has<MaxMinionsPerPlayerElement>())
        {
            componentData.AppendLine("  MaxMinionsPerPlayerElement");
            var buffer = ReadBuffer<MaxMinionsPerPlayerElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MaxMinionsPerPlayerElement>()));
        }
        if (entity.Has<Minion>())
        {
            componentData.AppendLine("  Minion");
            componentData.AppendLine(RetrieveFields(entity.Read<Minion>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Minion>()));
		}
		if (entity.Has<MinionBuffer>())
		{
			componentData.AppendLine("  MinionBuffer");
			var buffer = ReadBuffer<MinionBuffer>(entity);
			for (int i = 0; i < buffer.Length; i++)
			{
				componentData.AppendLine($"   [{i}]");
				componentData.AppendLine(RetrieveFields(buffer[i]));
			}
			checkedTypes.Add(new ComponentType(Il2CppType.Of<MinionBuffer>()));
		}
		if (entity.Has<MinionMaster>())
        {
            componentData.AppendLine("  MinionMaster");
            componentData.AppendLine(RetrieveFields(entity.Read<MinionMaster>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MinionMaster>()));
        }
        if (entity.Has<MiscAiGameplayData>())
        {
            componentData.AppendLine("  MiscAiGameplayData");
            componentData.AppendLine(RetrieveFields(entity.Read<MiscAiGameplayData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MiscAiGameplayData>()));
        }
        if (entity.Has<ModifiableValueIds>())
        {
            componentData.AppendLine("  ModifiableValueIds");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifiableValueIds>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifiableValueIds>()));
        }
        if (entity.Has<ModifyBloodDrainBuff>())
        {
            componentData.AppendLine("  ModifyBloodDrainBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifyBloodDrainBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyBloodDrainBuff>()));
        }
        if (entity.Has<ModifyMovementDuringCastData>())
        {
            componentData.AppendLine("  ModifyMovementDuringCastData");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifyMovementDuringCastData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyMovementDuringCastData>()));
        }
        if (entity.Has<ModifyMovementSpeedBuff>())
        {
            componentData.AppendLine("  ModifyMovementSpeedBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifyMovementSpeedBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyMovementSpeedBuff>()));
        }
        if (entity.Has<ModifyRotation>())
        {
            componentData.AppendLine("  ModifyRotation");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifyRotation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyRotation>()));
        }
        if (entity.Has<ModifyRotationDuringCast>())
        {
            componentData.AppendLine("  ModifyRotationDuringCast");
            componentData.AppendLine(RetrieveFields(entity.Read<ModifyRotationDuringCast>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyRotationDuringCast>()));
        }
        if (entity.Has<ModifyUnitStatBuff_DOTS>())
        {
            componentData.AppendLine("  ModifyUnitStatBuff_DOTS");
            var buffer = ReadBuffer<ModifyUnitStatBuff_DOTS>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ModifyUnitStatBuff_DOTS>()));
        }
        if (entity.Has<Mountable>())
        {
            componentData.AppendLine("  Mountable");
            componentData.AppendLine(RetrieveFields(entity.Read<Mountable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Mountable>()));
        }
        if (entity.Has<Mounter>())
        {
            componentData.AppendLine("  Mounter");
            componentData.AppendLine(RetrieveFields(entity.Read<Mounter>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Mounter>()));
        }
        if (entity.Has<MoveEntity>())
        {
            componentData.AppendLine("  MoveEntity");
            componentData.AppendLine(RetrieveFields(entity.Read<MoveEntity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MoveEntity>()));
        }
        if (entity.Has<MoveStopTrigger>())
        {
            componentData.AppendLine("  MoveStopTrigger");
            componentData.AppendLine(RetrieveFields(entity.Read<MoveStopTrigger>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MoveStopTrigger>()));
        }
        if (entity.Has<MoveVelocity>())
        {
            componentData.AppendLine("  MoveVelocity");
            componentData.AppendLine(RetrieveFields(entity.Read<MoveVelocity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MoveVelocity>()));
        }
        if (entity.Has<Movement>())
        {
            componentData.AppendLine("  Movement");
            componentData.AppendLine(RetrieveFields(entity.Read<Movement>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Movement>()));
        }
        if (entity.Has<MovementSpeedStackModifier>())
        {
            componentData.AppendLine("  MovementSpeedStackModifier");
            componentData.AppendLine(RetrieveFields(entity.Read<MovementSpeedStackModifier>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MovementSpeedStackModifier>()));
        }
        if (entity.Has<MultiplyAbsorbCapBySpellPower>())
        {
            componentData.AppendLine("  MultiplyAbsorbCapBySpellPower");
            componentData.AppendLine(RetrieveFields(entity.Read<MultiplyAbsorbCapBySpellPower>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<MultiplyAbsorbCapBySpellPower>()));
        }
        if (entity.Has<NPCServantColorIndex>())
        {
            componentData.AppendLine("  NPCServantColorIndex");
            componentData.AppendLine(RetrieveFields(entity.Read<NPCServantColorIndex>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NPCServantColorIndex>()));
        }
        if (entity.Has<NameableInteractable>())
        {
            componentData.AppendLine("  NameableInteractable");
            componentData.AppendLine(RetrieveFields(entity.Read<NameableInteractable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NameableInteractable>()));
        }
        if (entity.Has<NetSnapshot>())
        {
            componentData.AppendLine("  NetSnapshot");
            var buffer = ReadBuffer<NetSnapshot>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetSnapshot>()));
        }
        if (entity.Has<NetherSpawnPosition>())
        {
            componentData.AppendLine("  NetherSpawnPosition");
            componentData.AppendLine(RetrieveFields(entity.Read<NetherSpawnPosition>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetherSpawnPosition>()));
        }
        if (entity.Has<NetworkId>())
        {
            componentData.AppendLine("  NetworkId");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkId>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkId>()));
        }
        if (entity.Has<NetworkInterpolated_Shared>())
        {
            componentData.AppendLine("  NetworkInterpolated_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkInterpolated_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkInterpolated_Shared>()));
        }
        if (entity.Has<NetworkSnapshot>())
        {
            componentData.AppendLine("  NetworkSnapshot");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkSnapshot>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkSnapshot>()));
        }
        if (entity.Has<NetworkSnapshotType>())
        {
            componentData.AppendLine("  NetworkSnapshotType");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkSnapshotType>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkSnapshotType>()));
        }
        if (entity.Has<Networked>())
        {
            componentData.AppendLine("  Networked");
            componentData.AppendLine(RetrieveFields(entity.Read<Networked>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Networked>()));
        }
        if (entity.Has<NetworkedPrefabChildren>())
        {
            componentData.AppendLine("  NetworkedPrefabChildren");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkedPrefabChildren>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkedPrefabChildren>()));
        }
        if (entity.Has<NetworkedSequence>())
        {
            componentData.AppendLine("  NetworkedSequence");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkedSequence>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkedSequence>()));
        }
        if (entity.Has<NetworkedSettings>())
        {
            componentData.AppendLine("  NetworkedSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkedSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkedSettings>()));
        }
        if (entity.Has<NetworkedTimeout>())
        {
            componentData.AppendLine("  NetworkedTimeout");
            componentData.AppendLine(RetrieveFields(entity.Read<NetworkedTimeout>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NetworkedTimeout>()));
        }
        if (entity.Has<NonUniformScale>())
        {
            componentData.AppendLine("  NonUniformScale");
            componentData.AppendLine(RetrieveFields(entity.Read<NonUniformScale>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<NonUniformScale>()));
        }
        if (entity.Has<OffsetLastTranslationOnSpawn>())
        {
            componentData.AppendLine("  OffsetLastTranslationOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<OffsetLastTranslationOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OffsetLastTranslationOnSpawn>()));
        }
        if (entity.Has<OffsetTranslationOnSpawn>())
        {
            componentData.AppendLine("  OffsetTranslationOnSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<OffsetTranslationOnSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OffsetTranslationOnSpawn>()));
        }
        if (entity.Has<OffsetTranslationOnSpawnBlockerSettings>())
        {
            componentData.AppendLine("  OffsetTranslationOnSpawnBlockerSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<OffsetTranslationOnSpawnBlockerSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OffsetTranslationOnSpawnBlockerSettings>()));
        }
        if (entity.Has<OnlySyncToUserBitMask>())
        {
            componentData.AppendLine("  OnlySyncToUserBitMask");
            componentData.AppendLine(RetrieveFields(entity.Read<OnlySyncToUserBitMask>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OnlySyncToUserBitMask>()));
        }
        if (entity.Has<OnlySyncToUserBuffer>())
        {
            componentData.AppendLine("  OnlySyncToUserBuffer");
            var buffer = ReadBuffer<OnlySyncToUserBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OnlySyncToUserBuffer>()));
        }
        if (entity.Has<OpenDoors>())
        {
            componentData.AppendLine("  OpenDoors");
            componentData.AppendLine(RetrieveFields(entity.Read<OpenDoors>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OpenDoors>()));
        }
        if (entity.Has<OpenDoorsBuffer>())
        {
            componentData.AppendLine("  OpenDoorsBuffer");
            var buffer = ReadBuffer<OpenDoorsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<OpenDoorsBuffer>()));
        }
        if (entity.Has<PathBuffer>())
        {
            componentData.AppendLine("  PathBuffer");
            var buffer = ReadBuffer<PathBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PathBuffer>()));
        }
        if (entity.Has<PathRequestFilledSegmentBuffer>())
        {
            componentData.AppendLine("  PathRequestFilledSegmentBuffer");
            var buffer = ReadBuffer<PathRequestFilledSegmentBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PathRequestFilledSegmentBuffer>()));
        }
        if (entity.Has<PathRequestSolveDebugBuffer>())
        {
            componentData.AppendLine("  PathRequestSolveDebugBuffer");
            var buffer = ReadBuffer<PathRequestSolveDebugBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PathRequestSolveDebugBuffer>()));
        }
        if (entity.Has<Pathfinder>())
        {
            componentData.AppendLine("  Pathfinder");
            componentData.AppendLine(RetrieveFields(entity.Read<Pathfinder>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Pathfinder>()));
        }
        if (entity.Has<PavementBonus>())
        {
            componentData.AppendLine("  PavementBonus");
            componentData.AppendLine(RetrieveFields(entity.Read<PavementBonus>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PavementBonus>()));
        }
        if (entity.Has<PavementBonusSource>())
        {
            componentData.AppendLine("  PavementBonusSource");
            componentData.AppendLine(RetrieveFields(entity.Read<PavementBonusSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PavementBonusSource>()));
        }
        if (entity.Has<PerksBuffer>())
        {
            componentData.AppendLine("  PerksBuffer");
            var buffer = ReadBuffer<PerksBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PerksBuffer>()));
        }
        if (entity.Has<PhysicsCollider>())
        {
            componentData.AppendLine("  PhysicsCollider");
            componentData.AppendLine(RetrieveFields(entity.Read<PhysicsCollider>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PhysicsCollider>()));
        }
        if (entity.Has<PhysicsRubble>())
        {
            componentData.AppendLine("  PhysicsRubble");
            componentData.AppendLine(RetrieveFields(entity.Read<PhysicsRubble>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PhysicsRubble>()));
        }
        if (entity.Has<PlacementDestroyData>())
        {
            componentData.AppendLine("  PlacementDestroyData");
            componentData.AppendLine(RetrieveFields(entity.Read<PlacementDestroyData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlacementDestroyData>()));
        }
        if (entity.Has<PlacementLimitToSet>())
        {
            componentData.AppendLine("  PlacementLimitToSet");
            componentData.AppendLine(RetrieveFields(entity.Read<PlacementLimitToSet>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlacementLimitToSet>()));
        }
        if (entity.Has<PlayImpactOnGameplayEvent>())
        {
            componentData.AppendLine("  PlayImpactOnGameplayEvent");
            var buffer = ReadBuffer<PlayImpactOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlayImpactOnGameplayEvent>()));
        }
        if (entity.Has<PlaySequenceOnDeath>())
        {
            componentData.AppendLine("  PlaySequenceOnDeath");
            var buffer = ReadBuffer<PlaySequenceOnDeath>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlaySequenceOnDeath>()));
        }
        if (entity.Has<PlaySequenceOnGameplayEvent>())
        {
            componentData.AppendLine("  PlaySequenceOnGameplayEvent");
            var buffer = ReadBuffer<PlaySequenceOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlaySequenceOnGameplayEvent>()));
        }
        if (entity.Has<PlaySequenceOnPickup>())
        {
            componentData.AppendLine("  PlaySequenceOnPickup");
            componentData.AppendLine(RetrieveFields(entity.Read<PlaySequenceOnPickup>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlaySequenceOnPickup>()));
        }
        if (entity.Has<PlayerCharacter>())
        {
            componentData.AppendLine("  PlayerCharacter");
            componentData.AppendLine(RetrieveFields(entity.Read<PlayerCharacter>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlayerCharacter>()));
        }
        if (entity.Has<PlayerDeathContainer>())
        {
            componentData.AppendLine("  PlayerDeathContainer");
            componentData.AppendLine(RetrieveFields(entity.Read<PlayerDeathContainer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlayerDeathContainer>()));
        }
        if (entity.Has<PlayerMapIcon>())
        {
            componentData.AppendLine("  PlayerMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<PlayerMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PlayerMapIcon>()));
        }
        if (entity.Has<Prefab>())
        {
            componentData.AppendLine("  Prefab");
            componentData.AppendLine(RetrieveFields(entity.Read<Prefab>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Prefab>()));
        }
        if (entity.Has<PrefabCollectionPrefabTag>())
        {
            componentData.AppendLine("  PrefabCollectionPrefabTag");
            componentData.AppendLine(RetrieveFields(entity.Read<PrefabCollectionPrefabTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PrefabCollectionPrefabTag>()));
        }
        if (entity.Has<PrefabGUID>())
        {
			var prefabGuid = entity.Read<PrefabGUID>();
            componentData.AppendLine($"  PrefabGUID - {prefabGuid.LookupName()}");
            componentData.AppendLine(RetrieveFields(prefabGuid));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PrefabGUID>()));
        }

		if (entity.Has<PrefabGUIDModificationBuffer>())
        {
            componentData.AppendLine("  PrefabGUIDModificationBuffer");
            var buffer = ReadBuffer<PrefabGUIDModificationBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PrefabGUIDModificationBuffer>()));
        }
        if (entity.Has<PreventDisableBuff>())
        {
            componentData.AppendLine("  PreventDisableBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<PreventDisableBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PreventDisableBuff>()));
        }
        if (entity.Has<Prisonstation>())
        {
            componentData.AppendLine("  Prisonstation");
            componentData.AppendLine(RetrieveFields(entity.Read<Prisonstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Prisonstation>()));
        }
        if (entity.Has<ProfessorCoil>())
        {
            componentData.AppendLine("  ProfessorCoil");
            componentData.AppendLine(RetrieveFields(entity.Read<ProfessorCoil>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProfessorCoil>()));
        }
        if (entity.Has<ProfessorCoilBeam_Data_Server>())
        {
            componentData.AppendLine("  ProfessorCoilBeam_Data_Server");
            componentData.AppendLine(RetrieveFields(entity.Read<ProfessorCoilBeam_Data_Server>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProfessorCoilBeam_Data_Server>()));
        }
        if (entity.Has<ProfessorCoilBeam_State_Shared>())
        {
            componentData.AppendLine("  ProfessorCoilBeam_State_Shared");
            componentData.AppendLine(RetrieveFields(entity.Read<ProfessorCoilBeam_State_Shared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProfessorCoilBeam_State_Shared>()));
        }
        if (entity.Has<ProgressionBookBlueprintElement>())
        {
            componentData.AppendLine("  ProgressionBookBlueprintElement");
            var buffer = ReadBuffer<ProgressionBookBlueprintElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionBookBlueprintElement>()));
        }
        if (entity.Has<ProgressionBookRecipeElement>())
        {
            componentData.AppendLine("  ProgressionBookRecipeElement");
            var buffer = ReadBuffer<ProgressionBookRecipeElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionBookRecipeElement>()));
        }
        if (entity.Has<ProgressionBookShapeshiftElement>())
        {
            componentData.AppendLine("  ProgressionBookShapeshiftElement");
            var buffer = ReadBuffer<ProgressionBookShapeshiftElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionBookShapeshiftElement>()));
        }
        if (entity.Has<ProgressionBookTechElement>())
        {
            componentData.AppendLine("  ProgressionBookTechElement");
            var buffer = ReadBuffer<ProgressionBookTechElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionBookTechElement>()));
        }
        if (entity.Has<ProgressionDependencyElement>())
        {
            componentData.AppendLine("  ProgressionDependencyElement");
            var buffer = ReadBuffer<ProgressionDependencyElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionDependencyElement>()));
        }
        if (entity.Has<ProgressionGain>())
        {
            componentData.AppendLine("  ProgressionGain");
            componentData.AppendLine(RetrieveFields(entity.Read<ProgressionGain>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionGain>()));
		}
		if (entity.Has<ProgressionMapper>())
		{
			componentData.AppendLine("  ProgressionMapper");
			componentData.AppendLine(RetrieveFields(entity.Read<ProgressionMapper>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionMapper>()));
		}
		if (entity.Has<ProgressionUserContentDependency>())
        {
            componentData.AppendLine("  ProgressionUserContentDependency");
            componentData.AppendLine(RetrieveFields(entity.Read<ProgressionUserContentDependency>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProgressionUserContentDependency>()));
        }
        if (entity.Has<Projectile>())
        {
            componentData.AppendLine("  Projectile");
            componentData.AppendLine(RetrieveFields(entity.Read<Projectile>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Projectile>()));
        }
        if (entity.Has<ProjectileDestroyData>())
        {
            componentData.AppendLine("  ProjectileDestroyData");
            componentData.AppendLine(RetrieveFields(entity.Read<ProjectileDestroyData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProjectileDestroyData>()));
        }
        if (entity.Has<ProjectileSnapToHeight>())
        {
            componentData.AppendLine("  ProjectileSnapToHeight");
            componentData.AppendLine(RetrieveFields(entity.Read<ProjectileSnapToHeight>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProjectileSnapToHeight>()));
        }
        if (entity.Has<Pylonstation>())
        {
            componentData.AppendLine("  Pylonstation");
            componentData.AppendLine(RetrieveFields(entity.Read<Pylonstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Pylonstation>()));
        }
        if (entity.Has<PylonstationCastleClaimBuffer>())
        {
            componentData.AppendLine("  PylonstationCastleClaimBuffer");
            var buffer = ReadBuffer<PylonstationCastleClaimBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PylonstationCastleClaimBuffer>()));
        }
        if (entity.Has<PylonstationCastleDestroyBuffer>())
        {
            componentData.AppendLine("  PylonstationCastleDestroyBuffer");
            var buffer = ReadBuffer<PylonstationCastleDestroyBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PylonstationCastleDestroyBuffer>()));
        }
        if (entity.Has<PylonstationCastleRaidBuffer>())
        {
            componentData.AppendLine("  PylonstationCastleRaidBuffer");
            var buffer = ReadBuffer<PylonstationCastleRaidBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PylonstationCastleRaidBuffer>()));
        }
        if (entity.Has<PylonstationUpgradesBuffer>())
        {
            componentData.AppendLine("  PylonstationUpgradesBuffer");
            var buffer = ReadBuffer<PylonstationUpgradesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<PylonstationUpgradesBuffer>()));
        }
        if (entity.Has<QueuedTransitionRequests>())
        {
            componentData.AppendLine("  QueuedTransitionRequests");
            var buffer = ReadBuffer<QueuedTransitionRequests>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<QueuedTransitionRequests>()));
        }
        if (entity.Has<QueuedWorkstationCraftAction>())
        {
            componentData.AppendLine("  QueuedWorkstationCraftAction");
            var buffer = ReadBuffer<QueuedWorkstationCraftAction>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<QueuedWorkstationCraftAction>()));
        }
        if (entity.Has<QueuedWorkstationCraftActionItems>())
        {
            componentData.AppendLine("  QueuedWorkstationCraftActionItems");
            var buffer = ReadBuffer<QueuedWorkstationCraftActionItems>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<QueuedWorkstationCraftActionItems>()));
        }
        if (entity.Has<RadialDamageTarget>())
        {
            componentData.AppendLine("  RadialDamageTarget");
            componentData.AppendLine(RetrieveFields(entity.Read<RadialDamageTarget>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RadialDamageTarget>()));
        }
        if (entity.Has<RadialZone_Environment_Data>())
        {
            componentData.AppendLine("  RadialZone_Environment_Data");
            componentData.AppendLine(RetrieveFields(entity.Read<RadialZone_Environment_Data>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RadialZone_Environment_Data>()));
        }
        if (entity.Has<RadialZone_Environment_HitSpheres>())
        {
            componentData.AppendLine("  RadialZone_Environment_HitSpheres");
            var buffer = ReadBuffer<RadialZone_Environment_HitSpheres>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RadialZone_Environment_HitSpheres>()));
        }
        if (entity.Has<RagdollForceSource>())
        {
            componentData.AppendLine("  RagdollForceSource");
            componentData.AppendLine(RetrieveFields(entity.Read<RagdollForceSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RagdollForceSource>()));
        }
        if (entity.Has<RandomBloodTypeBuffer>())
        {
            componentData.AppendLine("  RandomBloodTypeBuffer");
            var buffer = ReadBuffer<RandomBloodTypeBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RandomBloodTypeBuffer>()));
        }
        if (entity.Has<RecipeRequirementBuffer>())
        {
            componentData.AppendLine("  RecipeRequirementBuffer");
            var buffer = ReadBuffer<RecipeRequirementBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RecipeRequirementBuffer>()));
        }
        if (entity.Has<Refinementstation>())
        {
            componentData.AppendLine("  Refinementstation");
            componentData.AppendLine(RetrieveFields(entity.Read<Refinementstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Refinementstation>()));
        }
        if (entity.Has<RefinementstationRecipesBuffer>())
        {
            componentData.AppendLine("  RefinementstationRecipesBuffer");
            var buffer = ReadBuffer<RefinementstationRecipesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RefinementstationRecipesBuffer>()));
        }
        if (entity.Has<Relic>())
        {
            componentData.AppendLine("  Relic");
            componentData.AppendLine(RetrieveFields(entity.Read<Relic>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Relic>()));
        }
        if (entity.Has<RelicMapIcon>())
        {
            componentData.AppendLine("  RelicMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<RelicMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RelicMapIcon>()));
        }
        if (entity.Has<RelicRadar>())
        {
            componentData.AppendLine("  RelicRadar");
            componentData.AppendLine(RetrieveFields(entity.Read<RelicRadar>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RelicRadar>()));
        }
        if (entity.Has<RelicSpawnBoundMapIcon>())
        {
            componentData.AppendLine("  RelicSpawnBoundMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<RelicSpawnBoundMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RelicSpawnBoundMapIcon>()));
        }
        if (entity.Has<RemoveBuffOnGameplayEvent>())
        {
            componentData.AppendLine("  RemoveBuffOnGameplayEvent");
            var buffer = ReadBuffer<RemoveBuffOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RemoveBuffOnGameplayEvent>()));
        }
        if (entity.Has<RemoveBuffOnGameplayEventEntry>())
        {
            componentData.AppendLine("  RemoveBuffOnGameplayEventEntry");
            var buffer = ReadBuffer<RemoveBuffOnGameplayEventEntry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RemoveBuffOnGameplayEventEntry>()));
        }
        if (entity.Has<ReplaceAbilityOnSlotBuff>())
        {
            componentData.AppendLine("  ReplaceAbilityOnSlotBuff");
            var buffer = ReadBuffer<ReplaceAbilityOnSlotBuff>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ReplaceAbilityOnSlotBuff>()));
        }
        if (entity.Has<ReplaceAbilityOnSlotData>())
        {
            componentData.AppendLine("  ReplaceAbilityOnSlotData");
            componentData.AppendLine(RetrieveFields(entity.Read<ReplaceAbilityOnSlotData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ReplaceAbilityOnSlotData>()));
        }
        if (entity.Has<ReplaceAbilityOnSlotWhenMountedBuffElement>())
        {
            componentData.AppendLine("  ReplaceAbilityOnSlotWhenMountedBuffElement");
            var buffer = ReadBuffer<ReplaceAbilityOnSlotWhenMountedBuffElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ReplaceAbilityOnSlotWhenMountedBuffElement>()));
        }
        if (entity.Has<ReplaceAbilityOnSlotWhenMountedBuffModificationElement>())
        {
            componentData.AppendLine("  ReplaceAbilityOnSlotWhenMountedBuffModificationElement");
            var buffer = ReadBuffer<ReplaceAbilityOnSlotWhenMountedBuffModificationElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ReplaceAbilityOnSlotWhenMountedBuffModificationElement>()));
        }
        if (entity.Has<RequireGroundedTag>())
        {
            componentData.AppendLine("  RequireGroundedTag");
            componentData.AppendLine(RetrieveFields(entity.Read<RequireGroundedTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RequireGroundedTag>()));
        }
        if (entity.Has<ResearchBuffer>())
        {
            componentData.AppendLine("  ResearchBuffer");
            var buffer = ReadBuffer<ResearchBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ResearchBuffer>()));
        }
        if (entity.Has<ResearchStation>())
        {
            componentData.AppendLine("  ResearchStation");
            componentData.AppendLine(RetrieveFields(entity.Read<ResearchStation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ResearchStation>()));
        }
        if (entity.Has<Residency>())
        {
            componentData.AppendLine("  Residency");
            componentData.AppendLine(RetrieveFields(entity.Read<Residency>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Residency>()));
        }
        if (entity.Has<Resident>())
        {
            componentData.AppendLine("  Resident");
            componentData.AppendLine(RetrieveFields(entity.Read<Resident>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Resident>()));
        }
        if (entity.Has<ResistCategoryStats>())
        {
            componentData.AppendLine("  ResistCategoryStats");
            componentData.AppendLine(RetrieveFields(entity.Read<ResistCategoryStats>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ResistCategoryStats>()));
        }
        if (entity.Has<ResistanceData>())
        {
            componentData.AppendLine("  ResistanceData");
            componentData.AppendLine(RetrieveFields(entity.Read<ResistanceData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ResistanceData>()));
        }
        if (entity.Has<RespawnCharacter>())
        {
            componentData.AppendLine("  RespawnCharacter");
            componentData.AppendLine(RetrieveFields(entity.Read<RespawnCharacter>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RespawnCharacter>()));
        }
        if (entity.Has<RespawnPoint>())
        {
            componentData.AppendLine("  RespawnPoint");
            componentData.AppendLine(RetrieveFields(entity.Read<RespawnPoint>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RespawnPoint>()));
        }
        if (entity.Has<RespawnPointOwnerBuffer>())
        {
            componentData.AppendLine("  RespawnPointOwnerBuffer");
            var buffer = ReadBuffer<RespawnPointOwnerBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RespawnPointOwnerBuffer>()));
        }
        if (entity.Has<RestrictPlacementArea>())
        {
            componentData.AppendLine("  RestrictPlacementArea");
            componentData.AppendLine(RetrieveFields(entity.Read<RestrictPlacementArea>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RestrictPlacementArea>()));
        }
        if (entity.Has<RestrictPlacementToMapZones>())
        {
            componentData.AppendLine("  RestrictPlacementToMapZones");
            componentData.AppendLine(RetrieveFields(entity.Read<RestrictPlacementToMapZones>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RestrictPlacementToMapZones>()));
        }
        if (entity.Has<RestrictedInventory>())
        {
            componentData.AppendLine("  RestrictedInventory");
            componentData.AppendLine(RetrieveFields(entity.Read<RestrictedInventory>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RestrictedInventory>()));
        }
        if (entity.Has<Restricted_InventoryBuffer>())
        {
            componentData.AppendLine("  Restricted_InventoryBuffer");
            var buffer = ReadBuffer<Restricted_InventoryBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Restricted_InventoryBuffer>()));
        }
        if (entity.Has<ReturnToNetherWaypoint>())
        {
            componentData.AppendLine("  ReturnToNetherWaypoint");
            componentData.AppendLine(RetrieveFields(entity.Read<ReturnToNetherWaypoint>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ReturnToNetherWaypoint>()));
        }
        if (entity.Has<RoofCategory>())
        {
            componentData.AppendLine("  RoofCategory");
            componentData.AppendLine(RetrieveFields(entity.Read<RoofCategory>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RoofCategory>()));
        }
        if (entity.Has<RoofInstanceTypeId>())
        {
            componentData.AppendLine("  RoofInstanceTypeId");
            componentData.AppendLine(RetrieveFields(entity.Read<RoofInstanceTypeId>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RoofInstanceTypeId>()));
        }
        if (entity.Has<RoofRootBlobElement>())
        {
            componentData.AppendLine("  RoofRootBlobElement");
            var buffer = ReadBuffer<RoofRootBlobElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RoofRootBlobElement>()));
        }
        if (entity.Has<RoofTileData>())
        {
            componentData.AppendLine("  RoofTileData");
            componentData.AppendLine(RetrieveFields(entity.Read<RoofTileData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RoofTileData>()));
        }
        if (entity.Has<RoofTileVariations>())
        {
            componentData.AppendLine("  RoofTileVariations");
            var buffer = ReadBuffer<RoofTileVariations>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RoofTileVariations>()));
        }
        if (entity.Has<Rotation>())
        {
            componentData.AppendLine("  Rotation");
            componentData.AppendLine(RetrieveFields(entity.Read<Rotation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Rotation>()));
        }
        if (entity.Has<RunCastleCleanupOnDeath>())
        {
            componentData.AppendLine("  RunCastleCleanupOnDeath");
            componentData.AppendLine(RetrieveFields(entity.Read<RunCastleCleanupOnDeath>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RunCastleCleanupOnDeath>()));
        }
        if (entity.Has<RunScriptOnGameplayEvent>())
        {
            componentData.AppendLine("  RunScriptOnGameplayEvent");
            var buffer = ReadBuffer<RunScriptOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RunScriptOnGameplayEvent>()));
        }
        if (entity.Has<RunScriptOnPreCastFinished>())
        {
            componentData.AppendLine("  RunScriptOnPreCastFinished");
            componentData.AppendLine(RetrieveFields(entity.Read<RunScriptOnPreCastFinished>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<RunScriptOnPreCastFinished>()));
        }
        if (entity.Has<SaddleBearer>())
        {
            componentData.AppendLine("  SaddleBearer");
            componentData.AppendLine(RetrieveFields(entity.Read<SaddleBearer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SaddleBearer>()));
        }
        if (entity.Has<SaddleData>())
        {
            componentData.AppendLine("  SaddleData");
            componentData.AppendLine(RetrieveFields(entity.Read<SaddleData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SaddleData>()));
        }
        if (entity.Has<Salvageable>())
        {
            componentData.AppendLine("  Salvageable");
            componentData.AppendLine(RetrieveFields(entity.Read<Salvageable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Salvageable>()));
        }
        if (entity.Has<Salvagestation>())
        {
            componentData.AppendLine("  Salvagestation");
            componentData.AppendLine(RetrieveFields(entity.Read<Salvagestation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Salvagestation>()));
        }
        if (entity.Has<SchoolDebuffData>())
        {
            componentData.AppendLine("  SchoolDebuffData");
            componentData.AppendLine(RetrieveFields(entity.Read<SchoolDebuffData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SchoolDebuffData>()));
        }
        if (entity.Has<ScriptDestroy>())
        {
            componentData.AppendLine("  ScriptDestroy");
            componentData.AppendLine(RetrieveFields(entity.Read<ScriptDestroy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ScriptDestroy>()));
        }
        if (entity.Has<ScriptSpawn>())
        {
            componentData.AppendLine("  ScriptSpawn");
            componentData.AppendLine(RetrieveFields(entity.Read<ScriptSpawn>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ScriptSpawn>()));
        }
        if (entity.Has<ScriptUpdate>())
        {
            componentData.AppendLine("  ScriptUpdate");
            componentData.AppendLine(RetrieveFields(entity.Read<ScriptUpdate>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ScriptUpdate>()));
        }
        if (entity.Has<Script_ApplyBuffUnderHealthThreshhold_DataServer>())
        {
            componentData.AppendLine("  Script_ApplyBuffUnderHealthThreshhold_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ApplyBuffUnderHealthThreshhold_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ApplyBuffUnderHealthThreshhold_DataServer>()));
        }
        if (entity.Has<Script_ApplyBuffUnderHealthThreshhold_MonsterVBlood_DataServer>())
        {
            componentData.AppendLine("  Script_ApplyBuffUnderHealthThreshhold_MonsterVBlood_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ApplyBuffUnderHealthThreshhold_MonsterVBlood_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ApplyBuffUnderHealthThreshhold_MonsterVBlood_DataServer>()));
        }
        if (entity.Has<Script_ApplyBuffUnderThreeHealthThreshholdsDataCarrier_DataServer>())
        {
            componentData.AppendLine("  Script_ApplyBuffUnderThreeHealthThreshholdsDataCarrier_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ApplyBuffUnderThreeHealthThreshholdsDataCarrier_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ApplyBuffUnderThreeHealthThreshholdsDataCarrier_DataServer>()));
        }
        if (entity.Has<Script_ApplyBuffUnderThreeHealthThreshholds_DataServer>())
        {
            componentData.AppendLine("  Script_ApplyBuffUnderThreeHealthThreshholds_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ApplyBuffUnderThreeHealthThreshholds_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ApplyBuffUnderThreeHealthThreshholds_DataServer>()));
        }
        if (entity.Has<Script_Buff_FreeCast_DataServer>())
        {
            componentData.AppendLine("  Script_Buff_FreeCast_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_Buff_FreeCast_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_Buff_FreeCast_DataServer>()));
        }
        if (entity.Has<Script_Buff_ModifyAggroFactor_DataServer>())
        {
            componentData.AppendLine("  Script_Buff_ModifyAggroFactor_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_Buff_ModifyAggroFactor_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_Buff_ModifyAggroFactor_DataServer>()));
        }
        if (entity.Has<Script_ConfuseDummy_BuffedEntitiesBuffer>())
        {
            componentData.AppendLine("  Script_ConfuseDummy_BuffedEntitiesBuffer");
            var buffer = ReadBuffer<Script_ConfuseDummy_BuffedEntitiesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ConfuseDummy_BuffedEntitiesBuffer>()));
        }
        if (entity.Has<Script_ConfuseDummy_DataServer>())
        {
            componentData.AppendLine("  Script_ConfuseDummy_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ConfuseDummy_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ConfuseDummy_DataServer>()));
        }
        if (entity.Has<Script_CreateGameplayEventOnAreaEnterExit_DataServer>())
        {
            componentData.AppendLine("  Script_CreateGameplayEventOnAreaEnterExit_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_CreateGameplayEventOnAreaEnterExit_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_CreateGameplayEventOnAreaEnterExit_DataServer>()));
        }
        if (entity.Has<Script_CreateGameplayEventOnAreaEnterExit_Entry>())
        {
            componentData.AppendLine("  Script_CreateGameplayEventOnAreaEnterExit_Entry");
            var buffer = ReadBuffer<Script_CreateGameplayEventOnAreaEnterExit_Entry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_CreateGameplayEventOnAreaEnterExit_Entry>()));
        }
        if (entity.Has<Script_CreateGameplayEventOnBuffTargetDeath_DataServer>())
        {
            componentData.AppendLine("  Script_CreateGameplayEventOnBuffTargetDeath_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_CreateGameplayEventOnBuffTargetDeath_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_CreateGameplayEventOnBuffTargetDeath_DataServer>()));
        }
        if (entity.Has<Script_CreateGameplayEventOnDamageDealtToEntityCategory_DataServer>())
        {
            componentData.AppendLine("  Script_CreateGameplayEventOnDamageDealtToEntityCategory_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_CreateGameplayEventOnDamageDealtToEntityCategory_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_CreateGameplayEventOnDamageDealtToEntityCategory_DataServer>()));
        }
        if (entity.Has<Script_CreateGameplayEventOnDamageTakenToEntityCategory_DataServer>())
        {
            componentData.AppendLine("  Script_CreateGameplayEventOnDamageTakenToEntityCategory_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_CreateGameplayEventOnDamageTakenToEntityCategory_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_CreateGameplayEventOnDamageTakenToEntityCategory_DataServer>()));
        }
        if (entity.Has<Script_InspectTarget_Data>())
        {
            componentData.AppendLine("  Script_InspectTarget_Data");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_InspectTarget_Data>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_InspectTarget_Data>()));
        }
        if (entity.Has<Script_ModifySpellAbilityCooldownOnGameplayEvent_DataServer>())
        {
            componentData.AppendLine("  Script_ModifySpellAbilityCooldownOnGameplayEvent_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_ModifySpellAbilityCooldownOnGameplayEvent_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_ModifySpellAbilityCooldownOnGameplayEvent_DataServer>()));
        }
        if (entity.Has<Script_Modify_Combat_Movement_Buff_Data>())
        {
            componentData.AppendLine("  Script_Modify_Combat_Movement_Buff_Data");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_Modify_Combat_Movement_Buff_Data>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_Modify_Combat_Movement_Buff_Data>()));
        }
        if (entity.Has<Script_Modify_Combat_Movement_Buff_State>())
        {
            componentData.AppendLine("  Script_Modify_Combat_Movement_Buff_State");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_Modify_Combat_Movement_Buff_State>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_Modify_Combat_Movement_Buff_State>()));
        }
        if (entity.Has<Script_SCTChatOnAggro_Buffer>())
        {
            componentData.AppendLine("  Script_SCTChatOnAggro_Buffer");
            var buffer = ReadBuffer<Script_SCTChatOnAggro_Buffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_SCTChatOnAggro_Buffer>()));
        }
        if (entity.Has<Script_SCTChatOnSpawn_Buffer>())
        {
            componentData.AppendLine("  Script_SCTChatOnSpawn_Buffer");
            var buffer = ReadBuffer<Script_SCTChatOnSpawn_Buffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_SCTChatOnSpawn_Buffer>()));
        }
        if (entity.Has<Script_SetHealthToOwnerPercentage_DataServer>())
        {
            componentData.AppendLine("  Script_SetHealthToOwnerPercentage_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_SetHealthToOwnerPercentage_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_SetHealthToOwnerPercentage_DataServer>()));
        }
        if (entity.Has<Script_StealthBush_Environment_ActiveStealths>())
        {
            componentData.AppendLine("  Script_StealthBush_Environment_ActiveStealths");
            var buffer = ReadBuffer<Script_StealthBush_Environment_ActiveStealths>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_StealthBush_Environment_ActiveStealths>()));
        }
        if (entity.Has<Script_StealthBush_Environment_HitSpheres>())
        {
            componentData.AppendLine("  Script_StealthBush_Environment_HitSpheres");
            var buffer = ReadBuffer<Script_StealthBush_Environment_HitSpheres>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_StealthBush_Environment_HitSpheres>()));
        }
        if (entity.Has<Script_UnitSpawn_DataServer>())
        {
            componentData.AppendLine("  Script_UnitSpawn_DataServer");
            componentData.AppendLine(RetrieveFields(entity.Read<Script_UnitSpawn_DataServer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Script_UnitSpawn_DataServer>()));
        }
        if (entity.Has<ServantCoffinEffects>())
        {
            componentData.AppendLine("  ServantCoffinEffects");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantCoffinEffects>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantCoffinEffects>()));
        }
        if (entity.Has<ServantCoffinstation>())
        {
            componentData.AppendLine("  ServantCoffinstation");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantCoffinstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantCoffinstation>()));
        }
        if (entity.Has<ServantConnectedCoffin>())
        {
            componentData.AppendLine("  ServantConnectedCoffin");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantConnectedCoffin>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantConnectedCoffin>()));
        }
        if (entity.Has<ServantConvertRequirement>())
        {
            componentData.AppendLine("  ServantConvertRequirement");
            var buffer = ReadBuffer<ServantConvertRequirement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantConvertRequirement>()));
        }
        if (entity.Has<ServantConvertable>())
        {
            componentData.AppendLine("  ServantConvertable");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantConvertable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantConvertable>()));
        }
        if (entity.Has<ServantData>())
        {
            componentData.AppendLine("  ServantData");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantData>()));
        }
        if (entity.Has<ServantEquipment>())
        {
            componentData.AppendLine("  ServantEquipment");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantEquipment>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantEquipment>()));
        }
        if (entity.Has<ServantInteractPointLocalTransform>())
        {
            componentData.AppendLine("  ServantInteractPointLocalTransform");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantInteractPointLocalTransform>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantInteractPointLocalTransform>()));
        }
        if (entity.Has<ServantPower>())
        {
            componentData.AppendLine("  ServantPower");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantPower>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantPower>()));
        }
        if (entity.Has<ServantPowerConstants>())
        {
            componentData.AppendLine("  ServantPowerConstants");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantPowerConstants>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantPowerConstants>()));
        }
        if (entity.Has<ServantTypeData>())
        {
            componentData.AppendLine("  ServantTypeData");
            componentData.AppendLine(RetrieveFields(entity.Read<ServantTypeData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServantTypeData>()));
        }
        if (entity.Has<ServerControlsPositionBuff>())
        {
            componentData.AppendLine("  ServerControlsPositionBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<ServerControlsPositionBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServerControlsPositionBuff>()));
        }
        if (entity.Has<ServerDebugLogs>())
        {
            componentData.AppendLine("  ServerDebugLogs");
            componentData.AppendLine(RetrieveFields(entity.Read<ServerDebugLogs>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServerDebugLogs>()));
        }
        if (entity.Has<ServerDebugViewData>())
        {
            componentData.AppendLine("  ServerDebugViewData");
            componentData.AppendLine(RetrieveFields(entity.Read<ServerDebugViewData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServerDebugViewData>()));
		}
		if (entity.Has<ServerNetworkState>())
		{
			componentData.AppendLine("  ServerNetworkState");
			componentData.AppendLine(RetrieveFields(entity.Read<ServerNetworkState>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<ServerNetworkState>()));
		}
		if (entity.Has<ServerTime>())
        {
            componentData.AppendLine("  ServerTime");
            componentData.AppendLine(RetrieveFields(entity.Read<ServerTime>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ServerTime>()));
        }
        if (entity.Has<SetOwnerRotateTowardsMouse>())
        {
            componentData.AppendLine("  SetOwnerRotateTowardsMouse");
            componentData.AppendLine(RetrieveFields(entity.Read<SetOwnerRotateTowardsMouse>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SetOwnerRotateTowardsMouse>()));
        }
        if (entity.Has<SetOwnerRotateTowardsMovement>())
        {
            componentData.AppendLine("  SetOwnerRotateTowardsMovement");
            componentData.AppendLine(RetrieveFields(entity.Read<SetOwnerRotateTowardsMovement>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SetOwnerRotateTowardsMovement>()));
        }
        if (entity.Has<Shapeshift>())
        {
            componentData.AppendLine("  Shapeshift");
            componentData.AppendLine(RetrieveFields(entity.Read<Shapeshift>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Shapeshift>()));
        }
        if (entity.Has<ShapeshiftAbility>())
        {
            componentData.AppendLine("  ShapeshiftAbility");
            var buffer = ReadBuffer<ShapeshiftAbility>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ShapeshiftAbility>()));
        }
        if (entity.Has<ShardBossHuntBuffer>())
        {
            componentData.AppendLine("  ShardBossHuntBuffer");
            var buffer = ReadBuffer<ShardBossHuntBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ShardBossHuntBuffer>()));
        }
        if (entity.Has<ShatteredItem>())
        {
            componentData.AppendLine("  ShatteredItem");
            componentData.AppendLine(RetrieveFields(entity.Read<ShatteredItem>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ShatteredItem>()));
        }
        if (entity.Has<ShatteredItemRepairCost>())
        {
            componentData.AppendLine("  ShatteredItemRepairCost");
            var buffer = ReadBuffer<ShatteredItemRepairCost>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ShatteredItemRepairCost>()));
        }
        if (entity.Has<SiegeWeapon>())
        {
            componentData.AppendLine("  SiegeWeapon");
            componentData.AppendLine(RetrieveFields(entity.Read<SiegeWeapon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SiegeWeapon>()));
        }
        if (entity.Has<SnapToHeight>())
        {
            componentData.AppendLine("  SnapToHeight");
            componentData.AppendLine(RetrieveFields(entity.Read<SnapToHeight>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SnapToHeight>()));
        }
        if (entity.Has<SnapshotFrameChangedBuffer>())
        {
            componentData.AppendLine("  SnapshotFrameChangedBuffer");
            var buffer = ReadBuffer<SnapshotFrameChangedBuffer>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<SnapshotFrameChangedBuffer>()));
        }
        if (entity.Has<Snapshot_AbilityStateBuffer>())
        {
            componentData.AppendLine("  Snapshot_AbilityStateBuffer");
            var buffer = ReadBuffer<Snapshot_AbilityStateBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_AbilityStateBuffer>()));
        }
        if (entity.Has<Snapshot_AchievementInProgressElement>())
        {
            componentData.AppendLine("  Snapshot_AchievementInProgressElement");
            var buffer = ReadBuffer<Snapshot_AchievementInProgressElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_AchievementInProgressElement>()));
        }
        if (entity.Has<Snapshot_ActiveServantMission>())
        {
            componentData.AppendLine("  Snapshot_ActiveServantMission");
            var buffer = ReadBuffer<Snapshot_ActiveServantMission>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_ActiveServantMission>()));
        }
        if (entity.Has<Snapshot_AllyPermission>())
        {
            componentData.AppendLine("  Snapshot_AllyPermission");
            var buffer = ReadBuffer<Snapshot_AllyPermission>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_AllyPermission>()));
        }
        if (entity.Has<Snapshot_CastleBuildingAttachToParentsBuffer>())
        {
            componentData.AppendLine("  Snapshot_CastleBuildingAttachToParentsBuffer");
            var buffer = ReadBuffer<Snapshot_CastleBuildingAttachToParentsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleBuildingAttachToParentsBuffer>()));
        }
        if (entity.Has<Snapshot_CastleBuildingAttachedChildrenBuffer>())
        {
            componentData.AppendLine("  Snapshot_CastleBuildingAttachedChildrenBuffer");
            var buffer = ReadBuffer<Snapshot_CastleBuildingAttachedChildrenBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleBuildingAttachedChildrenBuffer>()));
        }
        if (entity.Has<Snapshot_CastleBuildingFusedChildrenBuffer>())
        {
            componentData.AppendLine("  Snapshot_CastleBuildingFusedChildrenBuffer");
            var buffer = ReadBuffer<Snapshot_CastleBuildingFusedChildrenBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleBuildingFusedChildrenBuffer>()));
        }
        if (entity.Has<Snapshot_CastleMemberNames>())
        {
            componentData.AppendLine("  Snapshot_CastleMemberNames");
            var buffer = ReadBuffer<Snapshot_CastleMemberNames>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleMemberNames>()));
        }
        if (entity.Has<Snapshot_CastleTeleporterElement>())
        {
            componentData.AppendLine("  Snapshot_CastleTeleporterElement");
            var buffer = ReadBuffer<Snapshot_CastleTeleporterElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleTeleporterElement>()));
        }
        if (entity.Has<Snapshot_CastleTerritoryDecay>())
        {
            componentData.AppendLine("  Snapshot_CastleTerritoryDecay");
            var buffer = ReadBuffer<Snapshot_CastleTerritoryDecay>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleTerritoryDecay>()));
        }
        if (entity.Has<Snapshot_CastleTerritoryOccupant>())
        {
            componentData.AppendLine("  Snapshot_CastleTerritoryOccupant");
            var buffer = ReadBuffer<Snapshot_CastleTerritoryOccupant>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_CastleTerritoryOccupant>()));
        }
        if (entity.Has<Snapshot_ClanMemberStatus>())
        {
            componentData.AppendLine("  Snapshot_ClanMemberStatus");
            var buffer = ReadBuffer<Snapshot_ClanMemberStatus>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_ClanMemberStatus>()));
        }
        if (entity.Has<Snapshot_FollowerBuffer>())
        {
            componentData.AppendLine("  Snapshot_FollowerBuffer");
            var buffer = ReadBuffer<Snapshot_FollowerBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_FollowerBuffer>()));
        }
        if (entity.Has<Snapshot_InventoryBuffer>())
        {
            componentData.AppendLine("  Snapshot_InventoryBuffer");
            var buffer = ReadBuffer<Snapshot_InventoryBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_InventoryBuffer>()));
        }
        if (entity.Has<Snapshot_InventoryInstanceElement>())
        {
            componentData.AppendLine("  Snapshot_InventoryInstanceElement");
            var buffer = ReadBuffer<Snapshot_InventoryInstanceElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_InventoryInstanceElement>()));
        }
        if (entity.Has<Snapshot_PerksBuffer>())
        {
            componentData.AppendLine("  Snapshot_PerksBuffer");
            var buffer = ReadBuffer<Snapshot_PerksBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_PerksBuffer>()));
        }
        if (entity.Has<Snapshot_QueuedWorkstationCraftAction>())
        {
            componentData.AppendLine("  Snapshot_QueuedWorkstationCraftAction");
            var buffer = ReadBuffer<Snapshot_QueuedWorkstationCraftAction>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_QueuedWorkstationCraftAction>()));
        }
        if (entity.Has<Snapshot_RefinementstationRecipesBuffer>())
        {
            componentData.AppendLine("  Snapshot_RefinementstationRecipesBuffer");
            var buffer = ReadBuffer<Snapshot_RefinementstationRecipesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_RefinementstationRecipesBuffer>()));
        }
        if (entity.Has<Snapshot_ResearchBuffer>())
        {
            componentData.AppendLine("  Snapshot_ResearchBuffer");
            var buffer = ReadBuffer<Snapshot_ResearchBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_ResearchBuffer>()));
        }
        if (entity.Has<Snapshot_RespawnPointOwnerBuffer>())
        {
            componentData.AppendLine("  Snapshot_RespawnPointOwnerBuffer");
            var buffer = ReadBuffer<Snapshot_RespawnPointOwnerBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_RespawnPointOwnerBuffer>()));
        }
        if (entity.Has<Snapshot_Restricted_InventoryBuffer>())
        {
            componentData.AppendLine("  Snapshot_Restricted_InventoryBuffer");
            var buffer = ReadBuffer<Snapshot_Restricted_InventoryBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_Restricted_InventoryBuffer>()));
        }
        if (entity.Has<Snapshot_SpawnedUnitsBuffer>())
        {
            componentData.AppendLine("  Snapshot_SpawnedUnitsBuffer");
            var buffer = ReadBuffer<Snapshot_SpawnedUnitsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_SpawnedUnitsBuffer>()));
        }
        if (entity.Has<Snapshot_TradeCost>())
        {
            componentData.AppendLine("  Snapshot_TradeCost");
            var buffer = ReadBuffer<Snapshot_TradeCost>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_TradeCost>()));
        }
        if (entity.Has<Snapshot_TradeOutput>())
        {
            componentData.AppendLine("  Snapshot_TradeOutput");
            var buffer = ReadBuffer<Snapshot_TradeOutput>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_TradeOutput>()));
        }
        if (entity.Has<Snapshot_TraderEntry>())
        {
            componentData.AppendLine("  Snapshot_TraderEntry");
            var buffer = ReadBuffer<Snapshot_TraderEntry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_TraderEntry>()));
        }
        if (entity.Has<Snapshot_UnlockedWaypointElement>())
        {
            componentData.AppendLine("  Snapshot_UnlockedWaypointElement");
            var buffer = ReadBuffer<Snapshot_UnlockedWaypointElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_UnlockedWaypointElement>()));
        }
        if (entity.Has<Snapshot_UserMapZoneElement>())
        {
            componentData.AppendLine("  Snapshot_UserMapZoneElement");
            var buffer = ReadBuffer<Snapshot_UserMapZoneElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Snapshot_UserMapZoneElement>()));
        }
        if (entity.Has<SpawnBuffElement>())
        {
            componentData.AppendLine("  SpawnBuffElement");
            var buffer = ReadBuffer<SpawnBuffElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnBuffElement>()));
        }
        if (entity.Has<SpawnChainChild>())
        {
            componentData.AppendLine("  SpawnChainChild");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnChainChild>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnChainChild>()));
        }
        if (entity.Has<SpawnChainConstants>())
        {
            componentData.AppendLine("  SpawnChainConstants");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnChainConstants>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnChainConstants>()));
        }
        if (entity.Has<SpawnChainInstance>())
        {
            componentData.AppendLine("  SpawnChainInstance");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnChainInstance>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnChainInstance>()));
        }
        if (entity.Has<SpawnLocationSelector>())
        {
            componentData.AppendLine("  SpawnLocationSelector");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnLocationSelector>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnLocationSelector>()));
        }
        if (entity.Has<SpawnMinionOnGameplayEvent>())
        {
            componentData.AppendLine("  SpawnMinionOnGameplayEvent");
            var buffer = ReadBuffer<SpawnMinionOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnMinionOnGameplayEvent>()));
        }
        if (entity.Has<SpawnPhysicsObjectOnDeath>())
        {
            componentData.AppendLine("  SpawnPhysicsObjectOnDeath");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnPhysicsObjectOnDeath>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnPhysicsObjectOnDeath>()));
        }
        if (entity.Has<SpawnPrefabOnDestroy>())
        {
            componentData.AppendLine("  SpawnPrefabOnDestroy");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnPrefabOnDestroy>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnPrefabOnDestroy>()));
        }
        if (entity.Has<SpawnPrefabOnGameplayEvent>())
        {
            componentData.AppendLine("  SpawnPrefabOnGameplayEvent");
            var buffer = ReadBuffer<SpawnPrefabOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnPrefabOnGameplayEvent>()));
        }
        if (entity.Has<SpawnRandomLifeTime>())
        {
            componentData.AppendLine("  SpawnRandomLifeTime");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnRandomLifeTime>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnRandomLifeTime>()));
        }
        if (entity.Has<SpawnSequenceForEntity>())
        {
            componentData.AppendLine("  SpawnSequenceForEntity");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnSequenceForEntity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnSequenceForEntity>()));
        }
        if (entity.Has<SpawnTag>())
        {
            componentData.AppendLine("  SpawnTag");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnTag>()));
        }
        if (entity.Has<SpawnTransform>())
        {
            componentData.AppendLine("  SpawnTransform");
            componentData.AppendLine(RetrieveFields(entity.Read<SpawnTransform>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnTransform>()));
        }
        if (entity.Has<SpawnedUnitsBuffer>())
        {
            componentData.AppendLine("  SpawnedUnitsBuffer");
            var buffer = ReadBuffer<SpawnedUnitsBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpawnedUnitsBuffer>()));
        }
        if (entity.Has<SpellLevel>())
        {
            componentData.AppendLine("  SpellLevel");
            componentData.AppendLine(RetrieveFields(entity.Read<SpellLevel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellLevel>()));
        }
        if (entity.Has<SpellLevelSource>())
        {
            componentData.AppendLine("  SpellLevelSource");
            componentData.AppendLine(RetrieveFields(entity.Read<SpellLevelSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellLevelSource>()));
        }
        if (entity.Has<SpellModArithmetic>())
        {
            componentData.AppendLine("  SpellModArithmetic");
            var buffer = ReadBuffer<SpellModArithmetic>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellModArithmetic>()));
        }
        if (entity.Has<SpellModSetComponent>())
        {
            componentData.AppendLine("  SpellModSetComponent");
            componentData.AppendLine(RetrieveFields(entity.Read<SpellModSetComponent>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellModSetComponent>()));
        }
        if (entity.Has<SpellMovement>())
        {
            componentData.AppendLine("  SpellMovement");
            componentData.AppendLine(RetrieveFields(entity.Read<SpellMovement>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellMovement>()));
        }
        if (entity.Has<SpellTarget>())
        {
            componentData.AppendLine("  SpellTarget");
            componentData.AppendLine(RetrieveFields(entity.Read<SpellTarget>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpellTarget>()));
        }
        if (entity.Has<SpiderCocoonSpawns_DataServer>())
        {
            componentData.AppendLine("  SpiderCocoonSpawns_DataServer");
            var buffer = ReadBuffer<SpiderCocoonSpawns_DataServer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpiderCocoonSpawns_DataServer>()));
        }
        if (entity.Has<SpiderCocoon_DataShared>())
        {
            componentData.AppendLine("  SpiderCocoon_DataShared");
            componentData.AppendLine(RetrieveFields(entity.Read<SpiderCocoon_DataShared>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SpiderCocoon_DataShared>()));
        }
        if (entity.Has<StartGraveyardExitWaypoint>())
        {
            componentData.AppendLine("  StartGraveyardExitWaypoint");
            componentData.AppendLine(RetrieveFields(entity.Read<StartGraveyardExitWaypoint>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StartGraveyardExitWaypoint>()));
        }
        if (entity.Has<StartGraveyardMapIcon>())
        {
            componentData.AppendLine("  StartGraveyardMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<StartGraveyardMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StartGraveyardMapIcon>()));
        }
        if (entity.Has<StartItemBuffer>())
        {
            componentData.AppendLine("  StartItemBuffer");
            var buffer = ReadBuffer<StartItemBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StartItemBuffer>()));
        }
        if (entity.Has<StaticHierarchyBuffer>())
        {
            componentData.AppendLine("  StaticHierarchyBuffer");
            var buffer = ReadBuffer<StaticHierarchyBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticHierarchyBuffer>()));
        }
        if (entity.Has<StaticHierarchyData>())
        {
            componentData.AppendLine("  StaticHierarchyData");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticHierarchyData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticHierarchyData>()));
        }
        if (entity.Has<StaticPhysicsCollider>())
        {
            componentData.AppendLine("  StaticPhysicsCollider");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticPhysicsCollider>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticPhysicsCollider>()));
        }
        if (entity.Has<StaticPhysicsWorldBodyIndex>())
        {
            componentData.AppendLine("  StaticPhysicsWorldBodyIndex");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticPhysicsWorldBodyIndex>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticPhysicsWorldBodyIndex>()));
        }
        if (entity.Has<StaticTileModel>())
        {
            componentData.AppendLine("  StaticTileModel");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticTileModel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticTileModel>()));
        }
        if (entity.Has<StaticTransformCompatible>())
        {
            componentData.AppendLine("  StaticTransformCompatible");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticTransformCompatible>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticTransformCompatible>()));
        }
        if (entity.Has<StaticTransformIndex>())
        {
            componentData.AppendLine("  StaticTransformIndex");
            componentData.AppendLine(RetrieveFields(entity.Read<StaticTransformIndex>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StaticTransformIndex>()));
        }
        if (entity.Has<StationBonusBuffer>())
        {
            componentData.AppendLine("  StationBonusBuffer");
            var buffer = ReadBuffer<StationBonusBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StationBonusBuffer>()));
        }
        if (entity.Has<StationServants>())
        {
            componentData.AppendLine("  StationServants");
            componentData.AppendLine(RetrieveFields(entity.Read<StationServants>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StationServants>()));
        }
        if (entity.Has<Stealthable>())
        {
            componentData.AppendLine("  Stealthable");
            componentData.AppendLine(RetrieveFields(entity.Read<Stealthable>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Stealthable>()));
        }
        if (entity.Has<StoredBlood>())
        {
            componentData.AppendLine("  StoredBlood");
            componentData.AppendLine(RetrieveFields(entity.Read<StoredBlood>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StoredBlood>()));
        }
        if (entity.Has<StoredConsumeCount>())
        {
            componentData.AppendLine("  StoredConsumeCount");
            componentData.AppendLine(RetrieveFields(entity.Read<StoredConsumeCount>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<StoredConsumeCount>()));
        }
        if (entity.Has<SunDamageDebuff>())
        {
            componentData.AppendLine("  SunDamageDebuff");
            componentData.AppendLine(RetrieveFields(entity.Read<SunDamageDebuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SunDamageDebuff>()));
        }
        if (entity.Has<SwapArtWhileRaidedRoot>())
        {
            componentData.AppendLine("  SwapArtWhileRaidedRoot");
            componentData.AppendLine(RetrieveFields(entity.Read<SwapArtWhileRaidedRoot>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SwapArtWhileRaidedRoot>()));
        }
        if (entity.Has<SyncBoundingBox>())
        {
            componentData.AppendLine("  SyncBoundingBox");
            componentData.AppendLine(RetrieveFields(entity.Read<SyncBoundingBox>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SyncBoundingBox>()));
        }
        if (entity.Has<SyncedServerDebugSettings>())
        {
            componentData.AppendLine("  SyncedServerDebugSettings");
            componentData.AppendLine(RetrieveFields(entity.Read<SyncedServerDebugSettings>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<SyncedServerDebugSettings>()));
        }
        if (entity.Has<TakeDamageInSun>())
        {
            componentData.AppendLine("  TakeDamageInSun");
            componentData.AppendLine(RetrieveFields(entity.Read<TakeDamageInSun>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TakeDamageInSun>()));
        }
        if (entity.Has<TakeDamageInSunDebuffState>())
        {
            componentData.AppendLine("  TakeDamageInSunDebuffState");
            componentData.AppendLine(RetrieveFields(entity.Read<TakeDamageInSunDebuffState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TakeDamageInSunDebuffState>()));
        }
        if (entity.Has<TargetAOESequence>())
        {
            componentData.AppendLine("  TargetAOESequence");
            var buffer = ReadBuffer<TargetAOESequence>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TargetAOESequence>()));
        }
        if (entity.Has<TargetAoE>())
        {
            componentData.AppendLine("  TargetAoE");
            componentData.AppendLine(RetrieveFields(entity.Read<TargetAoE>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TargetAoE>()));
        }
        if (entity.Has<TargetDirection>())
        {
            componentData.AppendLine("  TargetDirection");
            componentData.AppendLine(RetrieveFields(entity.Read<TargetDirection>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TargetDirection>()));
        }
        if (entity.Has<Team>())
        {
            componentData.AppendLine("  Team");
            componentData.AppendLine(RetrieveFields(entity.Read<Team>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Team>()));
        }
        if (entity.Has<TeamAllies>())
        {
            componentData.AppendLine("  TeamAllies");
            var buffer = ReadBuffer<TeamAllies>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TeamAllies>()));
        }
        if (entity.Has<TeamData>())
        {
            componentData.AppendLine("  TeamData");
            componentData.AppendLine(RetrieveFields(entity.Read<TeamData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TeamData>()));
        }
        if (entity.Has<TeamReference>())
        {
            componentData.AppendLine("  TeamReference");
            componentData.AppendLine(RetrieveFields(entity.Read<TeamReference>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TeamReference>()));
        }
        if (entity.Has<TechUnlockAbilityBuffer>())
        {
            componentData.AppendLine("  TechUnlockAbilityBuffer");
            var buffer = ReadBuffer<TechUnlockAbilityBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TechUnlockAbilityBuffer>()));
        }
        if (entity.Has<Throw_Prefabs_To_Spawn>())
        {
            componentData.AppendLine("  Throw_Prefabs_To_Spawn");
            var buffer = ReadBuffer<Throw_Prefabs_To_Spawn>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Throw_Prefabs_To_Spawn>()));
        }
        if (entity.Has<TileBounds>())
        {
            componentData.AppendLine("  TileBounds");
            componentData.AppendLine(RetrieveFields(entity.Read<TileBounds>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileBounds>()));
        }
        if (entity.Has<TileCollisionHistoryElement>())
        {
            componentData.AppendLine("  TileCollisionHistoryElement");
            var buffer = ReadBuffer<TileCollisionHistoryElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<TileCollisionHistoryElement>()));
        }
        if (entity.Has<TileCollisionHistoryMetadataElement>())
        {
            componentData.AppendLine("  TileCollisionHistoryMetadataElement");
            var buffer = ReadBuffer<TileCollisionHistoryMetadataElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<TileCollisionHistoryMetadataElement>()));
        }
        if (entity.Has<TileCollisionTag>())
        {
            componentData.AppendLine("  TileCollisionTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TileCollisionTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileCollisionTag>()));
        }
        if (entity.Has<TileData>())
        {
            componentData.AppendLine("  TileData");
            componentData.AppendLine(RetrieveFields(entity.Read<TileData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileData>()));
        }
        if (entity.Has<TileDisabledCollisionHistoryElement>())
        {
            componentData.AppendLine("  TileDisabledCollisionHistoryElement");
            var buffer = ReadBuffer<TileDisabledCollisionHistoryElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<TileDisabledCollisionHistoryElement>()));
        }
        if (entity.Has<TileGameplayHeightsHistoryElement>())
        {
            componentData.AppendLine("  TileGameplayHeightsHistoryElement");
            var buffer = ReadBuffer<TileGameplayHeightsHistoryElement>(entity);
			componentData.AppendLine("    Length: " + buffer.Length);
			checkedTypes.Add(new ComponentType(Il2CppType.Of<TileGameplayHeightsHistoryElement>()));
        }
        if (entity.Has<TileHeightTag>())
        {
            componentData.AppendLine("  TileHeightTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TileHeightTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileHeightTag>()));
        }
        if (entity.Has<TileLineOfSightTag>())
        {
            componentData.AppendLine("  TileLineOfSightTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TileLineOfSightTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileLineOfSightTag>()));
        }
        if (entity.Has<TileModel>())
        {
            componentData.AppendLine("  TileModel");
            componentData.AppendLine(RetrieveFields(entity.Read<TileModel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileModel>()));
        }
        if (entity.Has<TileModelLayer>())
        {
            componentData.AppendLine("  TileModelLayer");
            componentData.AppendLine(RetrieveFields(entity.Read<TileModelLayer>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileModelLayer>()));
        }
        if (entity.Has<TileModelRegistrationState>())
        {
            componentData.AppendLine("  TileModelRegistrationState");
            componentData.AppendLine(RetrieveFields(entity.Read<TileModelRegistrationState>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileModelRegistrationState>()));
        }
        if (entity.Has<TileModelSpatialData>())
        {
            componentData.AppendLine("  TileModelSpatialData");
            componentData.AppendLine(RetrieveFields(entity.Read<TileModelSpatialData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileModelSpatialData>()));
        }
        if (entity.Has<TilePathfindingTag>())
        {
            componentData.AppendLine("  TilePathfindingTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TilePathfindingTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TilePathfindingTag>()));
        }
        if (entity.Has<TilePlacementTag>())
        {
            componentData.AppendLine("  TilePlacementTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TilePlacementTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TilePlacementTag>()));
        }
        if (entity.Has<TilePosition>())
        {
            componentData.AppendLine("  TilePosition");
            componentData.AppendLine(RetrieveFields(entity.Read<TilePosition>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TilePosition>()));
        }
        if (entity.Has<TileRestrictionAreaTag>())
        {
            componentData.AppendLine("  TileRestrictionAreaTag");
            componentData.AppendLine(RetrieveFields(entity.Read<TileRestrictionAreaTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TileRestrictionAreaTag>()));
        }
        if (entity.Has<TimeScale>())
        {
            componentData.AppendLine("  TimeScale");
            componentData.AppendLine(RetrieveFields(entity.Read<TimeScale>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TimeScale>()));
        }
        if (entity.Has<Torture>())
        {
            componentData.AppendLine("  Torture");
            componentData.AppendLine(RetrieveFields(entity.Read<Torture>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Torture>()));
        }
        if (entity.Has<TradeCost>())
        {
            componentData.AppendLine("  TradeCost");
            var buffer = ReadBuffer<TradeCost>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TradeCost>()));
        }
        if (entity.Has<TradeOutput>())
        {
            componentData.AppendLine("  TradeOutput");
            var buffer = ReadBuffer<TradeOutput>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TradeOutput>()));
        }
        if (entity.Has<Trader>())
        {
            componentData.AppendLine("  Trader");
            componentData.AppendLine(RetrieveFields(entity.Read<Trader>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Trader>()));
        }
        if (entity.Has<TraderCollectionGenerator>())
        {
            componentData.AppendLine("  TraderCollectionGenerator");
            var buffer = ReadBuffer<TraderCollectionGenerator>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TraderCollectionGenerator>()));
        }
        if (entity.Has<TraderEntry>())
        {
            componentData.AppendLine("  TraderEntry");
            var buffer = ReadBuffer<TraderEntry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TraderEntry>()));
        }
        if (entity.Has<TransitionWhenInventoryIsEmpty>())
        {
            componentData.AppendLine("  TransitionWhenInventoryIsEmpty");
            componentData.AppendLine(RetrieveFields(entity.Read<TransitionWhenInventoryIsEmpty>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TransitionWhenInventoryIsEmpty>()));
        }
        if (entity.Has<Translation>())
        {
            componentData.AppendLine("  Translation");
            componentData.AppendLine(RetrieveFields(entity.Read<Translation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Translation>()));
        }
        if (entity.Has<TravelBuff>())
        {
            componentData.AppendLine("  TravelBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<TravelBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TravelBuff>()));
        }
        if (entity.Has<TravelToTarget>())
        {
            componentData.AppendLine("  TravelToTarget");
            componentData.AppendLine(RetrieveFields(entity.Read<TravelToTarget>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TravelToTarget>()));
        }
        if (entity.Has<TravelToTargetRadius>())
        {
            componentData.AppendLine("  TravelToTargetRadius");
            componentData.AppendLine(RetrieveFields(entity.Read<TravelToTargetRadius>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TravelToTargetRadius>()));
        }
        if (entity.Has<TriggerCounterOnGameplayEvent>())
        {
            componentData.AppendLine("  TriggerCounterOnGameplayEvent");
            var buffer = ReadBuffer<TriggerCounterOnGameplayEvent>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TriggerCounterOnGameplayEvent>()));
        }
        if (entity.Has<TriggerHitConsume>())
        {
            componentData.AppendLine("  TriggerHitConsume");
            var buffer = ReadBuffer<TriggerHitConsume>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<TriggerHitConsume>()));
        }
        if (entity.Has<UnitLevel>())
        {
            componentData.AppendLine("  UnitLevel");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitLevel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitLevel>()));
        }
        if (entity.Has<UnitLevelDamageSettings>())
        {
            componentData.AppendLine("  UnitLevelDamageSettings");
            var buffer = ReadBuffer<UnitLevelDamageSettings>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitLevelDamageSettings>()));
        }
        if (entity.Has<UnitLevel_Extra>())
        {
            componentData.AppendLine("  UnitLevel_Extra");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitLevel_Extra>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitLevel_Extra>()));
        }
        if (entity.Has<UnitRespawnTime>())
        {
            componentData.AppendLine("  UnitRespawnTime");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitRespawnTime>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitRespawnTime>()));
        }
        if (entity.Has<UnitSpawnData>())
        {
            componentData.AppendLine("  UnitSpawnData");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitSpawnData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitSpawnData>()));
        }
        if (entity.Has<UnitSpawnPointBuffer>())
        {
            componentData.AppendLine("  UnitSpawnPointBuffer");
            var buffer = ReadBuffer<UnitSpawnPointBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitSpawnPointBuffer>()));
        }
        if (entity.Has<UnitSpawnerstation>())
        {
            componentData.AppendLine("  UnitSpawnerstation");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitSpawnerstation>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitSpawnerstation>()));
        }
        if (entity.Has<UnitStats>())
        {
            componentData.AppendLine("  UnitStats");
            componentData.AppendLine(RetrieveFields(entity.Read<UnitStats>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnitStats>()));
        }
        if (entity.Has<UnlockedAbilityElement>())
        {
            componentData.AppendLine("  UnlockedAbilityElement");
            var buffer = ReadBuffer<UnlockedAbilityElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedAbilityElement>()));
        }
        if (entity.Has<UnlockedBlueprintElement>())
        {
            componentData.AppendLine("  UnlockedBlueprintElement");
            var buffer = ReadBuffer<UnlockedBlueprintElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedBlueprintElement>()));
        }
        if (entity.Has<UnlockedPassiveElement>())
        {
            componentData.AppendLine("  UnlockedPassiveElement");
            var buffer = ReadBuffer<UnlockedPassiveElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedPassiveElement>()));
        }
        if (entity.Has<UnlockedProgressionElement>())
        {
            componentData.AppendLine("  UnlockedProgressionElement");
            var buffer = ReadBuffer<UnlockedProgressionElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedProgressionElement>()));
        }
        if (entity.Has<UnlockedRecipeElement>())
        {
            componentData.AppendLine("  UnlockedRecipeElement");
            var buffer = ReadBuffer<UnlockedRecipeElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedRecipeElement>()));
        }
        if (entity.Has<UnlockedShapeshiftElement>())
        {
            componentData.AppendLine("  UnlockedShapeshiftElement");
            var buffer = ReadBuffer<UnlockedShapeshiftElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedShapeshiftElement>()));
        }
        if (entity.Has<UnlockedVBlood>())
        {
            componentData.AppendLine("  UnlockedVBlood");
            var buffer = ReadBuffer<UnlockedVBlood>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedVBlood>()));
        }
        if (entity.Has<UnlockedWaypointElement>())
        {
            componentData.AppendLine("  UnlockedWaypointElement");
            var buffer = ReadBuffer<UnlockedWaypointElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnlockedWaypointElement>()));
        }
        if (entity.Has<UnsmoothedPathBuffer>())
        {
            componentData.AppendLine("  UnsmoothedPathBuffer");
            var buffer = ReadBuffer<UnsmoothedPathBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UnsmoothedPathBuffer>()));
        }
        if (entity.Has<UpToDateUserBitMask>())
        {
            componentData.AppendLine("  UpToDateUserBitMask");
            componentData.AppendLine(RetrieveFields(entity.Read<UpToDateUserBitMask>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UpToDateUserBitMask>()));
        }
        if (entity.Has<UpdateAgeWhenDisabled>())
        {
            componentData.AppendLine("  UpdateAgeWhenDisabled");
            componentData.AppendLine(RetrieveFields(entity.Read<UpdateAgeWhenDisabled>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UpdateAgeWhenDisabled>()));
        }
        if (entity.Has<UpdateLifeTimeWhenDisabled>())
        {
            componentData.AppendLine("  UpdateLifeTimeWhenDisabled");
            componentData.AppendLine(RetrieveFields(entity.Read<UpdateLifeTimeWhenDisabled>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UpdateLifeTimeWhenDisabled>()));
        }
        if (entity.Has<UpgradeableFromTileModel>())
        {
            componentData.AppendLine("  UpgradeableFromTileModel");
            componentData.AppendLine(RetrieveFields(entity.Read<UpgradeableFromTileModel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UpgradeableFromTileModel>()));
        }
        if (entity.Has<UseBossCenterPositionAsPreCombatPosition>())
        {
            componentData.AppendLine("  UseBossCenterPositionAsPreCombatPosition");
            componentData.AppendLine(RetrieveFields(entity.Read<UseBossCenterPositionAsPreCombatPosition>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UseBossCenterPositionAsPreCombatPosition>()));
        }
        if (entity.Has<User>())
        {
            componentData.AppendLine("  User");
            componentData.AppendLine(RetrieveFields(entity.Read<User>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<User>()));
        }
        if (entity.Has<UserDestroyedEntityBuffer>())
        {
            componentData.AppendLine("  UserDestroyedEntityBuffer");
            var buffer = ReadBuffer<UserDestroyedEntityBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserDestroyedEntityBuffer>()));
        }
        if (entity.Has<UserEntityNetworkState>())
        {
            componentData.AppendLine("  UserEntityNetworkState");
            var buffer = ReadBuffer<UserEntityNetworkState>(entity);
            componentData.AppendLine("    Length: " + buffer.Length);
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserEntityNetworkState>()));
        }
        if (entity.Has<UserHeartCount>())
        {
            componentData.AppendLine("  UserHeartCount");
            componentData.AppendLine(RetrieveFields(entity.Read<UserHeartCount>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserHeartCount>()));
        }
        if (entity.Has<UserMapZoneElement>())
        {
            componentData.AppendLine("  UserMapZoneElement");
            var buffer = ReadBuffer<UserMapZoneElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserMapZoneElement>()));
        }
        if (entity.Has<UserMapZonePackedRevealElement>())
        {
            componentData.AppendLine("  UserMapZonePackedRevealElement");
            var buffer = ReadBuffer<UserMapZonePackedRevealElement>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserMapZonePackedRevealElement>()));
        }
        if (entity.Has<UserNetBuffer>())
        {             
            componentData.AppendLine("  UserNetBuffer");
            var buffer = ReadBuffer<UserNetBuffer>(entity);
            componentData.AppendLine("    Length: " + buffer.Length);
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserNetBuffer>()));
        }
        if (entity.Has<UserOwner>())
        {
            componentData.AppendLine("  UserOwner");
            componentData.AppendLine(RetrieveFields(entity.Read<UserOwner>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UserOwner>()));
		}
		if (entity.Has<UserTeam>())
		{
			componentData.AppendLine("  UserTeam");
			componentData.AppendLine(RetrieveFields(entity.Read<UserTeam>()));
			checkedTypes.Add(new ComponentType(Il2CppType.Of<UserTeam>()));
		}
		if (entity.Has<UsesSpawnTag>())
        {
            componentData.AppendLine("  UsesSpawnTag");
            componentData.AppendLine(RetrieveFields(entity.Read<UsesSpawnTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<UsesSpawnTag>()));
        }
        if (entity.Has<VBloodAbilityBuffEntry>())
        {
            componentData.AppendLine("  VBloodAbilityBuffEntry");
            var buffer = ReadBuffer<VBloodAbilityBuffEntry>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodAbilityBuffEntry>()));
        }
        if (entity.Has<VBloodAbilityOwnerData>())
        {
            componentData.AppendLine("  VBloodAbilityOwnerData");
            componentData.AppendLine(RetrieveFields(entity.Read<VBloodAbilityOwnerData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodAbilityOwnerData>()));
        }
        if (entity.Has<VBloodConsumeSource>())
        {
            componentData.AppendLine("  VBloodConsumeSource");
            componentData.AppendLine(RetrieveFields(entity.Read<VBloodConsumeSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodConsumeSource>()));
        }
        if (entity.Has<VBloodProgressionUnlockData>())
        {
            componentData.AppendLine("  VBloodProgressionUnlockData");
            componentData.AppendLine(RetrieveFields(entity.Read<VBloodProgressionUnlockData>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodProgressionUnlockData>()));
        }
        if (entity.Has<VBloodRewardSequence>())
        {
            componentData.AppendLine("  VBloodRewardSequence");
            componentData.AppendLine(RetrieveFields(entity.Read<VBloodRewardSequence>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodRewardSequence>()));
        }
        if (entity.Has<VBloodUnit>())
        {
            componentData.AppendLine("  VBloodUnit");
            componentData.AppendLine(RetrieveFields(entity.Read<VBloodUnit>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodUnit>()));
        }
        if (entity.Has<VBloodUnlockTechBuffer>())
        {
            componentData.AppendLine("  VBloodUnlockTechBuffer");
            var buffer = ReadBuffer<VBloodUnlockTechBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VBloodUnlockTechBuffer>()));
        }
        if (entity.Has<VampireTag>())
        {
            componentData.AppendLine("  VampireTag");
            componentData.AppendLine(RetrieveFields(entity.Read<VampireTag>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VampireTag>()));
        }
        if (entity.Has<ProjectM.Velocity>())
        {
            componentData.AppendLine("  Velocity");
            componentData.AppendLine(RetrieveFields(entity.Read<ProjectM.Velocity>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<ProjectM.Velocity>()));
        }
        if (entity.Has<VisibleFromFlight>())
        {
            componentData.AppendLine("  VisibleFromFlight");
            componentData.AppendLine(RetrieveFields(entity.Read<VisibleFromFlight>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<VisibleFromFlight>()));
        }
        if (entity.Has<Vision>())
        {
            componentData.AppendLine("  Vision");
            componentData.AppendLine(RetrieveFields(entity.Read<Vision>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Vision>()));
        }
        if (entity.Has<WalkBackAndForth>())
        {
            componentData.AppendLine("  WalkBackAndForth");
            componentData.AppendLine(RetrieveFields(entity.Read<WalkBackAndForth>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WalkBackAndForth>()));
        }
        if (entity.Has<WallRoofOrnament>())
        {
            componentData.AppendLine("  WallRoofOrnament");
            componentData.AppendLine(RetrieveFields(entity.Read<WallRoofOrnament>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WallRoofOrnament>()));
        }
        if (entity.Has<WallpaperSet>())
        {
            componentData.AppendLine("  WallpaperSet");
            componentData.AppendLine(RetrieveFields(entity.Read<WallpaperSet>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WallpaperSet>()));
        }
        if (entity.Has<WallpaperStyles>())
        {
            componentData.AppendLine("  WallpaperStyles");
            componentData.AppendLine(RetrieveFields(entity.Read<WallpaperStyles>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WallpaperStyles>()));
        }
        if (entity.Has<Wallpaper_FourSplits>())
        {
            componentData.AppendLine("  Wallpaper_FourSplits");
            componentData.AppendLine(RetrieveFields(entity.Read<Wallpaper_FourSplits>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Wallpaper_FourSplits>()));
        }
        if (entity.Has<Wallpaper_NotSplit>())
        {
            componentData.AppendLine("  Wallpaper_NotSplit");
            componentData.AppendLine(RetrieveFields(entity.Read<Wallpaper_NotSplit>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Wallpaper_NotSplit>()));
        }
        if (entity.Has<Wallpaper_TwoSplits>())
        {
            componentData.AppendLine("  Wallpaper_TwoSplits");
            componentData.AppendLine(RetrieveFields(entity.Read<Wallpaper_TwoSplits>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<Wallpaper_TwoSplits>()));
        }
        if (entity.Has<WaypointMapIcon>())
        {
            componentData.AppendLine("  WaypointMapIcon");
            componentData.AppendLine(RetrieveFields(entity.Read<WaypointMapIcon>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WaypointMapIcon>()));
        }
        if (entity.Has<WeakenBuff>())
        {
            componentData.AppendLine("  WeakenBuff");
            componentData.AppendLine(RetrieveFields(entity.Read<WeakenBuff>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WeakenBuff>()));
        }
        if (entity.Has<WeaponLevel>())
        {
            componentData.AppendLine("  WeaponLevel");
            componentData.AppendLine(RetrieveFields(entity.Read<WeaponLevel>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WeaponLevel>()));
        }
        if (entity.Has<WeaponLevelSource>())
        {
            componentData.AppendLine("  WeaponLevelSource");
            componentData.AppendLine(RetrieveFields(entity.Read<WeaponLevelSource>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WeaponLevelSource>()));
        }
        if (entity.Has<WorkstationAssignedServant>())
        {
            componentData.AppendLine("  WorkstationAssignedServant");
            componentData.AppendLine(RetrieveFields(entity.Read<WorkstationAssignedServant>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WorkstationAssignedServant>()));
        }
        if (entity.Has<WorkstationRecipesBuffer>())
        {
            componentData.AppendLine("  WorkstationRecipesBuffer");
            var buffer = ReadBuffer<WorkstationRecipesBuffer>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WorkstationRecipesBuffer>()));
        }
        if (entity.Has<WorldZoneId>())
        {
            componentData.AppendLine("  WorldZoneId");
            componentData.AppendLine(RetrieveFields(entity.Read<WorldZoneId>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WorldZoneId>()));
        }
        if (entity.Has<WoundedConstants>())
        {
            componentData.AppendLine("  WoundedConstants");
            componentData.AppendLine(RetrieveFields(entity.Read<WoundedConstants>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<WoundedConstants>()));
        }
        if (entity.Has<YieldEssenceOnDeath>())
        {
            componentData.AppendLine("  YieldEssenceOnDeath");
            componentData.AppendLine(RetrieveFields(entity.Read<YieldEssenceOnDeath>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<YieldEssenceOnDeath>()));
        }
        if (entity.Has<YieldResourcesOnDamageTaken>())
        {
            componentData.AppendLine("  YieldResourcesOnDamageTaken");
            var buffer = ReadBuffer<YieldResourcesOnDamageTaken>(entity);
            for (int i = 0; i < buffer.Length; i++)
            {
                componentData.AppendLine($"   [{i}]");
                componentData.AppendLine(RetrieveFields(buffer[i]));
            }
            checkedTypes.Add(new ComponentType(Il2CppType.Of<YieldResourcesOnDamageTaken>()));
        }
        if (entity.Has<YieldResourcesOnPickup>())
        {
            componentData.AppendLine("  YieldResourcesOnPickup");
            componentData.AppendLine(RetrieveFields(entity.Read<YieldResourcesOnPickup>()));
            checkedTypes.Add(new ComponentType(Il2CppType.Of<YieldResourcesOnPickup>()));
        }

        foreach (var type in allTypes)
        {
            if (!checkedTypes.Contains(type))
            {
                componentData.AppendLine($"Unhandled Component Type: {type}");
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
				fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.ModifiableId.ToString()} and priority {v.Priority}");
			}
			else if (field.FieldType == typeof(ModificationData<Int32>))
			{
				var v = (ModificationData<Int32>)field.GetValue(component);
				fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.ModifiableId.ToString()} and priority {v.Priority}");
			}
			else if (field.FieldType == typeof(ModificationData<Int64>))
			{
				var v = (ModificationData<Int64>)field.GetValue(component);
				fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.ModifiableId.ToString()} and priority {v.Priority}");
			}
			else if (field.FieldType == typeof(ModificationData<bool>))
			{
				var v = (ModificationData<bool>)field.GetValue(component);
				fields.AppendLine(prepend + $"{field.Name}: {v.Source} is {v.ModType.ToString()} with value {v.ModValue} to {v.ModifiableId.ToString()} and priority {v.Priority}");
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
							fields.Append(RetrieveFields<ConditionInfo>(v.Info, prepend + "  "));
							fields.AppendLine(prepend + " ConditionalElements");
							for (int i = 0; i < v.Conditionals.Length; ++i)
							{
								ConditionElement e = v.Conditionals[i];
								fields.AppendLine(prepend + $"  [{i}] Source: {e.Source} Success: {e.SuccessIndex} Failure: {e.FailureIndex} Union: {e.Union}  {e}");
							}
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
					fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {entityString}");
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
					fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {networkEntityString}");
				}
				else
				{
					fields.AppendLine(prepend + $"{field.Name}: {networkEntityString}");
				}
			}
			else if (field.FieldType == typeof(ProjectM.ModifiableEntity))
			{
				var v = ((ModifiableEntity)field.GetValue(component)).Value;
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
					fields.AppendLine(prepend + $"{field.Name}: Prefab {v.Read<PrefabGUID>().LookupName()} - {entityString}");
				}
				else
				{
					fields.AppendLine(prepend + $"{field.Name}: {entityString}");
				}
			}
			else
			{
				fields.AppendLine(prepend + $"{field.Name}: Unhandled {field.GetValue(component).ToString()}");
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
