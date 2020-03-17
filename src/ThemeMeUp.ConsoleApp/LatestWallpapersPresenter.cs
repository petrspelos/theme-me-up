using System;
using System.Collections.Generic;
using System.Linq;
using ThemeMeUp.ConsoleApp.Utilities;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.ConsoleApp
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        private readonly WallpaperSetter _setter;

        public LatestWallpapersPresenter(WallpaperSetter setter)
        {
            _setter = setter;
        }

        public async void Default(IEnumerable<Wallpaper> wallpapers)
        {
            if(!wallpapers.Any())
            {
                Console.WriteLine("There are no wallpapers for your search.");
                return;
            }

            await _setter.SetFromUrlAsync(wallpapers.First().FullImageUrl);
        }

        public void InvalidApiKey()
        {
            Console.WriteLine("The provided API key was rejected by wallhaven.cc");
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
