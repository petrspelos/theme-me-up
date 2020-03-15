using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries;
using ThemeMeUp.Core.Boundaries.GetLatestWallpapers;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.Entities.Exceptions;

namespace ThemeMeUp.Core.UseCases
{
    public class GetLatestWallpapersUseCase : IGetLatestWallpapersUseCase
    {
        private readonly INetwork _network;
        private readonly IGetLatestWallpapersOutputPort _output;
        private readonly IWallpaperProvider _wallpaperProvider;

        public GetLatestWallpapersUseCase(IWallpaperProvider wallpaperProvider, IGetLatestWallpapersOutputPort output)
        {
            _wallpaperProvider = wallpaperProvider;
            _output = output;
        }

        public async Task Execute(GetLatestWallpapersInput input)
        {
            try
            {
                var wallpapers = await _wallpaperProvider.GetLatestAsync(new SearchOptions
                {
                    Nsfw = input.Nsfw,
                    SearchTerm = input.SearchTerm
                });
                _output.Default(wallpapers);
            }
            catch (NoConnectionException)
            {
                _output.NoConnection();
            }
            catch (UnauthenticatedException)
            {
                _output.Unauthenticated();
            }
        }
    }
}