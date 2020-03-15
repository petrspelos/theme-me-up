using System;
using Newtonsoft.Json;
using ThemeMeUp.ApiWrapper.Converters;

namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class WallpaperResponse
    {
        public string Id { get; set; }

        public string Url { get; set; }

        [JsonProperty("short_url")]
        public string ShortUrl { get; set; }

        public ulong Views { get; set; }

        public ulong Favorites { get; set; }

        public string Source { get; set; }

        [JsonConverter(typeof(BinaryOptionConverter<Purity>))]
        public Purity Purity { get; set; }

        [JsonConverter(typeof(BinaryOptionConverter<Category>))]
        public Category Category { get; set; }

        [JsonProperty("dimension_x")]
        public uint DimensionX { get; set; }

        [JsonProperty("dimension_y")]
        public uint DimensionY { get; set; }

        public string Resolution { get; set; }

        public string Ratio { get; set; }

        [JsonProperty("file_size")]
        public uint FileSize { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public string[] Colors { get; set; }

        public string Path { get; set; }

        public Thumbnails Thumbs { get; set; }
    }
}
