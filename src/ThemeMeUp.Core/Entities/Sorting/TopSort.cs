namespace ThemeMeUp.Core.Entities.Sorting
{
    public class TopSort : IWallpaperSort
    {
        private readonly TopSortRange _range;

        public TopSort(TopSortRange range = TopSortRange.Month)
        {
            _range = range;
        }

        public string ToQueryParameter() => $"sorting=toplist&topRange={RangeToValue(_range)}";

        private static string RangeToValue(TopSortRange range)
            => range switch 
            {
                TopSortRange.Day => "1d",
                TopSortRange.ThreeDays => "3d",
                TopSortRange.Week => "1w",
                TopSortRange.Month => "1M",
                TopSortRange.ThreeMonths => "3M",
                TopSortRange.HalfYear => "6M",
                TopSortRange.Year => "1y",
                _ => "1M"
            };
    }
}
