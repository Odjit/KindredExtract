using Il2CppInterop.Runtime;
using ProjectM;
using Stunlock.Core;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Entities;

namespace KindredExtract;

//#pragma warning disable CS8500
public static class ECSExtensions
{
	public unsafe static void Write<T>(this Entity entity, T componentData) where T : struct
	{
		// Get the ComponentType for T
		var ct = new ComponentType(Il2CppType.Of<T>());

		// Marshal the component data to a byte array
		byte[] byteArray = StructureToByteArray(componentData);

		// Get the size of T
		int size = Marshal.SizeOf<T>();

		// Create a pointer to the byte array
		fixed (byte* p = byteArray)
		{
			// Set the component data
			Core.TheWorld.EntityManager.SetComponentDataRaw(entity, ct.TypeIndex, p, size);
		}
	}

	// Helper function to marshal a struct to a byte array
	public static byte[] StructureToByteArray<T>(T structure) where T : struct
	{
		int size = Marshal.SizeOf(structure);
		byte[] byteArray = new byte[size];
		IntPtr ptr = Marshal.AllocHGlobal(size);

		Marshal.StructureToPtr(structure, ptr, true);
		Marshal.Copy(ptr, byteArray, 0, size);
		Marshal.FreeHGlobal(ptr);

		return byteArray;
	}

	public unsafe static T Read<T>(this Entity entity) where T : struct
	{
		// Get the ComponentType for T
		var ct = new ComponentType(Il2CppType.Of<T>());

		if(ct.IsZeroSized)
			return new T();

		// Get a pointer to the raw component data
		void* rawPointer = Core.TheWorld.EntityManager.GetComponentDataRawRO(entity, ct.TypeIndex);

        // Marshal the raw data to a T struct
        T componentData = Marshal.PtrToStructure<T>(new IntPtr(rawPointer));

		return componentData;
	}

	public static bool Has<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
		return Core.TheWorld.EntityManager.HasComponent(entity, ct);
	}

	public static string LookupName(this PrefabGUID prefabGuid)
	{
		var prefabCollectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();
		return (prefabCollectionSystem._PrefabLookupMap.GuidToEntityMap.ContainsKey(prefabGuid)
			? prefabCollectionSystem._PrefabLookupMap.GetName(prefabGuid) + " PrefabGuid(" + prefabGuid.GuidHash + ")" : "GUID Not Found");
	}

	public static void Add<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
		Core.TheWorld.EntityManager.AddComponent(entity, ct);
	}

	public static void Remove<T>(this Entity entity)
	{
		var ct = new ComponentType(Il2CppType.Of<T>());
		Core.TheWorld.EntityManager.RemoveComponent(entity, ct);
	}

	public static string GetComponentString(this Entity entity)
	{
        var componentTypes = Core.TheWorld.EntityManager.GetComponentTypes(entity);
		var componentNames = componentTypes.ToArray().Select(type => TypeManager.GetType(type.TypeIndex).Name);
		componentTypes.Dispose();

        return String.Join("+", componentNames);
    }
}
//#pragma warning restore CS8500
