using System.Net.Http;
using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.Infrastructure
{
    public class Network : INetwork
    {
        private readonly HttpClient _client;

        public Network(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetStringAsync(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}