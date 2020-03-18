using System.Threading.Tasks;

namespace ThemeMeUp.Core.Boundaries
{
    public interface IWallpaperSetter
    {
        Task SetFromUrlAsync(string url);
        void ApplyArgs(string[] args);
    }
}
