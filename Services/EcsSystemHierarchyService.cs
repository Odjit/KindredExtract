using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using KindredExtract.Models;
using Unity.Collections;
using Unity.Entities;

namespace KindredExtract.Services;

// responsible for building a systems update hierarchy for any given world
internal class EcsSystemHierarchyService
{
    private EcsSystemFinderService _systemFinderService;
    private ManualLogSource Log;

    public EcsSystemHierarchyService(EcsSystemFinderService systemFinderService, ManualLogSource log)
    {
        _systemFinderService = systemFinderService;
        Log = log;
    }

    public EcsSystemHierarchy BuildSystemHiearchyForWorld(World world)
    {
        var knownUnknowns = new KnownUnknowns();
        var systemGroupInstances = _systemFinderService.FindSystemGroupInstances(world, knownUnknowns);
        var systemBaseInstances = _systemFinderService.FindSystemBaseInstances(world, knownUnknowns);

        var managedSystemsDict = new Dictionary<SystemHandle, (Type, ComponentSystemBase)>();
        foreach (var (systemGroupType, systemGroup) in systemGroupInstances)
        {
            // ComponentSystemGroup is a subclass of ComponentSystemBase, so add them first (more specific)
            managedSystemsDict.TryAdd(systemGroup.SystemHandle, (systemGroupType, systemGroup));
        }
        foreach (var (systemBaseType, systemBase) in systemBaseInstances)
        {
            // add all other instances of ComponentSystemBase whose intitial types we're aware of
            managedSystemsDict.TryAdd(systemBase.SystemHandle, (systemBaseType, systemBase));
        }

        var unmanagedSystemHandles = _systemFinderService.FindUnmanagedSystemHandles(world);
        var unmanagedSystemsDict = new Dictionary<SystemHandle, Type>();
        foreach (var (unmanagedSystemType, systemHandle) in unmanagedSystemHandles)
        {
            unmanagedSystemsDict.TryAdd(systemHandle, unmanagedSystemType);
        }

        return BuildSystemsHierarchy(world, systemGroupInstances, managedSystemsDict, unmanagedSystemsDict, knownUnknowns);
    }

    private EcsSystemHierarchy BuildSystemsHierarchy(
        World world,
        IList<(Type, ComponentSystemGroup)> systemGroupInstances,
        Dictionary<SystemHandle, (Type, ComponentSystemBase)> managedSystemsDict,
        Dictionary<SystemHandle, Type> unmanagedSystemsDict,
        KnownUnknowns knownUnknowns
    )
    {
        var counts = new EcsSystemCounts()
        {
            Group = systemGroupInstances.Count,
            Base = managedSystemsDict.Count,
            Unmanaged = unmanagedSystemsDict.Count,
            Unknown = 0,
        };
        var allNodesDict = new Dictionary<SystemHandle, EcsSystemTreeNode>();

        // first pass: ensure all groups have a node in the dict
        foreach (var (systemGroupType, systemGroup) in systemGroupInstances)
        {
            var groupNode = new EcsSystemTreeNode(
                category: EcsSystemCategory.Group,
                systemHandle: systemGroup.SystemHandle,
                type: systemGroupType,
                instance: systemGroup
            );
            if (!allNodesDict.ContainsKey(systemGroup.SystemHandle))
            {
                allNodesDict.Add(systemGroup.SystemHandle, groupNode);
            }
        }

        // second pass: add edges for group nodes, and initialize non-group nodes in Dict
        foreach (var (systemGroupType, systemGroup) in systemGroupInstances)
        {
            var parentNode = allNodesDict[systemGroup.SystemHandle];

            var orderedSubsystems = systemGroup.GetAllSystems();
            foreach (var subsystemHandle in orderedSubsystems)
            {
                EcsSystemCategory subsystemCategory = EcsSystemCategory.Unknown;
                Type subsystemType = null;
                ComponentSystemBase subsystemInstance = null;

                if (managedSystemsDict.ContainsKey(subsystemHandle))
                {
                    var (bestKnownType, subsystem) = managedSystemsDict[subsystemHandle];
                    subsystemCategory = (subsystem is ComponentSystemGroup) ? EcsSystemCategory.Group : EcsSystemCategory.Base;
                    subsystemType = bestKnownType;
                    subsystemInstance = subsystem;
                }
                else if (unmanagedSystemsDict.ContainsKey(subsystemHandle))
                {
                    subsystemCategory = EcsSystemCategory.Unmanaged;
                    subsystemType = unmanagedSystemsDict[subsystemHandle];
                }
                else
                {
                    counts.Unknown++;
                }

                EcsSystemTreeNode childNode;
                if (allNodesDict.ContainsKey(subsystemHandle))
                {
                    childNode = allNodesDict[subsystemHandle];
                }
                else
                {
                    childNode = new EcsSystemTreeNode(
                        category: subsystemCategory,
                        systemHandle: subsystemHandle,
                        type: subsystemType,
                        instance: subsystemInstance
                    );
                    allNodesDict.Add(subsystemHandle, childNode);
                }
                parentNode.ChildrenOrderedForUpdate.Add(childNode);
                if (childNode.Parents.Count > 0)
                {
                    Log.LogError($"Uh oh, a system belongs to multiple groups. This should not happen: {childNode.Type}");
                }
                childNode.Parents.Add(parentNode);
            }
        }

        // find root groups
        var ungroupedNodes = new HashSet<EcsSystemTreeNode>();
        foreach (var (systemGroupType, systemGroup) in systemGroupInstances)
        {
            var node = allNodesDict[systemGroup.SystemHandle];
            if (node.Parents.Count == 0)
            {
                ungroupedNodes.Add(node);
            }
        }

        // now add all other ungrouped systems
        foreach (var systemHandle in world.Unmanaged.GetAllSystems(Allocator.Temp))
        {
            if (allNodesDict.ContainsKey(systemHandle))
            {
                continue;
            }
            EcsSystemCategory systemCategory = EcsSystemCategory.Unknown;
            Type systemType = null;
            ComponentSystemBase systemInstance = null;

            if (managedSystemsDict.ContainsKey(systemHandle))
            {
                var (bestKnownType, system) = managedSystemsDict[systemHandle];
                systemCategory = (system is ComponentSystemGroup) ? EcsSystemCategory.Group : EcsSystemCategory.Base;
                systemType = bestKnownType;
                systemInstance = system;
            }
            else if (unmanagedSystemsDict.ContainsKey(systemHandle))
            {
                systemCategory = EcsSystemCategory.Unmanaged;
                systemType = unmanagedSystemsDict[systemHandle];
            }
            else
            {
                counts.Unknown++;
            }

            var node = new EcsSystemTreeNode(
                category: systemCategory,
                systemHandle: systemHandle,
                type: systemType,
                instance: systemInstance
            );
            allNodesDict.Add(systemHandle, node);
            ungroupedNodes.Add(node);
        }

        return new EcsSystemHierarchy()
        {
            World = world,
            Counts = counts,
            KnownUnknowns = knownUnknowns,
            RootNodesUnordered = ungroupedNodes.ToList(),
        };
    }

}
