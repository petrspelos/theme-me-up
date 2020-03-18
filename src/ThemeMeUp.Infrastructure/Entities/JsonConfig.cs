using Newtonsoft.Json;

namespace ThemeMeUp.Infrastructure.Entities
{
    public class JsonConfig
    {
        [JsonProperty("wallhaven_api_key")]
        public string WallhavenApiKey { get; set; } = string.Empty;

        [JsonProperty("wallpaper_application")]
        public string WallpaperApp { get; set; } = "gsettings";

        [JsonProperty("wallpaper_application_arguments")]
        public string WallpaperAppArgs { get; set; } = "set org.gnome.desktop.background picture-uri {0}";
    }
}
