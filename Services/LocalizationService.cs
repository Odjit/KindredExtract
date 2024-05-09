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
        struct Code
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public string Description { get; set; }
        }

        struct Node
        {
            public string Guid { get; set; }
            public string Text { get; set; }
        }

        struct LocalizationFile
        {
            public Code[] Codes { get; set; }
            public Node[] Nodes { get; set; }
        }

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
                    var localizationFile = JsonSerializer.Deserialize<LocalizationFile>(jsonContent);
                    localization = localizationFile.Nodes.ToDictionary(x => x.Guid, x => x.Text);
                }
            }
            else
            {
                Console.WriteLine("Resource not found!");
            }
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
