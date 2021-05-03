using System;

namespace ThemeMeUp.Core.Entities
{
    public class Wallpaper
    {
        public string ShortUrl { get; set; }
        public string FullImageUrl { get; set; }
        public string SmallThumbnailUrl { get; set; }
        public ulong Views { get; set; }
        public ulong Favorites { get; set; }
    }
}
    