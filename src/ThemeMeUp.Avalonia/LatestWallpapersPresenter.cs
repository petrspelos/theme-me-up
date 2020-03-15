using System.Collections.Generic;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Avalonia
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public bool _noConnection = false;
        public IEnumerable<Wallpaper> _wallpapers;

        public void Default(IEnumerable<Wallpaper> wallpapers)
        {
            _wallpapers = wallpapers;
        }

        public void NoConnection()
        {
            _noConnection = true;
        }
    }
}
