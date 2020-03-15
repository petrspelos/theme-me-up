namespace ThemeMeUp.Core.Boundaries.GetLatestWallpapers
{
    public class GetLatestWallpapersInput
    {
        public bool Sfw { get; set; }
        public bool Sketchy { get; set; }
        public bool Nsfw { get; set; }
        public string SearchTerm { get; set; }
    }
}
