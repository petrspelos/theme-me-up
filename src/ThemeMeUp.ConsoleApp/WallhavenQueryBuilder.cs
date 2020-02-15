using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ThemeMeUp.ConsoleApp
{
    public class WallhavenQueryBuilder
    {
        private ICollection<string> options = new Collection<string>
        {
            Constants.SortingRandomOption
        };

        public string BuildUrl()
        {
            return Constants.WallhavenRootUrl + Constants.SearchOptionsRoot + string.Join(Constants.OptionSeparator, options);
        }

        public WallhavenQueryBuilder MinimumResolution(int width, int height)
        {
            options.Add(string.Format(Constants.MinResolutionOptionTemplate, width, height));
            return this;
        }
    }
}
