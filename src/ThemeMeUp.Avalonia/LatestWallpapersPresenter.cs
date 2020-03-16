using System.Collections.Generic;
using System.Linq;
using MessageBox.Avalonia;
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
            if(!wallpapers.Any())
            {
                ShowSimpleMessage("No Results", "Wallhaven didn't find any wallpapers for your search.");
            }

            this.wallpapers = wallpapers;
        }

        public void NoConnection()
        {
            noConnection = true;
            ShowSimpleMessage("No connection", "We cannot fetch new wallpapers because there seems to be no Internet connection.");
        }

        public void Unauthenticated()
        {
            noApiKey = true;
            ShowSimpleMessage("No API Key", "You need an API key to search for NSFW wallpapers.");
        }

        public void Clear()
        {
            noConnection = false;
            noApiKey = false;
            wallpapers = new Wallpaper[0];
        }

        private void ShowSimpleMessage(string title, string content)
            => MessageBoxManager.GetMessageBoxStandardWindow(title, content).Show();
    }
}
