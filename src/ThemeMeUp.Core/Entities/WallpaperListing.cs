using System.Collections.Generic;

namespace ThemeMeUp.Core.Entities
{
    public class WallpaperListing
    {
        public IEnumerable<Wallpaper> Wallpapers { get; set; }
        public WallpaperListingMeta Meta { get; set; }
    }
}
