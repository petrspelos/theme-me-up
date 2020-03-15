using System.Collections.Generic;
using System.Threading.Tasks;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Core.Boundaries.GetLatestWallpapers
{
    public interface IGetLatestWallpapersOutputPort
    {
        void NoConnection();
        void Default(IEnumerable<Wallpaper> wallpapers);
    }
}
