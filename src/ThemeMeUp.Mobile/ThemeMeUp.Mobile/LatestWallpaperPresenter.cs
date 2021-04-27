using System.Collections.Generic;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Mobile
{
    public class LatestWallpaperPresenter : IGetLatestWallpapersOutputPort
    {
        public readonly ICollection<Wallpaper> Wallpapers = new List<Wallpaper>();

        public void Default(IEnumerable<Wallpaper> wallpapers)
        {
            Wallpapers.Clear();

            foreach (var wallpaper in wallpapers)
                Wallpapers.Add(wallpaper);
        }

        public void InvalidApiKey()
        {
        }

        public void NoConnection()
        {
        }

        public void Unauthenticated()
        {
        }
    }
}
