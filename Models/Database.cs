using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BepInEx.Configuration;
using ProjectM.Network;
using Unity.Entities;

namespace KindredExtract.Models;
public readonly struct Database
{
	private readonly ConfigFile CONFIG;

	private static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, "KindredExtract");
	private static readonly string STAFF_PATH = Path.Combine(CONFIG_PATH, "staff.json");
	private static readonly string NOSPAWN_PATH = Path.Combine(CONFIG_PATH, "nospawn.json");

	public Database(ConfigFile config)
	{
		CONFIG = config;
	}

	public readonly void InitConfig()
	{
		string _json;
		Dictionary<string, string> _dic;
		STAFF.Clear();
		NOSPAWN.Clear();

		if (File.Exists(STAFF_PATH))
		{
			_json = File.ReadAllText(STAFF_PATH);
			_dic = JsonSerializer.Deserialize<Dictionary<string, string>>(_json);

			foreach (var kvp in _dic)
			{
				STAFF.Add(kvp.Key, kvp.Value);
			}
		}

		if (File.Exists(NOSPAWN_PATH))
		{
			_json = File.ReadAllText(NOSPAWN_PATH);
			_dic = JsonSerializer.Deserialize<Dictionary<string, string>>(_json);

			foreach (var kvp in _dic)
			{
				NOSPAWN.Add(kvp.Key, kvp.Value);
			}
		}
		else
		{
			NOSPAWN["CHAR_VampireMale"] = "it causes corruption to the save file.";
			NOSPAWN["CHAR_Mount_Horse_Gloomrot"] = "it causes an instant server crash.";
			NOSPAWN["CHAR_Mount_Horse_Vampire"] = "it causes an instant server crash.";
			SaveNoSpawn();
		}
	}

	static void WriteConfig(string path, Dictionary<string, string> dic)
	{
		if (!Directory.Exists(CONFIG_PATH)) Directory.CreateDirectory(CONFIG_PATH);
		if (!File.Exists(path))
		{
			var _json = JsonSerializer.Serialize(dic, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(path, _json);
		}
	}

	static public void SaveStaff()
	{
		WriteConfig(STAFF_PATH, STAFF);
	}
	static public void SaveNoSpawn()
	{
		WriteConfig(NOSPAWN_PATH, NOSPAWN);
	}

	static public void SetStaff(Entity userEntity, string rank)
	{
		var user = userEntity.Read<User>();
		STAFF[user.PlatformId.ToString()] = rank;
		SaveStaff();
		Core.Log.LogWarning($"User {user.CharacterName} added to staff config as {rank}.");
	}
	static public void SetNoSpawn(string prefabName, string reason)
	{
		NOSPAWN[prefabName] = reason;
		SaveNoSpawn();
		Core.Log.LogWarning($"NPC {prefabName} is banned from spawning because {reason}.");
	}

	public static string GetStaff(Entity user)
	{
		Player _player = new(user);
		return (_player == null || !STAFF.ContainsKey(_player.SteamID.ToString()) ? null : STAFF[_player.SteamID.ToString()]);
	}

	static public bool IsSpawnBanned(string prefabName, out string reason)
	{
		return NOSPAWN.TryGetValue(prefabName, out reason);
	}

	private static readonly Dictionary<string, string> STAFF = new()
	{
		{ "SteamID1", "[Rank]" },
		{ "SteamID2", "[Rank]" }
	};

	private static readonly Dictionary<string, string> NOSPAWN = new()
	{
		{ "PrefabGUID", "Reason" }
	};
	public static Dictionary<string, string> GetStaff()
	{
		return STAFF;
	}
	public static Dictionary<string, string> GetNoSpawn()
	{
		return NOSPAWN;
	}

	public bool RemoveStaff(Entity userEntity)
	{
		var removed = STAFF.Remove(userEntity.Read<User>().PlatformId.ToString());
		if (removed)
		{
			SaveStaff();
			Core.Log.LogWarning($"User {userEntity.Read<User>().CharacterName} removed from staff config.");
		}
		else
		{
			Core.Log.LogInfo($"User {userEntity.Read<User>().CharacterName} attempted to be removed from staff config but wasn't there.");
		}
		return removed;

	}
}
