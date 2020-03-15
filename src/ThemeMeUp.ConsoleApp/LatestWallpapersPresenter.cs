using System;
using System.Collections.Generic;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.ConsoleApp
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public void Default(IEnumerable<Wallpaper> wallpapers)
        {
            Console.WriteLine("LATEST WALLPAPERS:");

            foreach(var wallpaper in wallpapers)
            {
                Console.WriteLine(wallpaper.FullImageUrl);
            }
        }

        public void NoConnection()
        {
            Console.WriteLine("Internet connection is needed to fetch the latest wallpapers.");
        }

        public void Unauthenticated()
        {
            Console.WriteLine("An API key is required to get NSFW wallpapers.");
        }
    }
}
