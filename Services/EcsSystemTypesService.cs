using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Logging;
using Unity.Entities;

namespace KindredExtract.Services;

// responsible for identifying system Types from the loaded assemblies
internal class EcsSystemTypesService
{
    private ManualLogSource Log;
    private bool _initialized;
    private ISet<Type> _systemGroupTypes = new HashSet<Type>();
    private ISet<Type> _systemBaseTypes = new HashSet<Type>();
    private ISet<Type> _potentialUnmanagedSystemTypes = new HashSet<Type>();

    public EcsSystemTypesService(ManualLogSource log)
    {
        Log = log;
    }

    public ISet<Type> GetSystemGroupTypes()
    {
        InitIfNotInitd();
        return _systemGroupTypes;
    }

    public ISet<Type> GetSystemBaseTypes()
    {
        InitIfNotInitd();
        return _systemBaseTypes;
    }

    public ISet<Type> GetPotentialUnmanagedSystemTypes()
    {
        InitIfNotInitd();
        return _potentialUnmanagedSystemTypes;
    }

    private void InitIfNotInitd()
    {
        if (_initialized)
        {
            return;
        }
        Log.LogDebug($"Locating ECS System types");
        ISet<String> seenTypeNames = new HashSet<String>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assemblyCount = assemblies.Length;
        var counter = 0;
        foreach (var assembly in assemblies)
        {
            Log.LogDebug($"scanning assembly {++counter} of {assemblyCount}");

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Log.LogDebug(ex);
                types = ex.Types;
            }
            catch (Exception ex)
            {
                Log.LogWarning(ex);
                continue;
            }

            foreach (var type in types)
            {
                if (type is null)
                {
                    // can happen if there was a ReflectionTypeLoadException
                    continue;
                }

                if (seenTypeNames.Contains(type.FullName))
                {
                    Log.LogDebug($"Skipped duplicate Type found for {type.FullName}");
                    continue;
                }
                seenTypeNames.Add(type.FullName);

                try
                {
                    if (type.IsSubclassOf(typeof(ComponentSystemGroup)))
                    {
                        _systemGroupTypes.Add(type);
                    }
                    else if (type.IsSubclassOf(typeof(ComponentSystemBase)))
                    {
                        _systemBaseTypes.Add(type);
                    }
                    else if (IsProbablyISystem(type))
                    {
                        _potentialUnmanagedSystemTypes.Add(type);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogWarning(ex);
                }
            }

        }

        _initialized = true;
    }

    internal static bool IsProbablyISystem(Type type)
    {
        // note: the generated interops don't mark structs with ISystem which is probably a bug in the Il2CppInterop library
        // (the metadata does contain that information during generation)
        /*
        if (type.IsAssignableFrom(typeof(Unity.Entities.ISystem)))
        {
            return true;
        }
        */

        if (!type.IsValueType)
        {
            return false;
        }
        if (type.GetMethod("OnCreate") is null)
        {
            return false;
        }
        if (type.GetMethod("OnDestroy") is null)
        {
            return false;
        }
        if (type.GetMethod("OnUpdate") is null)
        {
            return false;
        }
        // we could check the method signatures in detail, but this is good enough
        return true;
    }

}
