using System.Collections.Generic;
using ProjectM;
using Stunlock.Core;

namespace KindredExtract.Services;

internal class PrefabService
{
	internal Dictionary<string, (string Name, PrefabGUID Prefab)> NameToGuid { get; init; } = new();

	internal PrefabService()
	{
		var collectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();

		var spawnable = collectionSystem.SpawnableNameToPrefabGuidDictionary;
		Core.Log.LogDebug($"Spawnable prefabs: {spawnable.Count}");
		foreach (var kvp in spawnable)
		{
			bool success = NameToGuid.TryAdd(kvp.Key.ToLowerInvariant(), (kvp.Key, kvp.Value));
			if (!success)
			{
				Core.Log.LogDebug($"{kvp.Key} exist already, skipping.");
			}
		}
	}

	internal bool TryGetItem(string input, out PrefabGUID prefab)
	{
		var lower = input.ToLowerInvariant();
		var output = NameToGuid.TryGetValue(lower, out var guidRec) || NameToGuid.TryGetValue($"item_{lower}", out guidRec);
		prefab = guidRec.Prefab;
		return output;
	}
}
