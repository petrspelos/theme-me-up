using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.ApiWrapper.Entities.Requests;
using ThemeMeUp.ApiWrapper.Entities.Responses;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.Entities.Exceptions;

namespace ThemeMeUp.ApiWrapper
{
    public class WallhavenClient : IWallhavenClient
    {
        private readonly INetwork _network;
        private readonly IAuthentication _auth;

        public WallhavenClient(INetwork network, IAuthentication auth)
        {
            _network = network;
            _auth = auth;
        }

        public async Task<IEnumerable<WallpaperResponse>> GetLatestWallpapersAsync(QueryOptions options)
        {
            var url = $@"https://wallhaven.cc/api/v1/search?{options.ToQueryString()}";

            string json;
            if(options.Purity.NSFW)
            {
                json = await _network.GetStringWithApiKeyAsync(url, _auth.GetApiKey());
            }
            else
            {
                json = await _network.GetStringAsync(url);
            }

            AssertNotErrorResponse(json);

            var latestWallpapers = JsonConvert.DeserializeObject<LatestWallpapersResponse>(json);
            return latestWallpapers.Data;
        }

        public bool IsAuthenticated() => !string.IsNullOrWhiteSpace(_auth.GetApiKey());

        private void AssertNotErrorResponse(string json)
        {
            if(!json.Contains("error"))
            {
                return;
            }

            var error = JsonConvert.DeserializeObject<ErrorResponse>(json);

            if(error != null)
            {
                throw new InvalidApiKeyException();
            }
        }
    }
}
