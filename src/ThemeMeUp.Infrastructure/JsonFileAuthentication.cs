using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.Infrastructure
{
    public class JsonFileAuthentication : IAuthentication
    {
        private readonly Configuration _config;

        public JsonFileAuthentication(Configuration config)
        {
            _config = config;

        }

        public string GetApiKey()
        {
            var config = _config.GetConfig();
            return config.WallhavenApiKey;
        }

        public void SetApiKey(string key)
        {
            var config = _config.GetConfig();
            config.WallhavenApiKey = key;
            _config.StoreConfig(config);
        }
    }
}
