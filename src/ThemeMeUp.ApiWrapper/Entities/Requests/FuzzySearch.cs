using System.Web;

namespace ThemeMeUp.ApiWrapper.Entities.Requests
{
    public class FuzzySearch
    {
        public string SearchTerm { get; set; }

        public string ToQueryParameter() => $"q={HttpUtility.UrlEncode(SearchTerm)}";
    }
}
