using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeMeUp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThemeMeUp.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        public FilterPage(MainPageViewModel vm)
        {
            var presenter = new LatestWallpaperPresenter();
            var useCase = UseCaseFactory.CreateGetLatestWallpapersUseCase(presenter);

            BindingContext = new FilterPageViewModel(useCase, presenter, vm);

            InitializeComponent();
        }
    }
}