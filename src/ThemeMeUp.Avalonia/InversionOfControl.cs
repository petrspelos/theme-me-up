using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
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
        private static ServiceProvider _provider;

        public static ServiceProvider Provider => GetOrInitProvider();

        private static ServiceProvider GetOrInitProvider()
        {
            if (_provider is null)
            {
                InitializeProvider();
            }

            return _provider;
        }

        private static void InitializeProvider()
            => _provider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddSingleton<UrlImageConverter>()
                .AddSingleton<WallpaperSetter>()
                .AddSingleton<INetwork, Network>()
                //.AddSingleton<INetwork, MockNetwork>()
                .AddSingleton<IWallhavenClient, WallhavenClient>()
                .AddSingleton<IWallpaperProvider, WallpaperProvider>()
                .AddSingleton<IGetLatestWallpapersUseCase, GetLatestWallpapersUseCase>()
                .AddSingleton<IGetLatestWallpapersOutputPort, LatestWallpapersPresenter>()
                .AddSingleton<IAuthentication, EnvironmentAuthentication>()
                .BuildServiceProvider();
    }
}