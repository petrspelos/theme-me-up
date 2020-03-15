namespace ThemeMeUp.Core.Entities
{
    public class SearchOptions
    {
        public bool Nsfw { get; set; }
        public bool Sfw { get; set; }
        public bool Sketchy { get; set; }
        public bool General { get; set; }
        public bool Anime { get; set; }
        public bool People { get; set; }
        public string SearchTerm { get; set; }
    }
}
