using System;
using System.Collections.Generic;
using System.Linq;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.ConsoleApp
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public static bool TrueRandom { get; set; }
        public static bool NewOrRandom { get; set; }

        private readonly IWallpaperSetter _setter;
        private readonly Random _rng;

        public LatestWallpapersPresenter(IWallpaperSetter setter, Random rng)
        {
            _setter = setter;
            _rng = rng;
        }

        public async void Default(IEnumerable<Wallpaper> wallpapers)
        {
            if(!wallpapers.Any())
            {
                Console.WriteLine("There are no wallpapers for your search.");
                return;
            }

            string wallpaperUrl = wallpapers.First().FullImageUrl;

            if(TrueRandom || (NewOrRandom && _setter.IsCached(wallpaperUrl)))
            {
                wallpaperUrl = wallpapers.RandomElement(_rng).FullImageUrl;
            }

            await _setter.SetFromUrlAsync(wallpaperUrl);
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
