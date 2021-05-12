using System.Diagnostics;
using System.Threading.Tasks;
using DryIoc;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Mobile.Services.Implementations;
using ThemeMeUp.Mobile.ViewModels;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = InversionOfControl.Container.Resolve(typeof(MainPageViewModel), IfUnresolved.Throw);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel vm && vm.Wallpapers.Count == 0)
                vm.LoadWallpapersAsync()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}
