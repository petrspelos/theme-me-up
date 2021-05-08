using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.ApiWrapper.Entities.Requests;
using ThemeMeUp.ApiWrapper.Entities.Responses;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.Entities.Exceptions;

namespace ThemeMeUp.ApiWrapper
{
    public class WallhavenClient : IWallhavenClient
    {
        private readonly INetwork _network;
        private readonly IAuthentication _auth;
        private readonly Random _rng;

        public WallhavenClient(INetwork network, IAuthentication auth, Random rng)
        {
            _network = network;
            _auth = auth;
            _rng = rng;
        }

        public async Task<WallpaperListing> GetLatestWallpapersAsync(QueryOptions options, bool randomPage)
        {
            var wallpapers = await GetWallpapersResponse(options);

            if(randomPage)
            {
                options.Page = _rng.NextULong(1, wallpapers.Meta.LastPage + 1);
                wallpapers = await GetWallpapersResponse(options);
            }
            
            return new WallpaperListing
            {
                Wallpapers = wallpapers.Data.Select(ToModel),
                Meta = new WallpaperListingMeta
                {
                    CurrentPage = wallpapers.Meta.CurrentPage,
                    TotalPages = wallpapers.Meta.LastPage
                }
            };
        }

        private Wallpaper ToModel(WallpaperResponse response)
        {
            return new Wallpaper
            {
                ShortUrl = response.ShortUrl,
                FullImageUrl = response.Path,
                SmallThumbnailUrl = response.Thumbs.Small,
                Views = response.Views,
                Favorites = response.Favorites,
            };
        }

        private async Task<LatestWallpapersResponse> GetWallpapersResponse(QueryOptions options)
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

            return JsonConvert.DeserializeObject<LatestWallpapersResponse>(json);
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
