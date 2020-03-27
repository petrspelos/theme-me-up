using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Api;
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

        public async Task<IEnumerable<Wallpaper>> GetLatestAsync(SearchOptions options)
        {
            if(options.Nsfw && !_client.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            var queryOptions = new QueryOptions(options);

            try
            {
                return (await _client.GetLatestWallpapersAsync(queryOptions, options.RandomPage)).Select(ToWallpaper);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                throw new NoConnectionException();
            }
        }

        private Wallpaper ToWallpaper(WallpaperResponse response)
        {
            return new Wallpaper
            {
                FullImageUrl = response.Path,
                SmallThumbnailUrl = response.Thumbs.Small
            };
        }
    }
}
