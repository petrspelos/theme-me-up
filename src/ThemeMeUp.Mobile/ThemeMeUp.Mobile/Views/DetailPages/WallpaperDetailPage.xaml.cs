using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThemeMeUp.Mobile.Views.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WallpaperDetailPage : ContentPage
    {
        public WallpaperDetailPage(Wallpaper wallpaper)
        {
            var bindingContext = InversionOfControl.Container.Resolve<Func<Wallpaper, WallpaperDetailPageViewModel>>();
            BindingContext = bindingContext(wallpaper);
            InitializeComponent();
        }
    }
}