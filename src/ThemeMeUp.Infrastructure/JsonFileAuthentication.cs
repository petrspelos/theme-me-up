using System.IO;
using Newtonsoft.Json;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Infrastructure.Entities;

namespace ThemeMeUp.Infrastructure
{
    public class JsonFileAuthentication : IAuthentication
    {
        const string ConfigFilePath = "config.json";

        public string GetApiKey()
        {
            var config = GetConfig();
            return config.WallhavenApiKey;
        }

        public void SetApiKey(string key)
        {
            var config = GetConfig();
            config.WallhavenApiKey = key;
            StoreConfig(config);
        }

        private static JsonConfig GetConfig()
        {
            try
            {
                var json = File.ReadAllText(ConfigFilePath);
                return JsonConvert.DeserializeObject<JsonConfig>(json);
            }
            catch (FileNotFoundException)
            {
                return InitializeDefaultConfig();
            }
        }

        private static JsonConfig InitializeDefaultConfig()
        {
            var config = new JsonConfig();
            StoreConfig(config);
            return config;
        }

        private static void StoreConfig(JsonConfig config)
        {
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigFilePath, json);
        }
    }
}
