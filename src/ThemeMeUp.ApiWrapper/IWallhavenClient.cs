using System.Collections.Generic;
using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Api;

namespace ThemeMeUp.ApiWrapper
{
    public interface IWallhavenClient
    {
        Task<IEnumerable<WallpaperResponse>> GetLatestWallpapersAsync();
    }
}
