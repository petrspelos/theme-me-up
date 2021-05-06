using DryIoc;
using ThemeMeUp.Mobile.Views;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile
{
    public partial class App : Application
    {
        public App(IContainer initialContainer)
        {
            InitializeComponent();
            InversionOfControl.InitialContainer = initialContainer;

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
