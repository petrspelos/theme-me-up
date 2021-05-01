using System.Threading.Tasks;
using ThemeMeUp.Mobile.Views;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        public async Task OpenFilterPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FilterPage());
        }
    }
}
