using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.ApiWrapper.Entities.Responses;
using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.ApiWrapper
{
    public class WallhavenClient : IWallhavenClient
    {
        private readonly INetwork _network;

        public WallhavenClient(INetwork network)
        {
            _network = network;
        }

        public async Task<IEnumerable<WallpaperResponse>> GetLatestWallpapersAsync()
        {
            var json = await _network.GetStringAsync(@"https://wallhaven.cc/api/v1/search");
            var latestWallpapers = JsonConvert.DeserializeObject<LatestWallpapersResponse>(json);
            return latestWallpapers.Data;
        }
    }
}
