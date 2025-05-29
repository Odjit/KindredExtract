using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Logging;
using Il2CppInterop.Runtime;
using KindredExtract.Models;
using Unity.Entities;

namespace KindredExtract.Services;

// responsible for finding existing systems in worlds
internal class EcsSystemFinderService
{
    private EcsSystemTypesService _typeService;
    private ManualLogSource Log;

    public EcsSystemFinderService(EcsSystemTypesService typeService, ManualLogSource log)
    {
        _typeService = typeService;
        Log = log;
    }

    public List<(Type, ComponentSystemGroup)> FindSystemGroupInstances(World world, KnownUnknowns knownUnknowns)
    {
        var instances = new List<(Type, ComponentSystemGroup)>();
        var notFoundTypes = new List<Type>();
        var seenHandles = new HashSet<SystemHandle>();
        foreach (var systemGroupType in _typeService.GetSystemGroupTypes())
        {
            try
            {
                if (systemGroupType.GetTypeInfo().ContainsGenericParameters)
                {
                    // System.InvalidOperationException: Late bound operations cannot be performed on fields with types for which Type.ContainsGenericParameters is true.
                    knownUnknowns.ContainsGenericParameters.Add(systemGroupType);
                    continue;
                }

                var systemInstance = world.GetExistingSystemManaged(Il2CppType.From(systemGroupType));
                if (systemInstance is null)
                {
                    notFoundTypes.Add(systemGroupType);
                    continue;
                }

                if (seenHandles.Contains(systemInstance.SystemHandle))
                {
                    // This happens and I have no idea how
                    // In the server world this only happens for one thing: Unity.Entities.SimulationSystemGroup
                    Log.LogWarning($"Skipped double-adding system for {systemGroupType}");
                    continue;
                }
                seenHandles.Add(systemInstance.SystemHandle);

                var systemGroupInstance = systemInstance.Cast<ComponentSystemGroup>();

                // use a tuple to remember the original type
                // or we could somehow cast it in here. not sure how to do it with a dynamic type though.
                instances.Add((systemGroupType, systemGroupInstance));
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Failure finding instance of {systemGroupType}: {ex}");
            }
        }
        Log.LogDebug($"Found {instances.Count} ComponentSystemGroup instances in world '{world.Name}'. The other {notFoundTypes.Count} are not being used by this world.");
        return instances;
    }

    public List<(Type, ComponentSystemBase)> FindSystemBaseInstances(World world, KnownUnknowns knownUnknowns)
    {
        var instances = new List<(Type, ComponentSystemBase)>();
        var notFoundTypes = new List<Type>();
        foreach (var systemBaseType in _typeService.GetSystemBaseTypes())
        {
            try
            {
                if (systemBaseType.GetTypeInfo().ContainsGenericParameters)
                {
                    // System.InvalidOperationException: Late bound operations cannot be performed on fields with types for which Type.ContainsGenericParameters is true.
                    knownUnknowns.ContainsGenericParameters.Add(systemBaseType);
                    continue;
                }

                var systemInstance = world.GetExistingSystemManaged(Il2CppType.From(systemBaseType));
                if (systemInstance is null)
                {
                    notFoundTypes.Add(systemBaseType);
                    continue;
                }
                var systemBaseInstance = systemInstance.Cast<ComponentSystemBase>();

                // use a tuple to remember the original type
                // or we could somehow cast it in here. not sure how to do it with a dynamic type though.
                instances.Add((systemBaseType, systemBaseInstance));
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Error finding instance of {systemBaseType}: {ex}");
            }
        }
        Log.LogDebug($"Found {instances.Count} CommponentSystemBase instances in world '{world.Name}'. The other {notFoundTypes.Count} are not being used by this world.");
        return instances;
    }

    public List<(Type, SystemHandle)> FindUnmanagedSystemHandles(World world)
    {
        var instances = new List<(Type, SystemHandle)>();
        var notFoundTypes = new List<Type>();
        foreach (var systemType in _typeService.GetPotentialUnmanagedSystemTypes())
        {
            try
            {
                var systemHandle = world.GetExistingSystem(Il2CppType.From(systemType));
                if (systemHandle.Equals(SystemHandle.Null))
                {
                    notFoundTypes.Add(systemType);
                    continue;
                }
                instances.Add((systemType, systemHandle));
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Error finding instance of {systemType}: {ex}");
            }
        }
        Log.LogDebug($"Found {instances.Count} ISystem instances in world '{world.Name}'. The other {notFoundTypes.Count} are not being used by this world.");
        return instances;
    }

}