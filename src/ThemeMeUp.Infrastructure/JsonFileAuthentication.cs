using System;
using System.IO;
using Newtonsoft.Json;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Infrastructure.Entities;

namespace ThemeMeUp.Infrastructure
{
    public class JsonFileAuthentication : IAuthentication
    {
        private readonly string _configFullPath;

        public JsonFileAuthentication()
        {
            var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(dataFolder, "theme-me-up");
            Directory.CreateDirectory(appFolder);
            _configFullPath = Path.Combine(appFolder, "config.json");
        }

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

        private JsonConfig GetConfig()
        {
            try
            {
                var json = File.ReadAllText(_configFullPath);
                return JsonConvert.DeserializeObject<JsonConfig>(json);
            }
            catch (FileNotFoundException)
            {
                return InitializeDefaultConfig();
            }
        }

        private JsonConfig InitializeDefaultConfig()
        {
            var config = new JsonConfig();
            StoreConfig(config);
            return config;
        }

        private void StoreConfig(JsonConfig config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configFullPath, json);
        }
    }
}
