using ProjectM;
using ProjectM.Shared;
using ProjectM.UI;
using Stunlock.Core;
using Stunlock.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using VampireCommandFramework;

namespace KindredExtract.Commands
{
    [CommandGroup("dump")]
    internal class DumpCommands
    {

        [Command("prefabs", "p", description: "Dumps all prefabs to a file as prefabGuids for a Prefabs.cs file", adminOnly: true)]
        public static void DumpPrefabs(ChatCommandContext ctx)
        {
            var alreadyAddedNames = new HashSet<string>();
            var prefabs = new List<string>();
            var collectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();
            foreach (var prefabGuid in collectionSystem._PrefabLookupMap.GuidToEntityMap.GetKeyArray(Allocator.Temp))
            {
                var name = collectionSystem._PrefabLookupMap.GetName(prefabGuid);
                name = name.Replace(" ", "_").Replace(".", "_").Replace("-", "_").Replace("(", "_").Replace(")", "_");
                var nameBeforehand = name;
                var i = 2;
                while (alreadyAddedNames.Contains(name))
                {
                    name = $"{nameBeforehand}_ALREADY_EXISTS_{i++}";
                }
                alreadyAddedNames.Add(name);
                prefabs.Add($"\tpublic static readonly PrefabGUID {name} = new PrefabGUID({prefabGuid.GuidHash});");
            }
            prefabs.Sort();
            File.WriteAllLines("prefabs.txt", prefabs);
            ctx.Reply($"Dumped {prefabs.Count} prefabs to prefabs.txt");
        }

        [Command("types", "t", description: "Dumps all ECS component types to file (for usage in ComponentExtractors.tt)", adminOnly: true)]
        public static void DumpComponentTypes(ChatCommandContext ctx)
        {
            // Get all component types registered in the TypeManager
            var allTypes = TypeManager.GetAllTypes();

            // File path where you want to save the output
            string filePath = "componentTypes.txt";

            var count = 0;
            using (StreamWriter writer = new(filePath))
            {
                foreach (var typeInfo in allTypes)
                {
                    if (typeInfo.TypeIndex == 0)
                        continue;

                    // Get the actual type from the TypeInfo
                    var type = TypeManager.GetType(typeInfo.TypeIndex);

                    if (type == null || type.IsClass || type.IsEnum || !type.IsValueType)
                        continue;

                    // Write the type's full name to the file
                    writer.WriteLine($"    \"{type.FullName.Replace("+", ".")}\",");
                    count++;
                }
            }
            ctx.Reply($"Dumped {count} component types to {filePath}");
        }


        [Command("entityqueries", "eq", description: "Dumps all ECS entity queries to file", adminOnly: true)]
        public static void DumpEntityQueries(ChatCommandContext ctx)
        {
            var sb = new StringBuilder();
            SystemsQueryExtraction.DumpAllSystemQueries(sb);
            File.WriteAllText("EntityQueryDescriptions.txt", sb.ToString());
        }

        public static void DumpSystemQueries<T>(T system, StringBuilder sb) where T : ComponentSystemBase
        {
            if (system == null) return;
            var t = system.GetType();
            var unfilteredFields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var fields = unfilteredFields.Where(p => p.FieldType == typeof(EntityQuery));
            var unfilitedProperties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var properties = unfilitedProperties.Where(p => p.PropertyType == typeof(EntityQuery));

            if (!fields.Any() && !properties.Any()) return;

            sb.AppendLine($"System: {system.GetType().FullName}");
            foreach (var field in fields)
            {
                try
                {
                    var entityQuery = (EntityQuery)field.GetValue(system);
                    sb.AppendLine($"  EntityQuery Field: {field.Name}");
                    if (!entityQuery.IsCacheValid)
                    {
                        sb.AppendLine("    Invalid to use");
                        continue;
                    }
                    var queryDesc = entityQuery.GetEntityQueryDesc();
                    sb.AppendLine($"    Absent Components: {string.Join(", ", queryDesc.Absent.Select(c => c.ToString()))}");
                    sb.AppendLine($"    All Components: {string.Join(", ", queryDesc.All.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Any Components: {string.Join(", ", queryDesc.Any.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Disabled Components: {string.Join(", ", queryDesc.Disabled.Select(c => c.ToString()))}");
                    sb.AppendLine($"    None Components: {string.Join(", ", queryDesc.None.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Present Components: {string.Join(", ", queryDesc.Present.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Options: {queryDesc.Options}");
                }
                catch (Exception) { sb.AppendLine("    Invalid to use"); }
            }
            foreach (var property in properties)
            {
                try
                {
                    var entityQuery = (EntityQuery)property.GetValue(system);
                    sb.AppendLine($"  EntityQuery Property: {property.Name}");
                    if (!entityQuery.IsCacheValid)
                    {
                        sb.AppendLine("    Invalid to use");
                        continue;
                    }
                    var queryDesc = entityQuery.GetEntityQueryDesc();
                    sb.AppendLine($"    Absent Components: {string.Join(", ", queryDesc.Absent.Select(c => c.ToString()))}");
                    sb.AppendLine($"    All Components: {string.Join(", ", queryDesc.All.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Any Components: {string.Join(", ", queryDesc.Any.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Disabled Components: {string.Join(", ", queryDesc.Disabled.Select(c => c.ToString()))}");
                    sb.AppendLine($"    None Components: {string.Join(", ", queryDesc.None.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Present Components: {string.Join(", ", queryDesc.Present.Select(c => c.ToString()))}");
                    sb.AppendLine($"    Options: {queryDesc.Options}");
                }
                catch (Exception) { sb.AppendLine("    Invalid to use"); }
            }
            sb.AppendLine();
            sb.AppendLine();
        }

        [Command("prefabjsons", "pj", description: "Dumps all prefab names and ids to JSON files, grouped by prefix", adminOnly: true)]
        public static void DumpPrefabJsons(ChatCommandContext ctx)
        {
            var collectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();

            var prefabDictionaryByPrefix = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);
            foreach (var prefabGuid in collectionSystem._PrefabLookupMap.GuidToEntityMap.GetKeyArray(Allocator.Temp))
            {
                var name = collectionSystem._PrefabLookupMap.GetName(prefabGuid);
                // Prefix is either the first part of the name before an underscore or the first camel case word
                var prefix = name.Split('_')[0];

                // Next try finding prefix by finding the second uppercase letter after a lowercase
                var foundLower = false;
                for (int i = 1; i < prefix.Length; i++)
                {
                    if (char.IsUpper(name[i]))
                    {
                        if (foundLower)
                        {
                            prefix = name[..i];
                            break;
                        }
                    }
                    else
                    {
                        foundLower = true;
                    }
                }

                if (!prefabDictionaryByPrefix.TryGetValue(prefix, out var prefixDict))
                {
                    prefixDict = new Dictionary<string, int>();
                    prefabDictionaryByPrefix[prefix] = prefixDict;
                }
                prefixDict[name] = prefabGuid.GuidHash;
            }

            // Create a folder for the JSON files called prefabJsons if it doesn't exist
            if (!Directory.Exists("prefabJsons"))
                Directory.CreateDirectory("prefabJsons");

            // Output the prefix dicts out to JSON files
            foreach (var (prefix, prefixDict) in prefabDictionaryByPrefix)
            {
                var output = JsonSerializer.Serialize(prefixDict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value),
                                                      new JsonSerializerOptions() { WriteIndented = true });
                File.WriteAllText($"prefabJsons/{prefix}.json", output);
            }

            ctx.Reply($"Dumped {prefabDictionaryByPrefix.Count} JSON files to prefabJsons folder");
        }


        [Command("guidpos", adminOnly: true)]
        public static void DumpGuidPos(ChatCommandContext ctx, int prefab)
        {
            // Open up file prefab.csv for writing
            using (var writer = new StreamWriter($"{prefab}.csv"))
            {
                // Write the header
                writer.WriteLine("x,y,z");
                foreach (var entity in Helper.GetEntitiesByComponentType<PrefabGUID>())
                {
                    var prefabGuid = entity.Read<PrefabGUID>();
                    if (prefabGuid.GuidHash != prefab) continue;
                    if (!entity.Has<Translation>()) continue;
                    var translation = entity.Read<Translation>();

                    // Write the x, y, z coordinates to the file

                    writer.WriteLine($"{translation.Value.x},{translation.Value.y},{translation.Value.z}");

                }
            }
        }

        [Command("localization", adminOnly: true)]
        public static void DumpLocalization(ChatCommandContext ctx)
        {
            Core.Localization.SaveLocalization();
        }

        [Command("prefabnames", adminOnly: true)]
        public static void DumpPrefabNames(ChatCommandContext ctx)
        {
            var prefabCollectionSystem = Core.TheWorld.GetExistingSystemManaged<PrefabCollectionSystem>();
            var gameDataSystem = Core.TheWorld.GetExistingSystemManaged<GameDataSystem>();
            Dictionary<int, string> prefabNames = [];
            foreach (var prefabData in prefabCollectionSystem._PrefabGuidToEntityMap)
            {
                var prefabGuid = prefabData.Key;

                var managedCharacter = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedCharacterHUD>(prefabGuid);
                if (managedCharacter != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} ManagedCharacter");
                    prefabNames[prefabGuid.GuidHash] = managedCharacter.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedItemData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedItemData>(prefabGuid);
                if (managedItemData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedItemData");
                    prefabNames[prefabGuid.GuidHash] = managedItemData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedUnitBloodType = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedUnitBloodTypeData>(prefabGuid);
                if (managedUnitBloodType != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedUnitBloodType");
                    prefabNames[prefabGuid.GuidHash] = managedUnitBloodType.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedMissionData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedMissionData>(prefabGuid);
                if (managedMissionData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedMissionData");
                    prefabNames[prefabGuid.GuidHash] = managedMissionData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedTechData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedTechData>(prefabGuid);
                if (managedTechData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedTechData");
                    prefabNames[prefabGuid.GuidHash] = managedTechData.NameKey.Key.ToGuid().ToString();
                    continue;
                }

                var managedPerkData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedPerkData>(prefabGuid);
                if (managedPerkData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedPerkData");
                    prefabNames[prefabGuid.GuidHash] = managedPerkData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedBlueprintData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedBlueprintData>(prefabGuid);
                if (managedBlueprintData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedBlueprintData");
                    prefabNames[prefabGuid.GuidHash] = managedBlueprintData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedAbilityGroupData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedAbilityGroupData>(prefabGuid);
                if (managedAbilityGroupData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedAbilityGroupData");
                    prefabNames[prefabGuid.GuidHash] = managedAbilityGroupData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedDataDropGroup = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedDataDropGroup>(prefabGuid);
                if (managedDataDropGroup != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedDataDropGroup");
                    prefabNames[prefabGuid.GuidHash] = managedDataDropGroup.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedBuildMenuTagData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedBuildMenuTagData>(prefabGuid);
                if (managedBuildMenuTagData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedBuildMenuTagData");
                    prefabNames[prefabGuid.GuidHash] = managedBlueprintData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedBuildMenuGroupData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedBuildMenuGroupData>(prefabGuid);
                if (managedBuildMenuGroupData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedBuildMenuGroupData");
                    prefabNames[prefabGuid.GuidHash] = managedBuildMenuGroupData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedBuildMenuCategoryData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedBuildMenuCategoryData>(prefabGuid);
                if (managedBuildMenuCategoryData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedBuildMenuCategoryData");
                    prefabNames[prefabGuid.GuidHash] = managedBuildMenuCategoryData.Name.Key.ToGuid().ToString();
                    continue;
                }

                var managedSpellSchoolData = gameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<ManagedSpellSchoolData>(prefabGuid);
                if (managedSpellSchoolData != null)
                {
                    Core.Log.LogInfo($"{prefabGuid.LookupName()} managedSpellSchoolData");
                    prefabNames[prefabGuid.GuidHash] = managedSpellSchoolData.LongName.Key.ToGuid().ToString();
                    continue;
                }
            }

            var json = JsonSerializer.Serialize(prefabNames);
            File.WriteAllText($"PrefabNames.json", json);
        }
    }
}
