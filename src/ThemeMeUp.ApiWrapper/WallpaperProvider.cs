using System;
using System.Net.Http;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Requests;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.Entities.Exceptions;

namespace ThemeMeUp.ApiWrapper
{
    public class WallpaperProvider : IWallpaperProvider
    {
        private readonly IWallhavenClient _client;

        public WallpaperProvider(IWallhavenClient client)
        {
            _client = client;
        }

        public async Task<WallpaperListing> GetLatestAsync(SearchOptions options)
        {
            if(options.Nsfw && !_client.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            var queryOptions = new QueryOptions(options);

            try
            {
                return await _client.GetLatestWallpapersAsync(queryOptions, options.RandomPage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                throw new NoConnectionException();
            }
        }
    }
}
