using System.Threading.Tasks;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Mobile.ViewModels;
using ThemeMeUp.Mobile.Views;
using ThemeMeUp.Mobile.Views.DetailPages;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        public async Task OpenFilterPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FilterPage());
        }

        public async Task OpenSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }

        public async Task OpenWallpaperDetailPageAsync(Wallpaper wallpaper)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new WallpaperDetailPage(wallpaper));
        }
    }
}
