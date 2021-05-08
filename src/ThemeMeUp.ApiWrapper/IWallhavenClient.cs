using System.Threading.Tasks;
using ThemeMeUp.ApiWrapper.Entities.Requests;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.ApiWrapper
{
    public interface IWallhavenClient
    {
        Task<WallpaperListing> GetLatestWallpapersAsync(QueryOptions options, bool randomPage);
        bool IsAuthenticated();
    }
}
