using System.Collections.Generic;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Avalonia
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public bool noConnection = false;
        public bool noApiKey = false;
        public IEnumerable<Wallpaper> wallpapers;

        public void Default(IEnumerable<Wallpaper> wallpapers)
        {
            this.wallpapers = wallpapers;
        }

        public void NoConnection()
        {
            noConnection = true;
        }

        public void Unauthenticated()
        {
            noApiKey = true;
        }

        public void Clear()
        {
            noConnection = false;
            noApiKey = false;
            wallpapers = new Wallpaper[0];
        }
    }
}
