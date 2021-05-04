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
    public partial class FilterPage : ContentPage
    {
        public FilterPage()
        {
            BindingContext = InversionOfControl.Container.Resolve(typeof(FilterPageViewModel), IfUnresolved.Throw);
            InitializeComponent();
        }
    }
}