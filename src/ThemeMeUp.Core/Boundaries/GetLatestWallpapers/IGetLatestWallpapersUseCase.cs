using System.Threading.Tasks;

namespace ThemeMeUp.Core.Boundaries.GetLatestWallpapers
{
    public interface IGetLatestWallpapersUseCase
    {
        Task Execute(GetLatestWallpapersInput input);
    }
}
