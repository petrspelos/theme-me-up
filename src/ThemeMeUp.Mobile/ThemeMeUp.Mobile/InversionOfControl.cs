using System;
using System.Net.Http;
using DryIoc;
using ThemeMeUp.ApiWrapper;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Infrastructure;
using ThemeMeUp.Mobile.Services;
using ThemeMeUp.Mobile.Services.Implementations;
using ThemeMeUp.Mobile.ViewModels;

namespace ThemeMeUp.Mobile
{
    public class InversionOfControl
    {
        private static IContainer _container;

        public static IContainer Container => GetOrInitContainer();

        public static IContainer InitialContainer { private get; set; }

        private static IContainer GetOrInitContainer()
        {
            if (_container is null)
                InitializeContainer();

            return _container;
        }

        private static void InitializeContainer()
        {
            _container = InitialContainer;
            _container.Register<MainPageViewModel>(Reuse.Singleton, FactoryMethod.ConstructorWithResolvableArguments);
            _container.Register<HttpClient>(Reuse.Singleton, FactoryMethod.ConstructorWithResolvableArguments);
            _container.Register<Random>(Reuse.Singleton, FactoryMethod.ConstructorWithResolvableArguments);
            _container.Register<WallpaperSetter>(Reuse.Transient);
            _container.Register<INetwork, Network>(Reuse.Transient);
            _container.Register<IWallhavenClient, WallhavenClient>(Reuse.Transient);
            _container.Register<IWallpaperProvider, WallpaperProvider>(Reuse.Transient);
            _container.Register<IGetLatestWallpapersUseCase, GetLatestWallpapersUseCase>(Reuse.Transient);
            _container.Register<IGetLatestWallpapersOutputPort, LatestWallpapersPresenter>(Reuse.Singleton);
            _container.Register<IAuthentication, SecureStorageAuthenticationService>(Reuse.Transient);
            _container.Register<INavigationService, NavigationService>(Reuse.Transient);
            _container.Register<IUserSettingsService, UserSettingsService>(Reuse.Transient);
            _container.Register<Configuration, Configuration>(Reuse.Transient);
        }
    }
}
