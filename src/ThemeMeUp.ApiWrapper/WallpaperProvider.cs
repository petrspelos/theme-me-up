using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Api;
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

        public async Task<IEnumerable<Wallpaper>> GetLatestAsync()
        {
            try
            {
                return (await _client.GetLatestWallpapersAsync()).Select(ToWallpaper);
            }
            catch (HttpRequestException)
            {
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
