using System.Collections.Generic;
using System.Threading.Tasks;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Core.Boundaries
{
    public interface IWallpaperProvider
    {
        Task<IEnumerable<Wallpaper>> GetLatestAsync(SearchOptions options);
    }
}
