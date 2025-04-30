using Stunlock.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace KindredExtract.Services
{
    internal class LocalizationService
    {

        Dictionary<string, string> localization = [];

        public LocalizationService()
        {
            LoadLocalization();
        }

        void LoadLocalization()
        {
            var resourceName = "KindredExtract.Localization.English.json";

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                {
                    string jsonContent = reader.ReadToEnd();
                    localization = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                }
            }
            else
            {
                Console.WriteLine("Resource not found!");
            }
        }

        public void SaveLocalization()
        {
            localization.Clear();

            foreach(var entry in Localization._LocalizedStrings)
            {
                localization[entry.Key.ToGuid().ToString()] = entry.Value;
            }

            var json = JsonSerializer.Serialize(localization);
            File.WriteAllText($"{Localization.CurrentLanguage}.json", json);
        }

        public string GetLocalization(string guid)
        {
            if (localization.TryGetValue(guid, out var text))
            {
                return text;
            }
            return "<Localization not found!>";
        }
    }
}
