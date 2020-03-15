using System.Collections.Generic;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.ApiWrapper.Entities.Requests;

namespace ThemeMeUp.ApiWrapper
{
    public interface IWallhavenClient
    {
        Task<IEnumerable<WallpaperResponse>> GetLatestWallpapersAsync(QueryOptions options);
        bool IsAuthenticated();
    }
}
