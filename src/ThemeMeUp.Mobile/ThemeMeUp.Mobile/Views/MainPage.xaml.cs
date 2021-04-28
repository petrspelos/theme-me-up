using System.Diagnostics;
using System.Threading.Tasks;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Mobile.ViewModels;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var presenter = new LatestWallpaperPresenter();
            var useCase = UseCaseFactory.CreateGetLatestWallpapersUseCase(presenter);

            BindingContext = new MainPageViewModel(useCase, presenter); 

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel vm && vm.Wallpapers.Count == 0)
                vm.GetLatestWallpapersAsync()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}
