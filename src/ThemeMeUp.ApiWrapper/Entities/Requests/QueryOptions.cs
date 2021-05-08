using ThemeMeUp.ApiWrapper.Entities.Api;
using ThemeMeUp.Core.Entities;
using ThemeMeUp.Core.Entities.Sorting;

namespace ThemeMeUp.ApiWrapper.Entities.Requests
{
    public class QueryOptions
    {
        public Purity Purity { get; set; }

        public Category Category { get; set; }

        public FuzzySearch FuzzySearch { get; set; }

        public IWallpaperSort Sort { get; set; }

        public ulong Page { get; set; }

        public QueryOptions(SearchOptions options)
        {
            Purity = new Purity
            {
                SFW = options.Sfw,
                Sketchy = options.Sketchy,
                NSFW = options.Nsfw
            };

            Category = new Category
            {
                General = options.General,
                Anime = options.Anime,
                People = options.People
            };

            FuzzySearch = new FuzzySearch
            {
                SearchTerm = options.SearchTerm
            };

            Sort = options.Sort;
            Page = options.Page;
        }

        public string ToQueryString() => $"{Purity.ToQueryParameter()}&{Category.ToQueryParameter()}&{FuzzySearch.ToQueryParameter()}&{Sort.ToQueryParameter()}&page={Page}";
    }
}
