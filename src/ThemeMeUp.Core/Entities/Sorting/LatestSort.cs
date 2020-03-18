namespace ThemeMeUp.Core.Entities.Sorting
{
    public class LatestSort : IWallpaperSort
    {
        public string ToQueryParameter() => $"sorting=date_added";
    }
}
