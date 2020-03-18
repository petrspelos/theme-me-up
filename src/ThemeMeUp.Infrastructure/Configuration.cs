using System;
using System.IO;
using Newtonsoft.Json;
using ThemeMeUp.Infrastructure.Entities;

namespace ThemeMeUp.Infrastructure
{
    public class Configuration
    {
        private readonly string _configFullPath;

        public Configuration()
        {
            var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(dataFolder, "theme-me-up");
            Directory.CreateDirectory(appFolder);
            _configFullPath = Path.Combine(appFolder, "config.json");
        }

        public JsonConfig GetConfig()
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

        public void StoreConfig(JsonConfig config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configFullPath, json);
        }

        private JsonConfig InitializeDefaultConfig()
        {
            var config = new JsonConfig();
            StoreConfig(config);
            return config;
        }
    }
}
