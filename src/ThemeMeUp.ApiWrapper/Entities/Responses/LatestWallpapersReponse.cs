using System;
using System.Collections.Generic;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.ApiWrapper.Entities.Responses
{
    public class LatestWallpapersResponse
    {
        public WallpaperResponse[] Data { get; set; }
        public ResultMeta Meta { get; set; }

        internal IEnumerable<Wallpaper> Select()
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<Wallpaper> Select(object toModel)
        {
            throw new NotImplementedException();
        }
    }
}
