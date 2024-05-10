using ProjectM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Unity.Entities;
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
            var collectionSystem = Core.Server.GetExistingSystemManaged<PrefabCollectionSystem>();
            foreach (var item in collectionSystem.PrefabGuidToNameDictionary)
            {
                var name = item.value.Replace(" ", "_").Replace(".", "_").Replace("-", "_").Replace("(", "_").Replace(")", "_");
                var nameBeforehand = name;
                var i = 2;
                while (alreadyAddedNames.Contains(name))
                {
                    name = $"{nameBeforehand}_ALREADY_EXISTS_{i++}";
                }
                alreadyAddedNames.Add(name);
                prefabs.Add($"\tpublic static readonly PrefabGUID {name} = new PrefabGUID({item.Key.GuidHash});");
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

        /*
        [Command("dumpentityqueries", "deq", description: "Dumps all ECS entity queries to file", adminOnly: true)]
        public static void DumpEntityQueries(ChatCommandContext ctx)
        {
            var sb = new System.Text.StringBuilder();

            // Get all types that are a subclass of SystemBase
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly.ToString().Contains("Unity"))
                    continue;
                try
                {
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(SystemBase)))
                        {
                            var system = Core.Server.GetExistingSystemManaged(Il2CppType.From(type));
                            if (system != null)
                            {
                                var properties = system.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .Where(p => p.PropertyType == typeof(EntityQuery));

                                if(!properties.Any())
                                    continue;

                                sb.AppendLine($"System: {type.FullName}");
                                foreach (var property in properties)
                                {
                                    var entityQuery = (EntityQuery)property.GetValue(system);
                                    sb.AppendLine($"  EntityQuery Property: {property.Name}");
                                    var queryDesc = entityQuery.GetEntityQueryDesc();
                                    sb.AppendLine($"    Any Components: {string.Join(", ", queryDesc.Any.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    None Components: {string.Join(", ", queryDesc.None.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    All Components: {string.Join(", ", queryDesc.All.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    Disabled Components: {string.Join(", ", queryDesc.Disabled.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    Absent Components: {string.Join(", ", queryDesc.Absent.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    Present Components: {string.Join(", ", queryDesc.Present.Select(c => c.ToString()))}");
                                    sb.AppendLine($"    Options: {queryDesc.Options}");
                                }
                                sb.AppendLine();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ctx.Reply($"Error processing assembly {assembly.FullName}: {e.Message}");
                }
            }

            File.WriteAllText("EntityQueryDescriptions.txt", sb.ToString());
        }*/

        [Command("prefabjsons", "pj", description: "Dumps all prefab names and ids to JSON files, grouped by prefix", adminOnly: true)]
        public static void DumpPrefabJsons(ChatCommandContext ctx)
        {
            var collectionSystem = Core.Server.GetExistingSystemManaged<PrefabCollectionSystem>();
            var prefabs = collectionSystem.PrefabGuidToNameDictionary;

            var prefabDictionaryByPrefix = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);
            foreach (var (prefabGuid, name) in prefabs)
            {
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
    }
}
