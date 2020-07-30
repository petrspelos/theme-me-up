using DryIoc;
using System;
using System.Net.Http;
using ThemeMeUp.ApiWrapper;
using ThemeMeUp.Avalonia.Utilities;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Infrastructure;

namespace ThemeMeUp.Avalonia
{
    public static class InversionOfControl
    {
        private static IContainer _container;
        
        public static IContainer Container => GetOrInitContainer();

        private static IContainer GetOrInitContainer()
        {
            if (_container is null)
                InitializeContainer();

            return _container;
        }

        private static void InitializeContainer()
        {
            _container = new Container(Rules.Default.WithAutoConcreteTypeResolution());
            _container.Register<HttpClient>(Reuse.Singleton, FactoryMethod.ConstructorWithResolvableArguments);
            _container.Register<Random>(Reuse.Singleton, FactoryMethod.ConstructorWithResolvableArguments);
            _container.Register<UrlImageConverter>(Reuse.Transient);
            _container.Register<WallpaperSetter>(Reuse.Transient);
            _container.Register<INetwork, Network>(Reuse.Transient);
            _container.Register<IWallhavenClient, WallhavenClient>(Reuse.Transient);
            _container.Register<IWallpaperProvider, WallpaperProvider>(Reuse.Transient);
            _container.Register<IGetLatestWallpapersUseCase, GetLatestWallpapersUseCase>(Reuse.Transient);
            _container.Register<IGetLatestWallpapersOutputPort, LatestWallpapersPresenter>(Reuse.Singleton);
            _container.Register<IAuthentication, JsonFileAuthentication>(Reuse.Transient);
            _container.Register<IWallpaperSetter, WallpaperSetter>(Reuse.Transient);
            _container.Register<Configuration, Configuration>(Reuse.Transient);
        }
    }
}
