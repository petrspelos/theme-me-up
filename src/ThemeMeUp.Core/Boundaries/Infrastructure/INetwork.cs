using System.Threading.Tasks;

namespace ThemeMeUp.Core.Boundaries.Infrastructure
{
    public interface INetwork
    {
        Task<string> GetStringAsync(string url);
    }
}
