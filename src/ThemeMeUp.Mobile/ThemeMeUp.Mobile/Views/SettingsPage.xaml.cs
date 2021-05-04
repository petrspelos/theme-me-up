using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using ThemeMeUp.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThemeMeUp.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            BindingContext = InversionOfControl.Container.Resolve(typeof(SettingsPageViewModel), IfUnresolved.Throw);
            InitializeComponent();
        }
    }
}