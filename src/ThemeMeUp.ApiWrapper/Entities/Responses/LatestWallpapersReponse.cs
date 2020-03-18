using ThemeMeUp.ApiWrapper.Entities.Api;

namespace ThemeMeUp.ApiWrapper.Entities.Responses
{
    public class LatestWallpapersResponse
    {
        public WallpaperResponse[] Data { get; set; }
        public ResultMeta Meta { get; set; }
    }
}
