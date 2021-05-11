using ThemeMeUp.Core.Entities;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class WallpaperDetailPageViewModel : BaseViewModel
    {
        public WallpaperDetailPageViewModel(Wallpaper wallpaper)
        {
            Wallpaper = wallpaper;
        }

        private Wallpaper _wallpaper;

        public Wallpaper Wallpaper
        {
            get => _wallpaper;
            set
            {
                _wallpaper = value;
                OnPropertyChanged();
            }
        }
    }
}