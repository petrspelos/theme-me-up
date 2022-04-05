using System.Collections.Generic;
using System.Linq;
using MessageBox.Avalonia;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Avalonia
{
    public class LatestWallpapersPresenter : IGetLatestWallpapersOutputPort
    {
        public bool HasNoConnection { get; private set; }
        public bool HasNoApiKey { get; private set; }
        public IEnumerable<Wallpaper> Wallpapers { get; private set; }

        public void Default(IEnumerable<Wallpaper> wallpapers)
        {
            if (!wallpapers.Any())
            {
                ShowSimpleMessage("No Results", "Wallhaven didn't find any wallpapers for your search.");
            }

            Wallpapers = wallpapers;
        }

        public void NoConnection()
        {
            HasNoConnection = true;
            ShowSimpleMessage("No connection", "We cannot fetch new wallpapers because there seems to be no Internet connection.");
        }

        public void Unauthenticated()
        {
            HasNoApiKey = true;
            ShowSimpleMessage("No API Key", "You need an API key to search for NSFW wallpapers.");
        }

        public void Clear()
        {
            HasNoConnection = false;
            HasNoApiKey = false;
            Wallpapers = System.Array.Empty<Wallpaper>();
        }

        private void ShowSimpleMessage(string title, string content)
            => MessageBoxManager.GetMessageBoxStandardWindow(title, content).Show();

        public void InvalidApiKey()
        {
            ShowSimpleMessage("Invalid API Key", "The provided API key was rejected by wallhaven.");
        }
    }
}
