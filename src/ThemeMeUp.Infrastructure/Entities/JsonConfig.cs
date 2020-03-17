using Newtonsoft.Json;

namespace ThemeMeUp.Infrastructure.Entities
{
    public class JsonConfig
    {
        [JsonProperty("wallhaven_api_key")]
        public string WallhavenApiKey { get; set; } = string.Empty;
    }
}
