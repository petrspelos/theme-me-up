using System;
using System.Linq;
using System.Net;
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
            
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if(_client.DefaultRequestHeaders.Contains("X-API-Key"))
                {
                    var values = _client.DefaultRequestHeaders.GetValues("X-API-Key");
                    Console.WriteLine($"There are {values.Count()} values.");
                    Console.WriteLine(string.Join(", ", values));
                }

                return "";
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetStringWithApiKeyAsync(string url, string apiKey)
        {
            _client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            return await GetStringAsync(url);
        }
    }
}
