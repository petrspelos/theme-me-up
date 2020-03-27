using System.Threading.Tasks;

namespace ThemeMeUp.Core.Boundaries
{
    public interface IWallpaperSetter
    {
        bool IsCached(string url);
        Task SetFromUrlAsync(string url);
        void ApplyArgs(string[] args);
    }
}
