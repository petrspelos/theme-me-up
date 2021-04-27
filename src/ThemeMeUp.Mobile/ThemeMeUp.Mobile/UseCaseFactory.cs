using System.Net.Http;
using ThemeMeUp.ApiWrapper;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.UseCases;
using ThemeMeUp.Infrastructure;

namespace ThemeMeUp.Mobile
{
    internal static class UseCaseFactory
    {
        internal static IGetLatestWallpapersUseCase CreateGetLatestWallpapersUseCase(IGetLatestWallpapersOutputPort outputPort)
        {
            var network = new Network(new HttpClient());
            var auth = new EnvironmentAuthentication();
            var whClient = new WallhavenClient(network, auth, new System.Random());
            var wallpaperProvider = new WallpaperProvider(whClient);
            return new GetLatestWallpapersUseCase(wallpaperProvider, outputPort);
        }
    }
}
