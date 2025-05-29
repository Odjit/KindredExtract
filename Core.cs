using BepInEx.Logging;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Engine.Console;
using Il2CppInterop.Runtime.Injection;
using KindredExtract.Services;
using ProjectM;
using ProjectM.Network;
using ProjectM.Physics;
using ProjectM.Scripting;
using ProjectM.UI;
using Stunlock.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;
using VampireCommandFramework;
using VampireCommandFramework.Breadstone;
using Object = UnityEngine.Object;

namespace KindredExtract;

internal static class Core
{
	public static World TheWorld { get; } = GetWorld("Server") ?? (GetWorld("Client_0") ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?"));

    public static bool IsServer => TheWorld.Name == "Server";
	public static EntityManager EntityManager { get; } = TheWorld.EntityManager;
    public static ServerScriptMapper ServerScriptMapper { get; internal set; }
    public static double ServerTime => ServerGameManager.ServerTime;
    public static ServerGameManager ServerGameManager => ServerScriptMapper.GetServerGameManager();
    public static ConsoleCommandSystem ConsoleCommandSystem => TheWorld.GetExistingSystemManaged<ConsoleCommandSystem>();
    public static RefinementstationMenuMapper RefinementstationMenuMapper => TheWorld.GetExistingSystemManaged<RefinementstationMenuMapper>();

    public static LocalizationService Localization { get; } = new();
	public static ManualLogSource Log { get; } = Plugin.LogInstance;
	public static PlayerService Players { get; internal set; }

	public static PrefabService Prefabs { get; internal set; }

    public static EcsSystemTypesService EcsSystemTypesService { get; } = new(Log);
    public static EcsSystemFinderService EcsSystemFinderService { get; } = new(EcsSystemTypesService, Log);
    public static EcsSystemHierarchyService EcsSystemHierarchyService { get; } = new(EcsSystemFinderService, Log);



    static MonoBehaviour monoBehaviour;

    public static void LogException(System.Exception e, [CallerMemberName] string caller = null)
	{
		Core.Log.LogError($"Failure in {caller}\nMessage: {e.Message} Inner:{e.InnerException?.Message}\n\nStack: {e.StackTrace}\nInner Stack: {e.InnerException?.StackTrace}");
	}


	internal static void InitializeAfterLoaded()
	{
		if (_hasInitialized) return;

        ServerScriptMapper = TheWorld.GetExistingSystemManaged<ServerScriptMapper>();

        Players = new();
		Prefabs = new();


        ComponentInitializer.InitializeComponents();

        _hasInitialized = true;
		Log.LogInfo($"{nameof(InitializeAfterLoaded)} completed");
    }

	private static bool _hasInitialized = false;

    public static (PrefabGUID, int)[] CountPrefabs(int maxNum=10, string filter=null)
    {
        Dictionary<PrefabGUID, int> prefabCount = new();
        foreach (var entity in EntityManager.GetAllEntities())
        {
            if (!EntityManager.HasComponent<PrefabGUID>(entity)) continue;
            
            var prefab = EntityManager.GetComponentData<PrefabGUID>(entity);

            if (filter != null && !prefab.LookupName().Contains(filter)) continue;

            if (!prefabCount.ContainsKey(prefab)) prefabCount.Add(prefab, 0);

            prefabCount[prefab]++;
        }
        return prefabCount.OrderByDescending(x => x.Value)
            .Take(maxNum)
            .Select(x => (x.Key, x.Value)).ToArray();
    }

    public static void SavePrefabCountToCSV()
    {
        Dictionary<string, int> nonPrefabCount = new();
        Dictionary<PrefabGUID, int> prefabCount = new();
        var total = 0;
        var totalNonPrefab = 0;
        foreach (var entity in EntityManager.GetAllEntities())
        {
            if (!EntityManager.HasComponent<PrefabGUID>(entity))
            {
                var componentString = entity.GetComponentString();
                if (!nonPrefabCount.ContainsKey(componentString)) nonPrefabCount.Add(componentString, 0);
                nonPrefabCount[componentString]++;
                totalNonPrefab++;

                if (componentString == "Simulate")
                    Core.EntityManager.DestroyEntity(entity);
                continue;
            }

            var prefab = EntityManager.GetComponentData<PrefabGUID>(entity);
            if (!prefabCount.ContainsKey(prefab)) prefabCount.Add(prefab, 0);

            prefabCount[prefab]++;
            total++;
        }

        Log.LogInfo($"Total Prefab Count: {total}");
        Log.LogInfo($"Total Non Prefab Count: {totalNonPrefab}");

        var sortedPrefabCount = prefabCount.ToList();
        sortedPrefabCount.Sort((x, y) => x.Key.LookupName().CompareTo(y.Key.LookupName()));
        using (var writer = new StreamWriter("prefab_count.csv"))
        {
            writer.WriteLine("PrefabGUID,Count");
            foreach (var kvp in sortedPrefabCount)
            {
                writer.WriteLine($"{kvp.Key.LookupName()},{kvp.Value}");
            }
        }

        var sortedNonPrefabCount = nonPrefabCount.ToList();
        sortedNonPrefabCount.Sort((x, y) => x.Key.CompareTo(y.Key));
        using (var writer = new StreamWriter("non_prefab_count.csv"))
        {
            writer.WriteLine("Components,Count");
            foreach (var kvp in sortedNonPrefabCount)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }
    }

    class TestCommand : Il2CppSystem.Object
    {
        public TestCommand(IntPtr ptr) : base(ptr) { }
        public TestCommand() { }

        public void Test()
        {
            Core.Log.LogInfo("Test command executed!");
        }
    }

    class ConsoleTest : ConsoleCommand
    {

        public ConsoleTest(string command) : base(command, 0)
        {
            Core.Log.LogInfo("Created!");
        }

        public override void ExecuteCommand(string command, string fullCommand)
        {
            Core.Log.LogInfo("ConsoleTest command executed!");
        }
    }

    public static void RegisterCommandsForConsole()
    {
        Core.Log.LogInfo("Registering commands!!!");
        var ccs = Core.TheWorld.GetExistingSystemManaged<ConsoleCommandSystem>();

        var commandType = typeof(ConsoleTest);
        if (!ClassInjector.IsTypeRegisteredInIl2Cpp(commandType))
        {
            ClassInjector.RegisterTypeInIl2Cpp(commandType);
        }

        ccs.AddCommand(new ConsoleTest("test"));
        return;

        var types = Assembly.GetAssembly(typeof(Core)).GetTypes();
        foreach(var t in types)
        {
            foreach(var method in t.GetMethods())
            {
                var commandAttr = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttr != null)
                {
                    Core.Log.LogInfo($"Registering command {commandAttr.Name}");
                    void ExecuteCommand()
                    {
                        try
                        {
                            var ctx = new ChatCommandContext(GetVChatEvent(commandAttr.Name));

                            // Prepare parameters
                            var parameters = method.GetParameters();
                            var args = new object[parameters.Length];

                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (parameters[i].ParameterType == typeof(ChatCommandContext))
                                {
                                    args[i] = ctx;
                                }
                                else if (parameters[i].HasDefaultValue)
                                {
                                    args[i] = parameters[i].DefaultValue;
                                }
                                else
                                {
                                    throw new InvalidOperationException($"Cannot provide a value for parameter '{parameters[i].Name}' of method '{method.Name}'");
                                }
                            }

                            method.Invoke(null, args);
                        }
                        catch (System.Exception e)
                        {
                            Core.LogException(e);
                        }
                    }

                    var methodPtr = Marshal.GetFunctionPointerForDelegate(ExecuteCommand);

                    ccs.RegisterCommand(commandAttr.Name, commandAttr.Description, new (methodPtr));
                }
            }
        }
    }

    static VChatEvent GetVChatEvent(string message)
    {
        var userEntity = Helper.GetEntitiesByComponentType<User>()[0];
        var characterEntity = Helper.GetEntitiesByComponentType<PlayerCharacter>()[0];

        var constructor = typeof(VChatEvent).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new Type[] { typeof(Entity), typeof(Entity), typeof(string), typeof(ChatMessageType), typeof(User) },
            null);

        var type = ChatMessageType.Local; // Replace with actual chat message type
        var user = userEntity.Read<User>(); // Adjust according to your actual method to get User

        // Create an instance of VChatEvent using the constructor
        var vChatEventInstance = constructor.Invoke(new object[] { userEntity, characterEntity, message, type, user });

        return vChatEventInstance as VChatEvent;
    }

    private static World GetWorld(string name)
	{
		foreach (var world in World.s_AllWorlds)
		{
			if (world.Name == name)
			{
				return world;
			}
		}

		return null;
    }

    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        if (monoBehaviour == null)
        {
            var go = new GameObject("KindredExtract");
            monoBehaviour = go.AddComponent<IgnorePhysicsDebugSystem>();
            Object.DontDestroyOnLoad(go);
        }

        return monoBehaviour.StartCoroutine(routine.WrapToIl2Cpp());
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        if (monoBehaviour == null)
        {
            return;
        }

        monoBehaviour.StopCoroutine(coroutine);
    }
}
