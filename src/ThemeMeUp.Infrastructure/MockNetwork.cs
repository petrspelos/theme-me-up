using System.IO;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.Infrastructure
{
    /// <summary>
    /// A mock of the Network abstraction so that you can use a JSON response from
    /// a file as opposed to connection to the Internet.
    ///
    /// This can be useful when you don't have Internet connection or when the
    /// API server rate limits you during debugging.
    ///
    /// You have to create the "mock-response.json" in the directory of the currently
    /// running front-end.
    /// </summary>
    public class MockNetwork : INetwork
    {
        public Task<string> GetStringAsync(string url)
        {
            return Task.FromResult(File.ReadAllText("mock-response.json"));
        }

        public Task<string> GetStringWithApiKeyAsync(string url, string apiKey) => GetStringAsync(url);
    }
}
