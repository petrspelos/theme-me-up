using System.Collections.Generic;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Mobile
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public readonly ICollection<Wallpaper> Wallpapers = new List<Wallpaper>();

        public void Default(WallpaperListing listing)
        {
            Wallpapers.Clear();

            foreach (var wallpaper in listing.Wallpapers)
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
