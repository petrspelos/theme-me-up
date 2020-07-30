using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DryIoc;
using ThemeMeUp.Avalonia.ViewModels;
using ThemeMeUp.Avalonia.Views;

namespace ThemeMeUp.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = InversionOfControl.Container.Resolve(typeof(MainWindowViewModel), IfUnresolved.Throw)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}