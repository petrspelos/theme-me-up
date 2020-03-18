using Newtonsoft.Json;

namespace ThemeMeUp.ApiWrapper.Entities.Api
{
    public class ResultMeta
    {
        [JsonProperty("current_page")]
        public ulong CurrentPage { get; set; }

        [JsonProperty("last_page")]
        public ulong LastPage { get; set; }

        [JsonProperty("per_page")]
        public ulong PerPage { get; set; }

        [JsonProperty("totla")]
        public ulong Total { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("seed")]
        public string Seed { get; set; }
    }
}
