using System;
using System.Collections.Generic;
using Unity.Entities;


namespace KindredExtract.Models;


public enum EcsSystemCategory
{
    Group, // is a ComponentSystemGroup
    Base, // is a ComponentSystemBase, but not a group
    Unmanaged, // implements ISystem
    Unknown, // we don't know the type of system
}

public class EcsSystemTreeNode(EcsSystemCategory category, SystemHandle systemHandle, Type type = null, ComponentSystemBase instance = null)
{
    public EcsSystemCategory Category = category;
    public Type Type = type;
    public SystemHandle SystemHandle = systemHandle;
    public ComponentSystemBase Instance = instance;
    public IList<EcsSystemTreeNode> ChildrenOrderedForUpdate = new List<EcsSystemTreeNode>();
    public IList<EcsSystemTreeNode> Parents = new List<EcsSystemTreeNode>();
}


public class EcsSystemHierarchy
{
    public World World;
    public EcsSystemCounts Counts;
    public KnownUnknowns KnownUnknowns;
    public IList<EcsSystemTreeNode> RootNodesUnordered;

    public IList<EcsSystemTreeNode> FindNodesWithMultipleParents()
    {
        var foundNodes = new List<EcsSystemTreeNode>();
        foreach (var node in RootNodesUnordered)
        {
            FindNodesWithMultipleParents(node, foundNodes);
        }
        return foundNodes;
    }

    private void FindNodesWithMultipleParents(EcsSystemTreeNode node, IList<EcsSystemTreeNode> foundNodes)
    {
        if (node.Parents.Count > 1)
        {
            foundNodes.Add(node);
        }
        foreach (var childNode in node.ChildrenOrderedForUpdate)
        {
            FindNodesWithMultipleParents(childNode, foundNodes);
        }
    }
}

public class EcsSystemCounts
{
    public int Group = 0;
    public int Base = 0;
    public int Unmanaged = 0;
    public int Unknown = 0;

    public int Sum()
    {
        return Group + Base + Unmanaged + Unknown;
    }
}

public class KnownUnknowns
{
    public ISet<Type> ContainsGenericParameters = new HashSet<Type>();

    public bool AreKnown()
    {
        return ContainsGenericParameters.Count > 0;
    }
}